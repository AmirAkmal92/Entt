﻿using System;
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
    [DesignerMetadata(Name = "DecoWithBabies", TypeName = "Bespoke.PosEntt.CustomActions.DecoWithBabiesAction, rts.pickup.babies", Description = "RTS Deco with child items", FontAwesomeIcon = "calendar-check-o")]
    public class DecoWithBabiesAction : EventWithChildrenAction
    {
        public static int RetryCount = ConfigurationManager.GetEnvironmentVariableInt32("DecoRetryCount", 3);
        public Func<int, TimeSpan> WaitInterval = x => TimeSpan.FromMilliseconds(ConfigurationManager.GetEnvironmentVariableInt32("DecoWaitInterval", 500) * Math.Pow(2, x));

        public override async Task ExecuteAsync(RuleContext context)
        {
            var deco = context.Item as Decos.Domain.Deco;
            if (null == deco) return;
            if (deco.TotalConsignment <= 0) return;

            await RunAsync(deco);
        }

        public async Task RunAsync(Decos.Domain.Deco deco)
        {
            var pendingConsoles = new List<string>();
            var consoleList = new List<string> { deco.ConsoleTag };

            //add to console_details first
            var success = await AddToConsoleDetailsAsync(deco);

            if (!success) return;

            var rows = new ConcurrentBag<Adapters.Oal.dbo_delivery_console_event_new>();
            var map = new Integrations.Transforms.RtsDecoOalDeliveryConsoleEventNew();
            var row = await map.TransformAsync(deco);
            rows.Add(row);
            rows.AddRange(await GetEventRowsAsync(row, pendingConsoles, consoleList, deco.AllConsignmentnNotes));

            //
            await InsertDeliveryConsoleEventNewAsync(rows);

            //
            if (pendingConsoles.Any())
                await InsertEventPendingConsoleAsync(row, pendingConsoles.ToArray());

            var consoleDetailsAdapter = new Adapters.Oal.dbo_console_detailsAdapter();
            var tagExistInPendingConsole = (await consoleDetailsAdapter.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM dbo.event_pending_console WHERE [console_no] = '{deco.ConsoleTag}'")) > 0;
            if (tagExistInPendingConsole)
                await ProcessPendingConsoleAsync(deco, rows);

        }

        private async Task ProcessPendingConsoleAsync(Decos.Domain.Deco deco, IEnumerable<Adapters.Oal.dbo_delivery_console_event_new> rows)
        {
            var pendingItems = await SearchEventPendingAsync(deco.ConsoleTag);
            var tasks = from p in pendingItems
                        select ProcessEventPendingItem(deco, p);
            await Task.WhenAll(tasks);

            var consolePendingList = new List<string> { deco.ConsoleTag };
            foreach (var item in rows)
            {
                var console = IsConsole(item.consignment_no);
                if (!console) continue;
                if (consolePendingList.Contains(item.consignment_no)) continue;

                consolePendingList.Add(item.consignment_no);
            }
        }

        private async Task<bool> AddToConsoleDetailsAsync(Decos.Domain.Deco deco)
        {
            var adapter = new Adapters.Oal.dbo_console_detailsAdapter();
            var id = await adapter.ExecuteScalarAsync<string>($"SELECT [id] FROM dbo.console_details WHERE [console_no] = '{deco.ConsoleTag}'");
            var consoleTagNotInConsoleDetails = string.IsNullOrWhiteSpace(id);
            if (consoleTagNotInConsoleDetails)
            {
                var map = new Integrations.Transforms.RtsDecoOalConsoleDetails();
                var row = await map.TransformAsync(deco);

                // check for duplicate
                var dpr = await Policy.Handle<SqlException>(e => e.IsTimeout())
                    .WaitAndRetryAsync(RetryCount, WaitInterval)
                    .ExecuteAndCaptureAsync(() => adapter.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM dbo.console_details WHERE console_no ='{row.console_no}'"));
                if (dpr.Result == 0)
                {
                    var pr = Policy.Handle<SqlException>(e => e.IsDeadlockOrTimeout())
                        .WaitAndRetryAsync(RetryCount, WaitInterval)
                        .ExecuteAndCaptureAsync(() => adapter.InsertAsync(row));
                    var result = await pr;
                    if (result.FinalException != null)
                        throw result.FinalException; // send to dead letter queue
                    System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");

                }
                return true;

            }

            var detailsPolly = await Policy.Handle<SqlException>(e => e.IsTimeout())
                .WaitAndRetryAsync(RetryCount, WaitInterval)
                .ExecuteAndCaptureAsync(async () => await adapter.LoadOneAsync(id));
            if (null != detailsPolly.FinalException)
                throw new Exception("Console Details Polly Error", detailsPolly.FinalException);

            var details = detailsPolly.Result;

            var decoDateTime = deco.Date.AddHours(deco.Time.Hour).AddMinutes(deco.Time.Minute).AddSeconds(deco.Time.Second);

            if (details.courier_id == deco.CourierId && details.office_no == deco.LocationId && (details.date_field ?? DateTime.MinValue).AddHours(28) >= decoDateTime)
            {
                var notes = details.item_consignments.Split(new[] { ',', '\t' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                notes.AddRange(deco.AllConsignmentnNotes.Split(new[] { ',', '\t' }, StringSplitOptions.RemoveEmptyEntries));
                details.item_consignments = string.Join("\t", notes.OrderBy(x => x).Where(x => x.Length > 3 /* remove anything less*/).Distinct());
                await adapter.UpdateAsync(details);
                return true;
            }

            await InsertConsoleDuplicationErrorAndEventExceptionAsync(deco);
            return false;
        }

        private async Task InsertConsoleDuplicationErrorAndEventExceptionAsync(Decos.Domain.Deco deco)
        {
            Console.WriteLine("insert into console_duplicate_error & event_exception");

            //
            var map = new Integrations.Transforms.RtsDecoOalConsoleDetails();
            var decoDetails = await map.TransformAsync(deco);

            var error = new Adapters.Oal.dbo_console_duplicate_error
            {
                item_consignments = decoDetails.item_consignments,
                date_field = decoDetails.date_field,
                courier_id = decoDetails.courier_id,
                batch_name = decoDetails.batch_name,
                beat_no = decoDetails.beat_no,
                console_no = decoDetails.console_no,
                console_type = null,
                console_type_desc = null,
                courier_name = decoDetails.courier_name,
                dt_created_oal_date_field = decoDetails.dt_created_oal_date_field,
                event_comment = decoDetails.event_comment,
                id = GenerateId(20),
                event_type = decoDetails.event_type,
                office_dest = null,
                version = 0,
                office_dest_name = null,
                office_name = decoDetails.office_name,
                office_next_code = decoDetails.office_next_code,
                office_no = decoDetails.office_no,
                other_console_type = null,
                routing_code = null

            };
            var errorAdapter = new Adapters.Oal.dbo_console_duplicate_errorAdapter();
            await errorAdapter.InsertAsync(error);

            var exc = new Adapters.Oal.dbo_event_exception
            {
                consignment_no = error.console_no,
                date_field = error.date_field,
                batch_name = error.batch_name,
                office_no = error.office_no,
                version = error.version,
                id = GenerateId(34),
                courier_id = error.courier_id,
                event_class = "pos.oal.DeliveryConsoleEventNew",
                event_id = error.id
            };
            var excAdapter = new Adapters.Oal.dbo_event_exceptionAdapter();
            await excAdapter.InsertAsync(exc);
        }

        private async Task InsertEventPendingConsoleAsync(Adapters.Oal.dbo_delivery_console_event_new parent, params string[] connoteNotes)
        {
            var pendingAdapter = new Adapters.Oal.dbo_event_pending_consoleAdapter();
            var rows = new List<Adapters.Oal.dbo_event_pending_console>();

            foreach (var connoteNo in connoteNotes)
            {
                rows.Add(GetPendingConsoleRow(parent.id, connoteNo));
                foreach (var item in rows)
                {
                    // verify the id, is not duplicate
                    var pr = await Policy.Handle<SqlException>(e => e.IsTimeout())
                        .WaitAndRetryAsync(RetryCount, WaitInterval)
                        .ExecuteAndCaptureAsync(() => pendingAdapter.LoadOneAsync(item.id));
                    var one = pr.Result;
                    if (null != one)
                        continue;

                    // ignore insert exception
                    await Policy.Handle<SqlException>(e => e.IsDeadlockOrTimeout())
                        .WaitAndRetryAsync(RetryCount, WaitInterval)
                        .ExecuteAndCaptureAsync(() => pendingAdapter.InsertAsync(item));
                }

            }
        }

        private async Task InsertDeliveryConsoleEventNewAsync(IEnumerable<Adapters.Oal.dbo_delivery_console_event_new> rows)
        {
            var decoEventAdapter = new Adapters.Oal.dbo_delivery_console_event_newAdapter();
            foreach (var item in rows)
            {
                Console.Write(".");
                if (string.IsNullOrWhiteSpace(item.item_consignments))
                    continue;
                var pr = Policy.Handle<SqlException>(e => e.IsDeadlockOrTimeout())
                    .WaitAndRetryAsync(RetryCount, WaitInterval)
                    .ExecuteAndCaptureAsync(() => decoEventAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }

        private async Task<IEnumerable<Adapters.Oal.dbo_delivery_console_event_new>> GetEventRowsAsync(
            Adapters.Oal.dbo_delivery_console_event_new eventRow,
            IList<string> pendingConsoles,
            IList<string> consoleList,
            string consignmentNotes,
            int level = 0)
        {
            Console.Write("." + level);

            var list = new ConcurrentBag<Adapters.Oal.dbo_delivery_console_event_new>();
            if (level > 4)
                return list;
            var items = consignmentNotes.Split(new[] { ',', '\t' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
            foreach (var c in items)
            {
                var itemConNote = c;
                if (consoleList.Contains(itemConNote)) continue;
                var row = await CloneChildAsync(eventRow, itemConNote);
                list.Add(row);

                if (!IsConsole(itemConNote)) continue;
                consoleList.Add(itemConNote);

                var childConsignments = await GetItemConsigmentsFromConsoleDetailsAsync(itemConNote);
                if (null == childConsignments)
                {
                    pendingConsoles.Add(itemConNote);
                    continue;
                }
                var children = await GetEventRowsAsync(eventRow,
                    pendingConsoles,
                    consoleList,
                    childConsignments.ToEmptyString().Trim(),
                    level + 1);
                list.AddRange(children);

            }

            return list;
        }

        private async Task ProcessEventPendingItem(Decos.Domain.Deco deco, Adapters.Oal.dbo_event_pending_console pending)
        {
            var itemList = deco.AllConsignmentnNotes.Split(new[] { ',', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            var ok = false;
            switch (pending.event_class)
            {
                case "pos.oal.DeliveryEventNew":
                    await ProcessDeliveryPendingItem(pending.event_id, itemList);
                    ok = true;
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
                    ok = true;
                    break;
                case "pos.oal.WwpEventNewLog":
                    await ProcessWwpPendingItem(pending.event_id, itemList);
                    ok = true;
                    break;
                case "pos.oal.IpsImport":
                    await ProcessIpsPendingItem(pending.event_id, itemList);
                    ok = true;
                    break;
            }

            //if (ok)
            //{
            var pendingAdapter = new Adapters.Oal.dbo_event_pending_consoleAdapter();
            await pendingAdapter.DeleteAsync(pending.id);
            //}
        }

        private async Task ProcessDeliveryPendingItem(string deliEventId, string[] itemList)
        {
            var deliAdapter = new Adapters.Oal.dbo_delivery_event_newAdapter();
            var ipsAdapter = new Adapters.Oal.dbo_ips_importAdapter();

            var deliPendingPolly = await Policy.Handle<SqlException>(e => e.IsTimeout())
                .WaitAndRetryAsync(RetryCount, WaitInterval)
                .ExecuteAndCaptureAsync(async () => await deliAdapter.LoadOneAsync(deliEventId));
            if (null != deliPendingPolly.FinalException)
                throw new Exception("Process Deli Pending Polly Error", deliPendingPolly.FinalException);

            var deli = deliPendingPolly.Result;
            if (null == deli) return;

            var deliItems = new List<Adapters.Oal.dbo_delivery_event_new>();
            var ipsItems = new List<Adapters.Oal.dbo_ips_import>();

            foreach (var item in itemList)
            {
                var console = IsConsole(item);
                var child = deli.Clone();
                child.id = GenerateId(34);
                child.consignment_no = item;
                child.data_flag = "1";
                child.item_type_code = console ? "02" : "01";
                deliItems.Add(child);

                if (IsIpsImportItem(item))
                {
                    var ips = await CreateDeliIpsImport(deli, item);
                    ipsItems.Add(ips);
                }
            }

            foreach (var item in deliItems)
            {
                var pr = Policy.Handle<SqlException>(e => e.IsDeadlockOrTimeout())
                    .WaitAndRetryAsync(RetryCount, WaitInterval)
                    .ExecuteAndCaptureAsync(() => deliAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }

            foreach (var item in ipsItems)
            {
                var pr = Policy.Handle<SqlException>(e => e.IsDeadlockOrTimeout())
                    .WaitAndRetryAsync(RetryCount, WaitInterval)
                    .ExecuteAndCaptureAsync(() => ipsAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }

        private async Task<Adapters.Oal.dbo_ips_import> CreateDeliIpsImport(Adapters.Oal.dbo_delivery_event_new deli, string consignmentNo)
        {
            var ips = new Adapters.Oal.dbo_ips_import
            {
                id = GenerateId(13),
                version = 0,
                data_code_name = "deli",
                item_id = consignmentNo,
                class_cd = GetClassCode(consignmentNo),
                status = "1",
                user_fid = deli.courier_id.Length >= 5 ? deli.courier_id.Substring(0, 5) : deli.courier_id,
                event_date_local_date_field = deli.date_field,
                event_date_g_m_t_date_field = (deli.date_field ?? DateTime.Now).AddHours(-8),
                postal_status_fcd = "MINL",
                dest_country_cd = "MY",
                condition_cd = "30",
                dt_created_oal_date_field = DateTime.Now,
                office_cd = "MY" + deli.office_no,
                non_delivery_reason = GetNonDeliveryReason(deli.delivery_code, ""),
                non_delivery_measure = GetNonDeliveryMeasure(deli.delivery_code, ""),
                tn_cd = GetDeliveryTransactionCode(deli.delivery_code, "")
            };
            var signatories = new[] { "01", "10", "11" };
            if (signatories.Contains(deli.delivery_code)) ips.signatory_nm = deli.receipent_name;
            if (null != ips.signatory_nm && ips.signatory_nm.Length > 30)
            {
                ips.signatory_nm = ips.signatory_nm.Substring(0, 35);
            }

            var consignmentInitialPolly = await Policy.Handle<SqlException>()
                .WaitAndRetryAsync(RetryCount, WaitInterval)
                .ExecuteAndCaptureAsync(async () => await SearchConsignmentInitialAsync(consignmentNo));
            if (null != consignmentInitialPolly.FinalException)
                throw new Exception("Process pending deli import Polly failed", consignmentInitialPolly.FinalException);

            var consignment = consignmentInitialPolly.Result;
            if (null != consignment)
            {
                ips.item_weight_double = consignment.weight_double;
                if (!string.IsNullOrEmpty(consignment.shipper_address_country))
                {
                    ips.orig_country_cd = consignment.shipper_address_country;
                }
                if (!string.IsNullOrEmpty(consignment.item_category))
                    ips.content = consignment.item_category.Equals("01") ? "M" : "D";
                else
                    ips.content = "D";
            }

            return ips;
        }

        private async Task ProcessMissPendingItem(string missEventId, string[] itemList)
        {
            var missAdapter = new Adapters.Oal.dbo_missort_event_newAdapter();
            var missPendingPolly = await Policy.Handle<SqlException>(e => e.IsTimeout())
                .WaitAndRetryAsync(RetryCount, WaitInterval)
                .ExecuteAndCaptureAsync(async () => await missAdapter.LoadOneAsync(missEventId));
            if (null != missPendingPolly.FinalException)
                throw new Exception("Process Miss Pending Polly Error", missPendingPolly.FinalException);

            var miss = missPendingPolly.Result;
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
                item.date_created_oal_date_field = item.date_created_oal_date_field.ToValidSqlDateTime();
                item.date_field = item.date_field.ToValidSqlDateTime();

                var pr = Policy.Handle<SqlException>(e => e.IsDeadlockOrTimeout())
                    .WaitAndRetryAsync(RetryCount, WaitInterval)
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
            var statPendingPolly = await Policy.Handle<SqlException>(e => e.IsTimeout())
                .WaitAndRetryAsync(RetryCount, WaitInterval)
                .ExecuteAndCaptureAsync(async () => await statAdapter.LoadOneAsync(statEventId));
            if (null != statPendingPolly.FinalException)
                throw new Exception("Process Stat Pending Polly Error", statPendingPolly.FinalException);

            var stat = statPendingPolly.Result;
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
                item.date_created_oal_date_field = item.date_created_oal_date_field.ToValidSqlDateTime();
                item.date_field = item.date_field.ToValidSqlDateTime();

                var pr = Policy.Handle<SqlException>(e => e.IsDeadlockOrTimeout())
                    .WaitAndRetryAsync(RetryCount, WaitInterval)
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
            var vasnPendingPolly = await Policy.Handle<SqlException>(e => e.IsTimeout())
                .WaitAndRetryAsync(RetryCount, WaitInterval)
                .ExecuteAndCaptureAsync(async () => await vasnAdapter.LoadOneAsync(vasnEventId));
            if (null != vasnPendingPolly.FinalException)
                throw new Exception("Process Vasn Pending Polly Error", vasnPendingPolly.FinalException);

            var vasn = vasnPendingPolly.Result;
            var vasnItems = new List<Adapters.Oal.dbo_vasn_event_new>();
            foreach (var item in itemList)
            {
                var console = IsConsole(item);
                var child = vasn.Clone();
                child.id = GenerateId(34);
                child.consignment_no = item;
                child.data_flag = "1";
                child.item_type_code = console ? "02" : "01";
                if (child.dateCreatedOALDateField.HasValue && child.dateCreatedOALDateField.Value < new DateTime(1753, 1, 1))
                {
                    child.dateCreatedOALDateField = null;
                }
                vasnItems.Add(child);
            }
            foreach (var item in vasnItems)
            {
                var pr = Policy.Handle<SqlException>(e => e.IsDeadlockOrTimeout())
                    .WaitAndRetryAsync(RetryCount, WaitInterval)
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
            var sopPendingPolly = await Policy.Handle<SqlException>(e => e.IsTimeout())
                .WaitAndRetryAsync(RetryCount, WaitInterval)
                .ExecuteAndCaptureAsync(async () => await sopAdapter.LoadOneAsync(sopEventId));
            if (null != sopPendingPolly.FinalException)
                throw new Exception("Process Sop Pending Polly Error", sopPendingPolly.FinalException);

            var sop = sopPendingPolly.Result;
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
                var pr = Policy.Handle<SqlException>(e => e.IsDeadlockOrTimeout())
                    .WaitAndRetryAsync(RetryCount, WaitInterval)
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
            var sipPendingPolly = await Policy.Handle<SqlException>(e => e.IsTimeout())
                .WaitAndRetryAsync(RetryCount, WaitInterval)
                .ExecuteAndCaptureAsync(async () => await sipAdapter.LoadOneAsync(sipEventId));
            if (null != sipPendingPolly.FinalException)
                throw new Exception("Process Sip Pending Polly Error", sipPendingPolly.FinalException);

            var sip = sipPendingPolly.Result;
            var sips = new List<Adapters.Oal.dbo_sip_event_new>();
            foreach (var item in itemList)
            {
                var console = IsConsole(item);
                var child = sip.Clone();
                child.id = GenerateId(34);
                child.consignment_no = item;
                child.data_flag = "1";
                child.item_type_code = console ? "02" : "01";
                if (child.dateCreatedOALDateField.HasValue && child.dateCreatedOALDateField.Value < new DateTime(1753, 1, 1))
                {
                    child.dateCreatedOALDateField = null;
                }
                sips.Add(child);
            }
            foreach (var item in sips)
            {
                var pr = Policy.Handle<SqlException>(e => e.IsDeadlockOrTimeout())
                    .WaitAndRetryAsync(RetryCount, WaitInterval)
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
            var hopPendingPolly = await Policy.Handle<SqlException>(e => e.IsTimeout())
                .WaitAndRetryAsync(RetryCount, WaitInterval)
                .ExecuteAndCaptureAsync(async () => await hopAdapter.LoadOneAsync(hopEventId));
            if (null != hopPendingPolly.FinalException)
                throw new Exception("Process Hop Pending Polly Error", hopPendingPolly.FinalException);

            var hop = hopPendingPolly.Result;
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
                var pr = Policy.Handle<SqlException>(e => e.IsDeadlockOrTimeout())
                    .WaitAndRetryAsync(RetryCount, WaitInterval)
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
            var hipPendingPolly = await Policy.Handle<SqlException>(e => e.IsTimeout())
                .WaitAndRetryAsync(RetryCount, WaitInterval)
                .ExecuteAndCaptureAsync(async () => await hipAdapter.LoadOneAsync(hipEventId));
            if (null != hipPendingPolly.FinalException)
                throw new Exception("Process Hip Pending Polly Error", hipPendingPolly.FinalException);

            var hip = hipPendingPolly.Result;
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
                var pr = Policy.Handle<SqlException>(e => e.IsDeadlockOrTimeout())
                    .WaitAndRetryAsync(RetryCount, WaitInterval)
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
            var wwpPolly = await Policy.Handle<SqlException>(e => e.IsTimeout())
                .WaitAndRetryAsync(RetryCount, WaitInterval)
                .ExecuteAndCaptureAsync(async () => await wwpAdapter.LoadOneAsync(wwpEventId));
            if (null != wwpPolly.FinalException)
                throw new Exception("Wwp Pending Polly Error", wwpPolly.FinalException);

            var wwp = wwpPolly.Result;
            if (null == wwp) return;

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
                var pr = Policy.Handle<SqlException>(e => e.IsDeadlockOrTimeout())
                    .WaitAndRetryAsync(RetryCount, WaitInterval)
                    .ExecuteAndCaptureAsync(() => wwpAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }

        private async Task ProcessIpsPendingItem(string ipsEventId, string[] itemList)
        {
            var statAdapter = new Adapters.Oal.dbo_status_code_event_newAdapter();
            var statPendingPolly = await Policy.Handle<SqlException>(e => e.IsTimeout())
                .WaitAndRetryAsync(RetryCount, WaitInterval)
                .ExecuteAndCaptureAsync(async () => await statAdapter.LoadOneAsync(ipsEventId));
            if (null != statPendingPolly.FinalException)
                throw new Exception("Process Stat Pending Polly Error", statPendingPolly.FinalException);

            var stat = statPendingPolly.Result;
            var statItems = new List<Adapters.Oal.dbo_status_code_event_new>();

            var ipsAdapter = new Adapters.Oal.dbo_ips_importAdapter();
            var ipsItems = new List<Adapters.Oal.dbo_ips_import>();

            foreach (var item in itemList)
            {
                if (IsIpsImportItem(item))
                {
                    var ips = await CreateStatIpsImport(stat, item);
                    ipsItems.Add(ips);
                }
            }

            foreach (var item in ipsItems)
            {
                var pr = Policy.Handle<SqlException>(e => e.IsDeadlockOrTimeout())
                    .WaitAndRetryAsync(RetryCount, WaitInterval)
                    .ExecuteAndCaptureAsync(() => ipsAdapter.InsertAsync(item));
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

        private async Task<Adapters.Oal.dbo_delivery_console_event_new> CloneChildAsync(Adapters.Oal.dbo_delivery_console_event_new parent, string consignmentNo)
        {
            var console = IsConsole(consignmentNo);
            var child = parent.Clone();
            child.date_created_oal_date_field = DateTime.Now;
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