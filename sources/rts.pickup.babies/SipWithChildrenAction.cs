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
    [DesignerMetadata(Name = "SipWithChildren", TypeName = "Bespoke.PosEntt.CustomActions.SipWithChildrenAction, rts.pickup.babies", Description = "RTS SIP with child items", FontAwesomeIcon = "calendar-check-o")]
    public class SipWithChildrenAction : EventWithChildrenAction
    {

        private List<Adapters.Oal.dbo_sip_event_new> m_sipEventRows;
        private List<Adapters.Oal.dbo_wwp_event_new_log> m_sipWwpEventLogRows;
        private List<Adapters.Oal.dbo_event_pending_console> m_sipEventPendingConsoleRows;

        public override async Task ExecuteAsync(RuleContext context)
        {
            var sip = context.Item as Sips.Domain.Sip;
            if (null == sip) return;
            var isConsole = IsConsole(sip.ConsignmentNo);
            if (!isConsole) return;

            var swca = new SipWithChildrenAction()
            {
                m_sipEventRows = new List<Adapters.Oal.dbo_sip_event_new>(),
                m_sipWwpEventLogRows = new List<Adapters.Oal.dbo_wwp_event_new_log>(),
                m_sipEventPendingConsoleRows = new List<Adapters.Oal.dbo_event_pending_console>()
            };
            await swca.RunAsync(sip);
        }

        public async Task RunAsync(Sips.Domain.Sip sip)
        {
            //console_details
            var consoleList = new List<string>();
            if (IsConsole(sip.ConsignmentNo)) consoleList.Add(sip.ConsignmentNo);

            var sipEventAdapter = new Adapters.Oal.dbo_sip_event_newAdapter();
            var sipWwpEventAdapter = new Adapters.Oal.dbo_wwp_event_new_logAdapter();
            var sipEventMap = new Integrations.Transforms.RtsSipToOalSipEventNew();
            var parentRow = await sipEventMap.TransformAsync(sip);
            parentRow.id = GenerateId(34);
            m_sipEventRows.Add(parentRow);

            var sipWwpEventLogMap = new Integrations.Transforms.RtsSipToOalSipWwpEventNewLog();
            var parentWwpRow = await sipWwpEventLogMap.TransformAsync(sip);
            m_sipWwpEventLogRows.Add(parentWwpRow);

            var consoleItem = await GetItemConsigmentsFromConsoleDetailsAsync(sip.ConsignmentNo);
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
                AddPendingItems(parentRow.id, parentWwpRow.id, sip.ConsignmentNo);
            }

            foreach (var item in m_sipEventRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => TrackEvents(sipEventAdapter.InsertAsync,item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }

            foreach (var item in m_sipWwpEventLogRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => TrackEvents(sipWwpEventAdapter.InsertAsync,item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }

            var pendingAdapter = new Adapters.Oal.dbo_event_pending_consoleAdapter();
            foreach (var item in m_sipEventPendingConsoleRows)
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

   
        private void ProcessChild(Adapters.Oal.dbo_sip_event_new parent, string consignmentNo)
        {
            var console = IsConsole(consignmentNo);
            var child = parent.Clone();
            child.id = GenerateId(34);
            child.date_created_o_a_l_date_field = DateTime.Now;
            child.consignment_no = consignmentNo;
            child.data_flag = "1";
            child.item_type_code = console ? "02" : "01";
            m_sipEventRows.Add(child);
        }

        private void AddPendingItems(string parentId, string wwpParentId, string consignmentNo)
        {
            //insert into pending items
            var pendingConsole = new Adapters.Oal.dbo_event_pending_console
            {
                id = GenerateId(20),
                event_id = parentId,
                event_class = "pos.oal.SipEventNew",
                console_no = consignmentNo,
                version = 0,
                date_field = DateTime.Now
            };
            m_sipEventPendingConsoleRows.Add(pendingConsole);

            var pendingWwp = new Adapters.Oal.dbo_event_pending_console
            {
                id = GenerateId(20),
                event_id = wwpParentId,
                event_class = "pos.oal.WwpEventNewLog",
                console_no = consignmentNo,
                version = 0,
                date_field = DateTime.Now
            };
            m_sipEventPendingConsoleRows.Add(pendingWwp);
        }

        private void ProcessChildWwp(Adapters.Oal.dbo_wwp_event_new_log sipWwp, string childConnoteNo)
        {
            var wwp = sipWwp.Clone();
            wwp.id = GenerateId(34);
            wwp.dt_created_oal_date_field = DateTime.Now;
            wwp.consignment_note_number = childConnoteNo;
            m_sipWwpEventLogRows.Add(wwp);
        }
        
        
        
    }
}