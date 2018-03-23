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
    [DesignerMetadata(Name = "MissWithChildren", TypeName = "Bespoke.PosEntt.CustomActions.MissWithChildrenAction, rts.pickup.babies", Description = "RTS MISSORT with child items", FontAwesomeIcon = "calendar-check-o")]
    public class MissWithChildrenAction : EventWithChildrenAction
    {
        private List<Adapters.Oal.dbo_missort_event_new> m_missEventRows;
        private List<Adapters.Oal.dbo_event_pending_console> m_missEventPendingConsoleRows;

        public override async Task ExecuteAsync(RuleContext context)
        {
            var miss = context.Item as Misses.Domain.Miss;
            if (null == miss) return;
            var isConsole = IsConsole(miss.ConsignmentNo);
            if (!isConsole) return;

            var mwca = new MissWithChildrenAction()
            {
                m_missEventRows = new List<Adapters.Oal.dbo_missort_event_new>(),
                m_missEventPendingConsoleRows = new List<Adapters.Oal.dbo_event_pending_console>()
            };
            await mwca.RunAsync(miss);
        }

        public async Task RunAsync(Misses.Domain.Miss miss)
        {
            //console_details
            var consoleList = new List<string>();
            if (IsConsole(miss.ConsignmentNo)) consoleList.Add(miss.ConsignmentNo);

            var missEventAdapter = new Adapters.Oal.dbo_missort_event_newAdapter();
            var missEventMap = new Integrations.Transforms.RtsMissToOalMissortEventNew();
            var parentRow = await missEventMap.TransformAsync(miss);
            parentRow.id = GenerateId(34);
            m_missEventRows.Add(parentRow);
            
            var consoleItem = await GetItemConsigmentsFromConsoleDetailsAsync(miss.ConsignmentNo);
            if (null != consoleItem)
            {
                var children = consoleItem.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in children)
                {
                    if (consoleList.Contains(item)) continue;
                    ProcessChild(parentRow, item);

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
                                        }
                                    }
                                    else
                                    {
                                        AddPendingItems(parentRow.id, cc);
                                    }
                                }
                            }
                        }
                        else
                        {
                            AddPendingItems(parentRow.id, item);
                        }
                    }
                }
            }
            else
            {
                AddPendingItems(parentRow.id, miss.ConsignmentNo);
            }

            foreach (var item in m_missEventRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => TrackEvents(missEventAdapter.InsertAsync,item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }

            var pendingAdapter = new Adapters.Oal.dbo_event_pending_consoleAdapter();
            foreach (var item in m_missEventPendingConsoleRows)
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

   

        private void ProcessChild(Adapters.Oal.dbo_missort_event_new parent, string consignmentNo)
        {
            var console = IsConsole(consignmentNo);
            var child = parent.Clone();
            child.id = GenerateId(34);
            child.consignment_no = consignmentNo;
            child.data_flag = "1";
            child.item_type_code = console ? "02" : "01";
            m_missEventRows.Add(child);
        }

        private void AddPendingItems(string parentId, string consignmentNo)
        {
            //insert into pending items
            var pendingConsole = new Adapters.Oal.dbo_event_pending_console
            {
                id = GenerateId(20),
                event_id = parentId,
                event_class = "pos.oal.MissortEventNew",
                console_no = consignmentNo,
                version = 0,
                date_field = DateTime.Now
            };
            m_missEventPendingConsoleRows.Add(pendingConsole);
        }

      
    }
}