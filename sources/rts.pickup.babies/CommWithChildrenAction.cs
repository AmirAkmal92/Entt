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
    [DesignerMetadata(Name = "CommWithChildren", TypeName = "Bespoke.PosEntt.CustomActions.CommWithChildrenAction, rts.pickup.babies", Description = "RTS Comm with child items", FontAwesomeIcon = "calendar-check-o")]
    public class CommWithChildrenAction : EventWithChildrenAction
    {
        public override bool UseAsync => true;
        private List<Adapters.Oal.dbo_comment_event_new> m_commEventRows;
        private List<Adapters.Oal.dbo_event_pending_console> m_commEventPendingConsoleRows;

        public override async Task ExecuteAsync(RuleContext context)
        {
            var comm = context.Item as Comms.Domain.Comm;
            if (null == comm) return;
            var isConsole = IsConsole(comm.ConsignmentNo);
            if (!isConsole) return;

            m_commEventRows = new List<Adapters.Oal.dbo_comment_event_new>();
            m_commEventPendingConsoleRows = new List<Adapters.Oal.dbo_event_pending_console>();

            await RunAsync(comm);
        }

        public async Task RunAsync(Comms.Domain.Comm comm)
        {
            //console_details
            var consoleList = new List<string>();
            if (IsConsole(comm.ConsignmentNo)) consoleList.Add(comm.ConsignmentNo);

            var commEventAdapter = new Adapters.Oal.dbo_comment_event_newAdapter();
            var commEventMap = new Integrations.Transforms.RtsCommToOalCommEventNew();
            var parentRow = await commEventMap.TransformAsync(comm);
            parentRow.id = GenerateId(34);
            m_commEventRows.Add(parentRow);
            
            var consoleItem = await GetItemConsigmentsFromConsoleDetailsAsync(comm.ConsignmentNo);
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
                AddPendingItems(parentRow.id, comm.ConsignmentNo);
            }

            foreach (var item in m_commEventRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => commEventAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }

            var pendingAdapter = new Adapters.Oal.dbo_event_pending_consoleAdapter();
            foreach (var item in m_commEventPendingConsoleRows)
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


        private void ProcessChild(Adapters.Oal.dbo_comment_event_new parent, string consignmentNo)
        {
            var console = IsConsole(consignmentNo);
            var child = parent.Clone();
            child.id = GenerateId(34);
            child.consignment_no = consignmentNo;
            child.data_flag = "1";
            child.item_type_code = console ? "02" : "01";
            m_commEventRows.Add(child);
        }

        private void AddPendingItems(string parentId, string consignmentNo)
        {
            //insert into pending items
            var pendingConsole = new Adapters.Oal.dbo_event_pending_console
            {
                id = GenerateId(20),
                event_id = parentId,
                event_class = "pos.oal.CommentEventNew",
                console_no = consignmentNo,
                version = 0,
                date_field = DateTime.Now
            };
            m_commEventPendingConsoleRows.Add(pendingConsole);
        }

      
    }
}