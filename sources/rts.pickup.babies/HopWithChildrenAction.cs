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
    [DesignerMetadata(Name = "HopWithChildren", TypeName = "Bespoke.PosEntt.CustomActions.HopWithChildrenAction, rts.pickup.babies", Description = "RTS HOP with child items", FontAwesomeIcon = "calendar-check-o")]
    public class HopWithChildrenAction : EventWithChildrenAction
    {
        private List<Adapters.Oal.dbo_hop_event_new> m_hopEventRows;
        private List<Adapters.Oal.dbo_wwp_event_new_log> m_hopWwpEventLogRows;
        private List<Adapters.Oal.dbo_event_pending_console> m_hopEventPendingConsoleRows;

        public override async Task ExecuteAsync(RuleContext context)
        {
            var hop = context.Item as Hops.Domain.Hop;
            if (null == hop) return;
            var isConsole = IsConsole(hop.ConsignmentNo);
            if (!isConsole) return;

            m_hopEventRows = new List<Adapters.Oal.dbo_hop_event_new>();
            m_hopWwpEventLogRows = new List<Adapters.Oal.dbo_wwp_event_new_log>();
            m_hopEventPendingConsoleRows = new List<Adapters.Oal.dbo_event_pending_console>();

            await RunAsync(hop);
        }

        public async Task RunAsync(Hops.Domain.Hop hop)
        {
            //console_details
            var consoleList = new List<string>();
            if (IsConsole(hop.ConsignmentNo)) consoleList.Add(hop.ConsignmentNo);

            var hopEventAdapter = new Adapters.Oal.dbo_hop_event_newAdapter();
            var hopWwpEventAdapter = new Adapters.Oal.dbo_wwp_event_new_logAdapter();
            var hopEventMap = new Integrations.Transforms.RtsHopToOalHopEventNew();
            var parentRow = await hopEventMap.TransformAsync(hop);
            parentRow.id = GenerateId(34);
            m_hopEventRows.Add(parentRow);

            var hopWwpEventLogMap = new Integrations.Transforms.RtsHopToOalWwpEventNewLog();
            var parentWwpRow = await hopWwpEventLogMap.TransformAsync(hop);
            m_hopWwpEventLogRows.Add(parentWwpRow);

            var consoleItem = await GetItemConsigmentsFromConsoleDetailsAsync(hop.ConsignmentNo);
            if (null != consoleItem)
            {
                var children = consoleItem.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in children)
                {
                    if (consoleList.Contains(item)) continue;
                    ProcessChild(parentRow, item);
                    ProcessChildWwp(parentWwpRow, item);

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
                AddPendingItems(parentRow.id, parentWwpRow.id, hop.ConsignmentNo);
            }

            foreach (var item in m_hopEventRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => hopEventAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }

            foreach (var item in m_hopWwpEventLogRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => hopWwpEventAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }

            var pendingAdapter = new Adapters.Oal.dbo_event_pending_consoleAdapter();
            foreach (var item in m_hopEventPendingConsoleRows)
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


        private void ProcessChild(Adapters.Oal.dbo_hop_event_new parent, string consignmentNo)
        {
            var console = IsConsole(consignmentNo);
            var child = parent.Clone();
            child.id = GenerateId(34);
            child.consignment_no = consignmentNo;
            child.data_flag = "1";
            child.item_type_code = console ? "02" : "01";
            child.date_created_oal_date_field = DateTime.Now;
            m_hopEventRows.Add(child);
        }

        private void AddPendingItems(string parentId, string wwpParentId, string consignmentNo)
        {
            //insert into pending items
            var pendingConsole = new Adapters.Oal.dbo_event_pending_console
            {
                id = GenerateId(20),
                event_id = parentId,
                event_class = "pos.oal.HopEventNew",
                console_no = consignmentNo,
                version = 0,
                date_field = DateTime.Now
            };
            m_hopEventPendingConsoleRows.Add(pendingConsole);

            var pendingWwp = new Adapters.Oal.dbo_event_pending_console
            {
                id = GenerateId(20),
                event_id = wwpParentId,
                event_class = "pos.oal.WwpEventNewLog",
                console_no = consignmentNo,
                version = 0,
                date_field = DateTime.Now
            };
            m_hopEventPendingConsoleRows.Add(pendingWwp);
        }

        private void ProcessChildWwp(Adapters.Oal.dbo_wwp_event_new_log hopWwp, string childConnoteNo)
        {
            var wwp = hopWwp.Clone();
            wwp.id = GenerateId(34);
            wwp.dt_created_oal_date_field = DateTime.Now;
            wwp.consignment_note_number = childConnoteNo;
            m_hopWwpEventLogRows.Add(wwp);
        }


       

    }
}