using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Polly;
using System.Linq;

namespace Bespoke.PosEntt.CustomActions
{
    [Export(typeof(CustomAction))]
    [DesignerMetadata(Name = "HipWithChildren", TypeName = "Bespoke.PosEntt.CustomActions.HipWithChildrenAction, rts.pickup.babies", Description = "RTS HIP with child items", FontAwesomeIcon = "calendar-check-o")]
    public class HipWithChildrenAction : EventWithChildrenAction
    {
        public override bool UseAsync => true;
        private List<Adapters.Oal.dbo_hip_event_new> m_hipEventRows;
        private List<Adapters.Oal.dbo_wwp_event_new_log> m_hipWwpEventLogRows;
        private List<Adapters.Oal.dbo_event_pending_console> m_hipEventPendingConsoleRows;

        public override async Task ExecuteAsync(RuleContext context)
        {
            var hip = context.Item as Hips.Domain.Hip;
            if (null == hip) return;
            var isConsole = IsConsole(hip.ConsignmentNo);
            if (!isConsole) return;

            m_hipEventRows = new List<Adapters.Oal.dbo_hip_event_new>();
            m_hipWwpEventLogRows = new List<Adapters.Oal.dbo_wwp_event_new_log>();
            m_hipEventPendingConsoleRows = new List<Adapters.Oal.dbo_event_pending_console>();

            await RunAsync(hip);
        }

        public async Task RunAsync(Hips.Domain.Hip hip)
        {
            //console_details
            var consoleList = new List<string>();
            if (IsConsole(hip.ConsignmentNo)) consoleList.Add(hip.ConsignmentNo);

            var hipEventAdapter = new Adapters.Oal.dbo_hip_event_newAdapter();
            var hipWwpEventAdapter = new Adapters.Oal.dbo_wwp_event_new_logAdapter();
            var hipEventMap = new Integrations.Transforms.RtsHipToOalHipEventNew();
            var parentRow = await hipEventMap.TransformAsync(hip);
            parentRow.id = GenerateId(34);
            m_hipEventRows.Add(parentRow);

            var hipWwpEventLogMap = new Integrations.Transforms.RtsHipToOalWwpEventNewLog();
            var parentWwpRow = await hipWwpEventLogMap.TransformAsync(hip);
            m_hipWwpEventLogRows.Add(parentWwpRow);

            var consoleItem = await GetItemConsigmentsFromConsoleDetailsAsync(hip.ConsignmentNo);
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
                AddPendingItems(parentRow.id, parentWwpRow.id, hip.ConsignmentNo);
            }

            foreach (var item in m_hipEventRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => hipEventAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }

            foreach (var item in m_hipWwpEventLogRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => hipWwpEventAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }

            var pendingAdapter = new Adapters.Oal.dbo_event_pending_consoleAdapter();
            foreach (var item in m_hipEventPendingConsoleRows)
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

        private void ProcessChild(Adapters.Oal.dbo_hip_event_new parent, string consignmentNo)
        {
            var console = IsConsole(consignmentNo);
            var child = parent.Clone();
            child.id = GenerateId(34);
            child.consignment_no = consignmentNo;
            child.data_flag = "1";
            child.item_type_code = console ? "02" : "01";
            m_hipEventRows.Add(child);
        }

        private void AddPendingItems(string parentId, string wwpParentId, string consignmentNo)
        {
            //insert into pending items
            var pendingConsole = new Adapters.Oal.dbo_event_pending_console();
            pendingConsole.id = GenerateId(20);
            pendingConsole.event_id = parentId;
            pendingConsole.event_class = "pos.oal.HipEventNew";
            pendingConsole.console_no = consignmentNo;
            pendingConsole.version = 0;
            pendingConsole.date_field = DateTime.Now;
            m_hipEventPendingConsoleRows.Add(pendingConsole);

            var pendingWwp = new Adapters.Oal.dbo_event_pending_console();
            pendingWwp.id = GenerateId(20);
            pendingWwp.event_id = wwpParentId;
            pendingWwp.event_class = "pos.oal.WwpEventNewLog";
            pendingWwp.console_no = consignmentNo;
            pendingWwp.version = 0;
            pendingWwp.date_field = DateTime.Now;
            m_hipEventPendingConsoleRows.Add(pendingWwp);
        }

        private void ProcessChildWwp(Adapters.Oal.dbo_wwp_event_new_log hipWwp, string childConnoteNo)
        {
            var wwp = hipWwp.Clone();
            wwp.id = GenerateId(34);
            wwp.consignment_note_number = childConnoteNo;
            m_hipWwpEventLogRows.Add(wwp);
        }
        
    }
}