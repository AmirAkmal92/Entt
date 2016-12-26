using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Polly;

namespace Bespoke.PosEntt.CustomActions
{
    [Export(typeof(CustomAction))]
    [DesignerMetadata(Name = "NormWithBabies", TypeName = "Bespoke.PosEntt.CustomActions.NormWithBabiesAction, rts.pickup.babies", Description = "RTS Norm with babies", FontAwesomeIcon = "calendar-check-o")]
    public class NormWithBabiesAction : EventWithChildrenAction
    {

        public override async Task ExecuteAsync(RuleContext context)
        {
            var norm = context.Item as Norms.Domain.Norm;
            if (null == norm) return;
            if (norm.TotalConsignment <= 0) return;

            await RunAsync(norm);
        }

        public async Task RunAsync(Norms.Domain.Norm norm)
        {
            var pendingConsoles = new List<string>();
            //add to console_details first
            await AddToConsoleDetailsAsync(norm);

            var rows = new ConcurrentBag<Adapters.Oal.dbo_normal_console_event_new>();
            var map = new Integrations.Transforms.RtsNormToNormalConsoleEventNew();
            var row = await map.TransformAsync(norm);
            rows.Add(row);
            rows.AddRange(await GetEventRowsAsync(row, pendingConsoles, norm.AllConsignmentNotes));

            //
            await InsertDeliveryConsoleEventNewAsync(rows);

            //
            if (pendingConsoles.Any())
                await InsertEventPendingConsoleAsync(row, pendingConsoles.ToArray());

            var consoleDetailsAdapter = new Adapters.Oal.dbo_console_detailsAdapter();
            var tagExistInPendingConsole = (await consoleDetailsAdapter.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM dbo.event_pending_console WHERE [console_no] = '{norm.ConsoleTag}'")) > 0;
            if (tagExistInPendingConsole)
                await ProcessPendingConsoleAsync(norm, rows);

        }

        private async Task ProcessPendingConsoleAsync(Norms.Domain.Norm norm, IEnumerable<Adapters.Oal.dbo_normal_console_event_new> rows)
        {
            var pendingItems = await SearchEventPendingAsync(norm.ConsoleTag);
            var tasks = from p in pendingItems
                        select ProcessEventPendingItem(norm, p);
            await Task.WhenAll(tasks);

            var consolePendingList = new List<string> { norm.ConsoleTag };
            foreach (var item in rows)
            {
                var console = IsConsole(item.consignment_no);
                if (!console) continue;
                if (consolePendingList.Contains(item.consignment_no)) continue;

                consolePendingList.Add(item.consignment_no);
            }
        }

        private async Task AddToConsoleDetailsAsync(Norms.Domain.Norm norm)
        {
            var adapter = new Adapters.Oal.dbo_console_detailsAdapter();
            var id = await adapter.ExecuteScalarAsync<string>($"SELECT [id] FROM dbo.console_details WHERE [console_no] = '{norm.ConsoleTag}'");
            var consoleTagNotInConsoleDetails = string.IsNullOrWhiteSpace(id);
            if (consoleTagNotInConsoleDetails)
            {
                var map = new Integrations.Transforms.RtsNormToOalConsoleDetails();
                var row = await map.TransformAsync(norm);

                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => adapter.InsertAsync(row));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
                return;
            }

            var details = await adapter.LoadOneAsync(id);
            if (details.courier_id == norm.CourierId && (details.date_field ?? DateTime.MinValue).Date == norm.Date.Date)
            {
                var notes = details.item_consignments.Split(new[] { ',', '\t' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                notes.AddRange(norm.AllConsignmentNotes.Split(new[] { ',', '\t' }, StringSplitOptions.RemoveEmptyEntries));
                details.item_consignments = string.Join("\t", notes.OrderBy(x => x).Distinct());
                await adapter.UpdateAsync(details);
                return;
            }
            await InsertConsoleDuplicationErrorAndEventExceptionAsync(norm);

        }

        private async Task InsertConsoleDuplicationErrorAndEventExceptionAsync(Norms.Domain.Norm norm)
        {
            Console.WriteLine("insert into console_duplicate_error & event_exception");
            var error = new Adapters.Oal.dbo_console_duplicate_error
            {
                item_consignments = norm.AllConsignmentNotes,
                date_field = norm.Date,
                courier_id = norm.CourierId,
                batch_name = "", // TODO : where's the batch_name value
                beat_no = norm.BeatNo,
                console_no = norm.ConsoleTag,
                console_type = "norm", // TODO : is this correct ?
                console_type_desc = "",// TODO : again, nothing is said about
                courier_name = "", // TODO : lookup the courier name
                dt_created_oal_date_field = norm.CreatedDate,
                event_comment = norm.Comment,
                id = GenerateId(20),
                event_type = "deco",
                office_dest = "",
                version = 0,
                office_dest_name = null,
                office_name = null,
                office_next_code = null,
                office_no = null,
                other_console_type = null,
                routing_code = null

            };
            var errorAdapter = new Adapters.Oal.dbo_console_duplicate_errorAdapter();
            await errorAdapter.InsertAsync(error);


            var exc = new Adapters.Oal.dbo_event_exception
            {
                consignment_no = norm.ConsoleTag,
                date_field = norm.Date,
                batch_name = error.batch_name,
                office_no = error.office_no,
                version = 0,
                id = GenerateId(34),
                courier_id = norm.CourierId,
                event_class = "", // TODO : I don't know
                event_id = ""// TODO : I just don't know
            };
            var excAdapter = new Adapters.Oal.dbo_event_exceptionAdapter();
            await excAdapter.InsertAsync(exc);
        }

        private async Task InsertEventPendingConsoleAsync(Adapters.Oal.dbo_normal_console_event_new parent, params string[] connoteNotes)
        {
            var pendingAdapter = new Adapters.Oal.dbo_event_pending_consoleAdapter();
            var rows = new List<Adapters.Oal.dbo_event_pending_console>();

            foreach (var connoteNo in connoteNotes)
            {
                rows.Add(GetPendingConsoleRow(parent.id, connoteNo));
                foreach (var item in rows)
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
        }

        private async Task InsertDeliveryConsoleEventNewAsync(IEnumerable<Adapters.Oal.dbo_normal_console_event_new> rows)
        {

            var decoEventAdapter = new Adapters.Oal.dbo_normal_console_event_newAdapter();
            foreach (var item in rows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => decoEventAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }

        private async Task<IEnumerable<Adapters.Oal.dbo_normal_console_event_new>> GetEventRowsAsync(Adapters.Oal.dbo_normal_console_event_new eventRow, IList<string> pendingConsoles, string consignmentNotes)
        {

            var list = new ConcurrentBag<Adapters.Oal.dbo_normal_console_event_new>();
            var items = consignmentNotes.Split(new[] { ',', '\t' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
            foreach (var c in items)
            {
                var itemConNote = c;
                var row = await CloneChildAsync(eventRow, itemConNote);
                list.Add(row);
                if (!IsConsole(itemConNote)) continue;

                var childConsignments = await GetItemConsigmentsFromConsoleDetailsAsync(itemConNote);
                if (null == childConsignments)
                {
                    pendingConsoles.Add(itemConNote);
                    continue;
                }
                var children = await GetEventRowsAsync(eventRow, pendingConsoles, childConsignments);
                list.AddRange(children);

            }

            return list;
        }

        private async Task ProcessEventPendingItem(Norms.Domain.Norm norm, Adapters.Oal.dbo_event_pending_console pending)
        {
            var itemList = norm.AllConsignmentNotes.Split(new[] { ',', '\t' }, StringSplitOptions.RemoveEmptyEntries);
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

        private async Task<IEnumerable<Adapters.Oal.dbo_event_pending_console>> SearchEventPendingAsync(string consoleNo)
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

        private async Task<Adapters.Oal.dbo_normal_console_event_new> CloneChildAsync(Adapters.Oal.dbo_normal_console_event_new parent, string consignmentNo)
        {
            var console = IsConsole(consignmentNo);
            var child = parent.Clone();
            child.id = GenerateId(34);
            child.consignment_no = consignmentNo;
            child.data_flag = "1";
            child.item_type_code = console ? "02" : "01";
            if (console)
                child.item_consignments = await this.GetItemConsigmentsFromConsoleDetailsAsync(consignmentNo);


            return child;

        }

        private Adapters.Oal.dbo_event_pending_console GetPendingConsoleRow(string parentId, string consignmentNo)
        {

            var pendingConsole = new Adapters.Oal.dbo_event_pending_console
            {
                id = GenerateId(20),
                event_id = parentId,
                event_class = "pos.oal.DeliveryConsoleEventNew",
                console_no = consignmentNo,
                version = 0,
                date_field = DateTime.Now
            };
            return pendingConsole;
        }


    }
}