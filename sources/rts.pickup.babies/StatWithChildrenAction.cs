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
    [DesignerMetadata(Name = "StatWithChildren", TypeName = "Bespoke.PosEntt.CustomActions.StatWithChildrenAction, rts.pickup.babies", Description = "RTS STAT with child items", FontAwesomeIcon = "calendar-check-o")]
    public class StatWithChildrenAction : EventWithChildrenAction
    {
        private List<Adapters.Oal.dbo_status_code_event_new> m_statEventRows;
        private List<Adapters.Oal.dbo_wwp_event_new_log> m_statWwpEventLogRows;
        private List<Adapters.Oal.dbo_event_pending_console> m_statEventPendingConsoleRows;
        private List<Adapters.Oal.dbo_ips_import> m_statIpsImportRows;

        public override async Task ExecuteAsync(RuleContext context)
        {
            var stat = context.Item as Stats.Domain.Stat;
            if (null == stat) return;
            var isConsole = IsConsole(stat.ConsignmentNo);
            if (!isConsole) return;

            m_statEventRows = new List<Adapters.Oal.dbo_status_code_event_new>();
            m_statWwpEventLogRows = new List<Adapters.Oal.dbo_wwp_event_new_log>();
            m_statEventPendingConsoleRows = new List<Adapters.Oal.dbo_event_pending_console>();
            m_statIpsImportRows = new List<Adapters.Oal.dbo_ips_import>();

            await RunAsync(stat);
        }

        public async Task RunAsync(Stats.Domain.Stat stat)
        {
            //console_details
            var consoleList = new List<string>();
            if (IsConsole(stat.ConsignmentNo)) consoleList.Add(stat.ConsignmentNo);

            var statEventAdapter = new Adapters.Oal.dbo_status_code_event_newAdapter();
            var statWwpEventAdapter = new Adapters.Oal.dbo_wwp_event_new_logAdapter();
            var statIpsImportEventAdapter = new Adapters.Oal.dbo_ips_importAdapter();

            var statEventMap = new Integrations.Transforms.RtsStatToOalStatusCodeEventNew();
            var parentRow = await statEventMap.TransformAsync(stat);
            var ipsEventMap = new Integrations.Transforms.RtsStatToOalIpsImport();
            var ipsParentStatus = await ipsEventMap.TransformAsync(stat);

            parentRow.id = GenerateId(34);
            m_statEventRows.Add(parentRow);

            var statWwpEventLogMap = new Integrations.Transforms.RtsStatOalWwpEventNewLog();
            var parentWwpRow = await statWwpEventLogMap.TransformAsync(stat);
            m_statWwpEventLogRows.Add(parentWwpRow);

            var consoleItem = await GetItemConsigmentsFromConsoleDetailsAsync(stat.ConsignmentNo);
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
                        ProcessChildIpsImport(ipsParentStatus, item);
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
                                    ProcessChildIpsImport(ipsParentStatus, cc);
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
                                                ProcessChildIpsImport(ipsParentStatus, ccc);
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
                AddPendingItems(parentRow.id, parentWwpRow.id, stat.ConsignmentNo);
            }

            foreach (var item in m_statEventRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => statEventAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }

            foreach (var item in m_statWwpEventLogRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => statWwpEventAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }

            foreach (var item in m_statIpsImportRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => statIpsImportEventAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }

            var pendingAdapter = new Adapters.Oal.dbo_event_pending_consoleAdapter();
            foreach (var item in m_statEventPendingConsoleRows)
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

     

        private void ProcessChild(Adapters.Oal.dbo_status_code_event_new parent, string consignmentNo)
        {
            var console = IsConsole(consignmentNo);
            var child = parent.Clone();
            child.id = GenerateId(34);
            child.consignment_no = consignmentNo;
            child.data_flag = "1";
            child.item_type_code = console ? "02" : "01";
            m_statEventRows.Add(child);
        }

        private void AddPendingItems(string parentId, string wwpParentId, string consignmentNo)
        {
            //insert into pending items
            var pendingConsole = new Adapters.Oal.dbo_event_pending_console
            {
                id = GenerateId(20),
                event_id = parentId,
                event_class = "pos.oal.StatusEventNew",
                console_no = consignmentNo,
                version = 0,
                date_field = DateTime.Now
            };
            m_statEventPendingConsoleRows.Add(pendingConsole);

            var pendingWwp = new Adapters.Oal.dbo_event_pending_console
            {
                id = GenerateId(20),
                event_id = wwpParentId,
                event_class = "pos.oal.WwpEventNewLog",
                console_no = consignmentNo,
                version = 0,
                date_field = DateTime.Now
            };
            m_statEventPendingConsoleRows.Add(pendingWwp);
        }

        private void ProcessChildWwp(Adapters.Oal.dbo_wwp_event_new_log statWwp, string childConnoteNo)
        {
            var wwp = statWwp.Clone();
            wwp.id = GenerateId(34);
            wwp.consignment_note_number = childConnoteNo;
            m_statWwpEventLogRows.Add(wwp);
        }

        private void ProcessChildIpsImport(Adapters.Oal.dbo_ips_import parent, string consignmentNo)
        {
            var console = IsConsole(consignmentNo);
            var child = parent.Clone();
            child.id = GenerateId(13);
            child.item_id = consignmentNo;
            m_statIpsImportRows.Add(child);
        }

    }
}