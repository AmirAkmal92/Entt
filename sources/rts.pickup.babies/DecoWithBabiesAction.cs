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

            //
            //execute pending items, if any
            var pendingItems = await SearchEventPending(deco.ConsoleTag);
            foreach (var pending in pendingItems)
            {
                await ProcessEventPendingItem(deco, pending);
            }


            var consolePendingList = new List<string> { deco.ConsoleTag };
            foreach (var item in itemList)
            {
                var console = IsConsole(item);
                if (!console) continue;
                if (consolePendingList.Contains(item)) continue;

                consolePendingList.Add(item);
            }
        }

        private async Task ProcessEventPendingItem(Decos.Domain.Deco deco, Adapters.Oal.dbo_event_pending_console pending)
        {
            var itemList = deco.AllConsignmentnNotes.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var ok = false;
            switch (pending.event_class)
            {
                case "pos.oal.DeliveryEventNew":
                    await ProcessDeliveryPendingItem(pending.event_id, itemList);
                    break;
                case "pos.oal.SopEventNew":
                    await ProcessSopPendingItem(pending.event_id, itemList);
                    ok = true;
                    break;
                case "pos.oal.SipEventNew":
                    await ProcessSipPendingItem(pending.event_id, itemList);
                    ok = true;
                    break;
                case "pos.oal.HipEventNew":
                    await ProcessHipPendingItem(pending.event_id, itemList);
                    ok = true;
                    break;
                case "pos.oal.HopEventNew":
                    await ProcessHopPendingItem(pending.event_id, itemList);
                    ok = true;
                    break;
                case "pos.oal.StatusEventNew":
                    await ProcessStatPendingItem(pending.event_id, itemList);
                    ok = true;
                    break;
                case "pos.oal.VasnEventNew":
                    await ProcessVasnPendingItem(pending.event_id, itemList);
                    ok = true;
                    break;
                case "pos.oal.NormalConsoleEventNew":
                    //ProcessNormPendingItem(item.console_no, item.event_id);
                    break;
                case "pos.oal.MissortEventNew":
                    await ProcessMissPendingItem(pending.event_id, itemList);
                    break;
                case "pos.oal.WwpEventNewLog":
                    await ProcessWwpPendingItem(pending.event_id, itemList);
                    ok = true;
                    break;
            }

            if (ok)
            {
                var pendingAdapter = new Adapters.Oal.dbo_event_pending_consoleAdapter();
                await pendingAdapter.DeleteAsync(pending.id);
            }
        }
        
        private async Task ProcessDeliveryPendingItem(string deliEventId, string[] itemList)
        {
            var deliAdapter = new Adapters.Oal.dbo_delivery_event_newAdapter();
            var deli = await deliAdapter.LoadOneAsync(deliEventId);
            var deliItems = new List<Adapters.Oal.dbo_delivery_event_new>();
            foreach (var item in itemList)
            {
                var console = IsConsole(item);
                var child = deli.Clone();
                child.id = GenerateId(34);
                child.consignment_no = item;
                child.data_flag = "1";
                child.item_type_code = console ? "02" : "01";
                deliItems.Add(child);
            }
            foreach (var item in deliItems)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => deliAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }

        private async Task ProcessMissPendingItem(string missEventId, string[] itemList)
        {
            var missAdapter = new Adapters.Oal.dbo_missort_event_newAdapter();
            var miss = await missAdapter.LoadOneAsync(missEventId);
            var missItems = new List<Adapters.Oal.dbo_missort_event_new>();
            foreach (var item in itemList)
            {
                var console = IsConsole(item);
                var child = miss.Clone();
                child.id = GenerateId(34);
                child.consignment_no = item;
                child.data_flag = "1";
                child.item_type_code = console ? "02" : "01";
                missItems.Add(child);
            }
            foreach (var item in missItems)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => missAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }

        private async Task ProcessStatPendingItem(string statEventId, string[] itemList)
        {
            var statAdapter = new Adapters.Oal.dbo_status_code_event_newAdapter();
            var stat = await statAdapter.LoadOneAsync(statEventId);
            var statItems = new List<Adapters.Oal.dbo_status_code_event_new>();
            foreach (var item in itemList)
            {
                var console = IsConsole(item);
                var child = stat.Clone();
                child.id = GenerateId(34);
                child.consignment_no = item;
                child.data_flag = "1";
                child.item_type_code = console ? "02" : "01";
                statItems.Add(child);
            }
            foreach (var item in statItems)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => statAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }

        private async Task ProcessVasnPendingItem(string vasnEventId, string[] itemList)
        {
            var vasnAdapter = new Adapters.Oal.dbo_vasn_event_newAdapter();
            var vasn = await vasnAdapter.LoadOneAsync(vasnEventId);
            var vasnItems = new List<Adapters.Oal.dbo_vasn_event_new>();
            foreach (var item in itemList)
            {
                var console = IsConsole(item);
                var child = vasn.Clone();
                child.id = GenerateId(34);
                child.consignment_no = item;
                child.data_flag = "1";
                child.item_type_code = console ? "02" : "01";
                vasnItems.Add(child);
            }
            foreach (var item in vasnItems)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => vasnAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }

        private async Task ProcessSopPendingItem(string sopEventId, string[] itemList)
        {
            var sopAdapter = new Adapters.Oal.dbo_sop_event_newAdapter();
            var sop = await sopAdapter.LoadOneAsync(sopEventId);
            var sops = new List<Adapters.Oal.dbo_sop_event_new>();
            foreach (var item in itemList)
            {
                var console = IsConsole(item);
                var child = sop.Clone();
                child.id = GenerateId(34);
                child.consignment_no = item;
                child.data_flag = "1";
                child.item_type_code = console ? "02" : "01";
                sops.Add(child);
            }
            foreach (var item in sops)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => sopAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }

        private async Task ProcessSipPendingItem(string sipEventId, string[] itemList)
        {
            var sipAdapter = new Adapters.Oal.dbo_sip_event_newAdapter();
            var sip = await sipAdapter.LoadOneAsync(sipEventId);
            var sips = new List<Adapters.Oal.dbo_sip_event_new>();
            foreach (var item in itemList)
            {
                var console = IsConsole(item);
                var child = sip.Clone();
                child.id = GenerateId(34);
                child.consignment_no = item;
                child.data_flag = "1";
                child.item_type_code = console ? "02" : "01";
                sips.Add(child);
            }
            foreach (var item in sips)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => sipAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }

        private async Task ProcessHopPendingItem(string hopEventId, string[] itemList)
        {
            var hopAdapter = new Adapters.Oal.dbo_hop_event_newAdapter();
            var hop = await hopAdapter.LoadOneAsync(hopEventId);
            var hops = new List<Adapters.Oal.dbo_hop_event_new>();
            foreach (var item in itemList)
            {
                var console = IsConsole(item);
                var child = hop.Clone();
                child.id = GenerateId(34);
                child.consignment_no = item;
                child.data_flag = "1";
                child.item_type_code = console ? "02" : "01";
                hops.Add(child);
            }
            foreach (var item in hops)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => hopAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }

        private async Task ProcessHipPendingItem(string hipEventId, string[] itemList)
        {
            var hipAdapter = new Adapters.Oal.dbo_hip_event_newAdapter();
            var hip = await hipAdapter.LoadOneAsync(hipEventId);
            var hips = new List<Adapters.Oal.dbo_hip_event_new>();
            foreach (var item in itemList)
            {
                var console = IsConsole(item);
                var child = hip.Clone();
                child.id = GenerateId(34);
                child.consignment_no = item;
                child.data_flag = "1";
                child.item_type_code = console ? "02" : "01";
                hips.Add(child);
            }
            foreach (var item in hips)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => hipAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }

        private async Task ProcessWwpPendingItem(string wwpEventId, string[] itemList)
        {
            var wwpAdapter = new Adapters.Oal.dbo_wwp_event_new_logAdapter();
            var wwp = await wwpAdapter.LoadOneAsync(wwpEventId);
            var wwpItems = new List<Adapters.Oal.dbo_wwp_event_new_log>();
            foreach (var item in itemList)
            {
                var child = wwp.Clone();
                child.id = GenerateId(34);
                child.consignment_note_number = item;
                child.date_sent_date_field = null;
                wwpItems.Add(child);
            }
            foreach (var item in wwpItems)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => wwpAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }

        private async Task<IEnumerable<Adapters.Oal.dbo_event_pending_console>> SearchEventPending(string consoleNo)
        {
            var pendingAdapter = new Adapters.Oal.dbo_event_pending_consoleAdapter();
            var query = $"SELECT [id], [event_class], [event_id] FROM [dbo].[event_pending_console] WHERE console_no = '{consoleNo}'";

            var list = new List<Adapters.Oal.dbo_event_pending_console>();
            using (var conn = new SqlConnection(pendingAdapter.ConnectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var item = new Adapters.Oal.dbo_event_pending_console
                        {
                            id = reader["id"].ReadNullableString(),
                            event_class = reader["event_class"].ReadNullableString(),
                            event_id = reader["event_id"].ReadNullableString()
                        };
                        list.Add(item);

                    }
                }

            }
            return list;

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