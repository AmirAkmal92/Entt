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
    [DesignerMetadata(Name = "DecoWithBabies", TypeName = "Bespoke.PosEntt.CustomActions.DecoWithBabiesAction, rts.pickup.babies", Description = "RTS Deco with child items", FontAwesomeIcon = "calendar-check-o")]
    public class DecoWithBabiesAction : EventWithChildrenAction
    {
        private List<Adapters.Oal.dbo_delivery_console_event_new> m_decoEventRows;
        private List<Adapters.Oal.dbo_event_pending_console> m_decoEventPendingConsoleRows;

        public override async Task ExecuteAsync(RuleContext context)
        {
            var deco = context.Item as Decos.Domain.Deco;
            if (null == deco) return;
            if (deco.TotalConsignment <= 0) return;

            m_decoEventRows = new List<Adapters.Oal.dbo_delivery_console_event_new>();
            m_decoEventPendingConsoleRows = new List<Adapters.Oal.dbo_event_pending_console>();

            await RunAsync(deco);
        }

        public async Task RunAsync(Decos.Domain.Deco deco)
        {
            var consoleList = new List<string>();
            if (IsConsole(deco.ConsoleTag)) consoleList.Add(deco.ConsoleTag);

            //deco console_details
            var decoEventMap = new Integrations.Transforms.RtsDecoOalDeliveryConsoleEventNew();
            var parentRow = await decoEventMap.TransformAsync(deco);
            var decoEventAdapter = new Adapters.Oal.dbo_delivery_console_event_newAdapter();
            m_decoEventRows.Add(parentRow);

            var itemList = deco.AllConsignmentnNotes.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in itemList)
            {
                ProcessChild(parentRow, item);

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
                            }
                        }
                        else
                        {
                            AddPendingItems(parentRow.id, cc);
                        }
                    }
                }
                else
                {
                    AddPendingItems(parentRow.id, item);
                }
            }

            foreach (var item in m_decoEventRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => decoEventAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }

            var pendingAdapter = new Adapters.Oal.dbo_event_pending_consoleAdapter();
            foreach (var item in m_decoEventPendingConsoleRows)
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


        private void ProcessChild(Adapters.Oal.dbo_delivery_console_event_new parent, string consignmentNo)
        {
            var console = IsConsole(consignmentNo);
            var child = parent.Clone();
            child.id = GenerateId(34);
            child.consignment_no = consignmentNo;
            child.data_flag = "1";
            child.item_type_code = console ? "02" : "01";
            m_decoEventRows.Add(child);
        }

        private void AddPendingItems(string parentId, string consignmentNo)
        {
            //insert into pending items
            var pendingConsole = new Adapters.Oal.dbo_event_pending_console
            {
                id = GenerateId(20),
                event_id = parentId,
                event_class = "pos.oal.DeliveryConsoleEventNew",
                console_no = consignmentNo,
                version = 0,
                date_field = DateTime.Now
            };
            m_decoEventPendingConsoleRows.Add(pendingConsole);
        }

    }
}