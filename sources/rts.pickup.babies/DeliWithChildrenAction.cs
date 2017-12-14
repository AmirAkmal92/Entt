using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Polly;

namespace Bespoke.PosEntt.CustomActions
{
    [Export(typeof(CustomAction))]
    [DesignerMetadata(Name = "DeliWithChildren", TypeName = "Bespoke.PosEntt.CustomActions.DeliWithChildrenAction, rts.pickup.babies", Description = "RTS DELI with child items", FontAwesomeIcon = "car")]
    public class DeliWithChildrenAction : EventWithChildrenAction
    {
        private List<Adapters.Oal.dbo_delivery_event_new> m_deliEventRows;
        private List<Adapters.Oal.dbo_wwp_event_new_log> m_deliWwpEventLogRows;
        private List<Adapters.Oal.dbo_event_pending_console> m_deliEventPendingConsoleRows;
        private List<Adapters.Oal.dbo_ips_import> m_deliIpsImportEventRows;

        public override async Task ExecuteAsync(RuleContext context)
        {
            var deli = context.Item as Deliveries.Domain.Delivery;
            if (null == deli) return;
            var isConsole = IsConsole(deli.ConsignmentNo);
            if (!isConsole) return;

            m_deliEventRows = new List<Adapters.Oal.dbo_delivery_event_new>();
            m_deliWwpEventLogRows = new List<Adapters.Oal.dbo_wwp_event_new_log>();
            m_deliEventPendingConsoleRows = new List<Adapters.Oal.dbo_event_pending_console>();
            m_deliIpsImportEventRows = new List<Adapters.Oal.dbo_ips_import>();

            await RunAsync(deli);
        }

        public async Task RunAsync(Deliveries.Domain.Delivery deli)
        {
            //console_details
            var consoleList = new List<string>();
            if (IsConsole(deli.ConsignmentNo)) consoleList.Add(deli.ConsignmentNo);

            var deliEventAdapter = new Adapters.Oal.dbo_delivery_event_newAdapter();
            var deliWwpEventAdapter = new Adapters.Oal.dbo_wwp_event_new_logAdapter();
            var deliEventMap = new Integrations.Transforms.RtsDeliveryToOalDboDeliveryEventNew();
            var deliIpsImportMap = new Integrations.Transforms.RtsDeliveryToIpsImport();
            var parentRow = await deliEventMap.TransformAsync(deli);
            var parentIpsRow = await deliIpsImportMap.TransformAsync(deli);

            parentRow.id = GenerateId(34);
            m_deliEventRows.Add(parentRow);

            var deliWwpEventLogMap = new Integrations.Transforms.RtsDeliveryOalWwpEventNewLog();
            var parentWwpRow = await deliWwpEventLogMap.TransformAsync(deli);
            m_deliWwpEventLogRows.Add(parentWwpRow);

            var consoleItem = await GetItemConsigmentsFromConsoleDetailsAsync(deli.ConsignmentNo);
            if (null != consoleItem)
            {
                var children = consoleItem.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in children)
                {
                    if (consoleList.Contains(item)) continue;
                    ProcessChild(parentRow, item);
                    ProcessChildWwp(parentWwpRow, item);
                    if (IsIpsImportItem(item))
                    {
                        await ProcessChildIpsImport(parentIpsRow, item);
                    }

                    //2 level
                    var console = IsConsole(item);
                    if (console)
                    {
                        consoleList.Add(item);
                        var childConsole = await GetItemConsigmentsFromConsoleDetailsAsync(item);
                        if (null != childConsole)
                        {
                            var childConsoleItems = childConsole.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var cc in childConsoleItems)
                            {
                                if (consoleList.Contains(cc)) continue;
                                ProcessChild(parentRow, cc);
                                ProcessChildWwp(parentWwpRow, cc);
                                if (IsIpsImportItem(cc))
                                {
                                    await ProcessChildIpsImport(parentIpsRow, cc);
                                }

                                //3 level
                                var anotherConsole = IsConsole(cc);
                                if (anotherConsole)
                                {
                                    consoleList.Add(cc);
                                    var anotherChildConsole = await GetItemConsigmentsFromConsoleDetailsAsync(cc);
                                    if (null != anotherChildConsole)
                                    {
                                        var anotherChildConsoleItems = anotherChildConsole.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (var ccc in anotherChildConsoleItems)
                                        {
                                            if (consoleList.Contains(ccc)) continue;
                                            ProcessChild(parentRow, ccc);
                                            ProcessChildWwp(parentWwpRow, ccc);
                                            if (IsIpsImportItem(ccc))
                                            {
                                                await ProcessChildIpsImport(parentIpsRow, ccc);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        AddPendingItems(parentRow.id, parentWwpRow.id, cc);
                                    }
                                }
                            }
                        }
                        else
                        {
                            AddPendingItems(parentRow.id, parentWwpRow.id, item);
                        }
                    }
                }
            }
            else
            {
                AddPendingItems(parentRow.id, parentWwpRow.id, deli.ConsignmentNo);
            }

            foreach (var item in m_deliEventRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => deliEventAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }

            foreach (var item in m_deliWwpEventLogRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => deliWwpEventAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }

            var ipsAdapter = new Adapters.Oal.dbo_ips_importAdapter();
            foreach (var item in m_deliIpsImportEventRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => ipsAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }

            var pendingAdapter = new Adapters.Oal.dbo_event_pending_consoleAdapter();
            foreach (var item in m_deliEventPendingConsoleRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => pendingAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }

        private void ProcessChild(Adapters.Oal.dbo_delivery_event_new parent, string consignmentNo)
        {
            var console = IsConsole(consignmentNo);
            var child = parent.Clone();
            child.id = GenerateId(34);
            child.consignment_no = consignmentNo;
            child.data_flag = "1";
            child.item_type_code = console ? "02" : "01";
            child.date_created_o_a_l_date_field = DateTime.Now;
            m_deliEventRows.Add(child);
        }

        private void AddPendingItems(string parentId, string wwpParentId, string consignmentNo)
        {
            //insert into pending items
            var pendingConsole = new Adapters.Oal.dbo_event_pending_console
            {
                id = GenerateId(20),
                event_id = parentId,
                event_class = "pos.oal.DeliveryEventNew",
                console_no = consignmentNo,
                version = 0,
                date_field = DateTime.Now
            };
            m_deliEventPendingConsoleRows.Add(pendingConsole);

            var pendingWwp = new Adapters.Oal.dbo_event_pending_console
            {
                id = GenerateId(20),
                event_id = wwpParentId,
                event_class = "pos.oal.WwpEventNewLog",
                console_no = consignmentNo,
                version = 0,
                date_field = DateTime.Now
            };
            m_deliEventPendingConsoleRows.Add(pendingWwp);
        }

        private void ProcessChildWwp(Adapters.Oal.dbo_wwp_event_new_log sipWwp, string childConnoteNo)
        {
            var wwp = sipWwp.Clone();
            wwp.id = GenerateId(34);
            wwp.consignment_note_number = childConnoteNo;
            wwp.dt_created_oal_date_field = DateTime.Now;
            m_deliWwpEventLogRows.Add(wwp);
        }

        private async Task ProcessChildIpsImport(Adapters.Oal.dbo_ips_import parent, string consignmentNo)
        {
            var child = parent.Clone();
            child.id = GenerateId(13);
            child.item_id = consignmentNo;
            child.data_code_name = "deli";
            child.class_cd = GetClassCode(consignmentNo);

            var consignmentInitialAdapter = new Adapters.Oal.dbo_consignment_initialAdapter();
            var consignmentInitialPolly = await Policy.Handle<SqlException>()
                .WaitAndRetryAsync(3, c => TimeSpan.FromMilliseconds(c * 500))
                .ExecuteAndCaptureAsync(async () => await SearchConsignmentInitialAsync(consignmentNo));
            if (null != consignmentInitialPolly.FinalException)
                throw new Exception("Process child IPS import Polly failed", consignmentInitialPolly.FinalException);

            var consignment = consignmentInitialPolly.Result;
            if (null != consignment)
            {
                child.item_weight_double = consignment.weight_double;
                if (!string.IsNullOrEmpty(consignment.shipper_address_country))
                {
                    child.orig_country_cd = consignment.shipper_address_country;
                }
                if (!string.IsNullOrEmpty(consignment.item_category))
                    child.content = consignment.item_category.Equals("01") ? "M" : "D";
                else
                    child.content = "D";
            }

            if (string.IsNullOrWhiteSpace(child.orig_country_cd))
            {
                var pattern = @"\w{2}\d{9}(?<country>\w{2})";
                var match = System.Text.RegularExpressions.Regex.Match(consignmentNo, pattern);

                if (match.Success)
                {
                    child.orig_country_cd = match.Groups["country"].Value;
                }
            }

            if (null == child.item_weight_double) child.item_weight_double = 0d;

            m_deliIpsImportEventRows.Add(child);
        }

        
    }
}