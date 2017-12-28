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
    [DesignerMetadata(Name = "SopWithChildren", TypeName = "Bespoke.PosEntt.CustomActions.SopWithChildrenAction, rts.pickup.babies", Description = "RTS SOP with child items", FontAwesomeIcon = "calendar-check-o")]
    public class SopWithChildrenAction : EventWithChildrenAction
    {
        private List<Adapters.Oal.dbo_sop_event_new> m_sopEventRows;
        private List<Adapters.Oal.dbo_wwp_event_new_log> m_sopWwpEventLogRows;
        private List<Adapters.Oal.dbo_event_pending_console> m_sopEventPendingConsoleRows;

        public override async Task ExecuteAsync(RuleContext context)
        {
            var sop = context.Item as Sops.Domain.Sop;
            if (null == sop) return;
            var isConsole = IsConsole(sop.ConsignmentNo);
            if (!isConsole) return;

            var swca = new SopWithChildrenAction()
            {
                m_sopEventRows = new List<Adapters.Oal.dbo_sop_event_new>(),
                m_sopWwpEventLogRows = new List<Adapters.Oal.dbo_wwp_event_new_log>(),
                m_sopEventPendingConsoleRows = new List<Adapters.Oal.dbo_event_pending_console>()
            };
            await swca.RunAsync(sop);
        }

        public async Task RunAsync(Sops.Domain.Sop sop)
        {

            //console_details
            var consoleList = new List<string> { sop.ConsignmentNo };

            var sopWwpEventAdapter = new Adapters.Oal.dbo_wwp_event_new_logAdapter();
            var sopEventAdapter = new Adapters.Oal.dbo_sop_event_newAdapter();
            var sopEventMap = new Integrations.Transforms.RtsSopToOalSopEventNew();
            var parentRow = await sopEventMap.TransformAsync(sop);
            parentRow.id = GenerateId(34);
            m_sopEventRows.Add(parentRow);

            var sopWwpEventLogMap = new Integrations.Transforms.RtsSopToOalSopWwpEventNewLog();
            var parentWwpRow = await sopWwpEventLogMap.TransformAsync(sop);
            m_sopWwpEventLogRows.Add(parentWwpRow);

            var consoleItem = await GetItemConsigmentsFromConsoleDetailsAsync(sop.ConsignmentNo);
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
                    if (!console) continue;
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
                            if (!anotherConsole) continue;
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
                    else
                    {
                        AddPendingItems(parentRow.id, parentWwpRow.id, item);
                    }
                }
            }
            else
            {
                AddPendingItems(parentRow.id, parentWwpRow.id, sop.ConsignmentNo);
            }

            //persist any rows
            foreach (var item in m_sopEventRows)
            {
                System.Diagnostics.Debug.WriteLine("sop_event_new: {0}|{1}", item.consignment_no, item.id);
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => TrackEvents(sopEventAdapter.InsertAsync, item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }

            foreach (var item in m_sopWwpEventLogRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => TrackEvents(sopWwpEventAdapter.InsertAsync,item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }

            var pendingAdapter = new Adapters.Oal.dbo_event_pending_consoleAdapter();
            foreach (var item in m_sopEventPendingConsoleRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => TrackEvents(pendingAdapter.InsertAsync,item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }

        private void ProcessChild(Adapters.Oal.dbo_sop_event_new parent, string consignmentNo)
        {
            var console = IsConsole(consignmentNo);
            var child = parent.Clone();
            child.date_created_oal_date_field = DateTime.Now;
            child.id = GenerateId(34);
            child.consignment_no = consignmentNo;
            child.data_flag = "1";
            child.item_type_code = console ? "02" : "01";
            m_sopEventRows.Add(child);
        }

        private void AddPendingItems(string parentId, string wwpParentId, string consignmentNo)
        {
            //insert into pending items
            var pendingConsole = new Adapters.Oal.dbo_event_pending_console
            {
                id = GenerateId(20),
                event_id = parentId,
                event_class = "pos.oal.SopEventNew",
                console_no = consignmentNo,
                version = 0,
                date_field = DateTime.Now
            };
            m_sopEventPendingConsoleRows.Add(pendingConsole);

            var pendingWwp = new Adapters.Oal.dbo_event_pending_console
            {
                id = GenerateId(20),
                event_id = wwpParentId,
                event_class = "pos.oal.WwpEventNewLog",
                console_no = consignmentNo,
                version = 0,
                date_field = DateTime.Now
            };
            m_sopEventPendingConsoleRows.Add(pendingWwp);
        }

        private void ProcessChildWwp(Adapters.Oal.dbo_wwp_event_new_log sopWwp, string childConnoteNo)
        {
            var wwp = sopWwp.Clone();
            wwp.id = GenerateId(34);
            wwp.dt_created_oal_date_field = DateTime.Now;
            wwp.consignment_note_number = childConnoteNo;
            m_sopWwpEventLogRows.Add(wwp);
        }
    }
}