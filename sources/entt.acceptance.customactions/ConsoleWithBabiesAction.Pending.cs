using Bespoke.PosEntt.Adapters.Entt;
using Bespoke.PosEntt.RecordConsoles.Domain;
using Bespoke.Sph.Domain;
using Polly;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Entt.Acceptance.CustomActions
{
    public partial class ConsoleWithBabiesAction
    {
        private async Task ProcessPendingConsoleAsync(RecordConsole console, IEnumerable<Entt_Console> rows)
        {
            var pendingItems = await SearchEventPendingAsync(console.ConsignmentNo);
            var tasks = from p in pendingItems
                        select ProcessEventPendingItem(console, p);
            await Task.WhenAll(tasks);

            var consolePendingList = new List<string> { console.ConsignmentNo };
            foreach (var item in rows)
            {
                var isConsole = IsConsole(item.ConsignmentNo);
                if (!isConsole) continue;
                if (consolePendingList.Contains(item.ConsignmentNo)) continue;

                consolePendingList.Add(item.ConsignmentNo);
            }
        }

        private async Task<IEnumerable<Entt_EventPendingConsole>> SearchEventPendingAsync(string consoleNo)
        {
            var pendingAdapter = new Entt_EventPendingConsoleAdapter();
            var query = $"SELECT [Id],[EventName],[EventId] FROM [Entt].[EventPendingConsole] WHERE [ConsoleNo] = '{consoleNo}'";

            var list = new List<Entt_EventPendingConsole>();
            using (var conn = new SqlConnection(pendingAdapter.ConnectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var item = new Entt_EventPendingConsole
                        {
                            Id = reader["Id"].ReadNullableString(),
                            EventName = reader["EventName"].ReadNullableString(),
                            EventId = reader["EventId"].ReadNullableString()
                        };
                        list.Add(item);
                    }
                }
            }
            return list;

        }

        private async Task ProcessEventPendingItem(RecordConsole console, Entt_EventPendingConsole pending)
        {
            var itemList = console.ItemConsignments.Split(new[] { ',', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            var ok = false;
            switch (pending.EventName)
            {
                //case "pos.oal.DeliveryEventNew":
                //    await ProcessDeliveryPendingItem(pending.event_id, itemList);
                //    ok = true;
                //    break;
                case "Sop":
                    await ProcessSopPendingItem(pending.EventId, itemList);
                    ok = true;
                    break;
                case "Sip":
                    await ProcessSipPendingItem(pending.EventId, itemList);
                    ok = true;
                    break;
                case "Hip":
                    await ProcessHipPendingItem(pending.EventId, itemList);
                    ok = true;
                    break;
                case "Hop":
                    await ProcessHopPendingItem(pending.EventId, itemList);
                    ok = true;
                    break;
                case "StatusCode":
                    await ProcessStatPendingItem(pending.EventId, itemList);
                    ok = true;
                    break;
                case "Vasn":
                    await ProcessVasnPendingItem(pending.EventId, itemList);
                    ok = true;
                    break;
                    //case "pos.oal.NormalConsoleEventNew":
                    //    //ProcessNormPendingItem(item.console_no, item.event_id);
                    //    break;
                    //case "pos.oal.MissortEventNew":
                    //    await ProcessMissPendingItem(pending.event_id, itemList);
                    //    ok = true;
                    //    break;
                    //case "pos.oal.WwpEventNewLog":
                    //    await ProcessWwpPendingItem(pending.event_id, itemList);
                    //    ok = true;
                    //    break;
                    //case "pos.oal.IpsImport":
                    //    await ProcessIpsPendingItem(pending.event_id, itemList);
                    //    ok = true;
                    //    break;
            }

            if (ok)
            {
                var pendingAdapter = new Entt_EventPendingConsoleAdapter();
                await pendingAdapter.DeleteAsync(pending.Id);
            }
        }

        private async Task ProcessSipPendingItem(string sipEventId, string[] itemList)
        {
            var sipAdapter = new Entt_SipAdapter();
            var sipPendingPolly = await Policy.Handle<SqlException>(e => e.IsTimeout())
                .WaitAndRetryAsync(RetryCount, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                .ExecuteAndCaptureAsync(async () => await sipAdapter.LoadOneAsync(sipEventId));
            if (null != sipPendingPolly.FinalException)
                throw new Exception("Process Sip Pending Polly Error", sipPendingPolly.FinalException);

            var sip = sipPendingPolly.Result;
            var sips = new List<Entt_Sip>();
            foreach (var item in itemList)
            {
                var console = IsConsole(item);
                var child = sip.Clone();
                child.Id = GenerateId(34);
                child.ConsignmentNo = item;
                child.DataFlag = "1";
                child.ItemTypeCode = console ? "02" : "01";
                sips.Add(child);
            }
            foreach (var item in sips)
            {
                var pr = Policy.Handle<SqlException>(e => e.IsDeadlockOrTimeout())
                    .WaitAndRetryAsync(RetryCount, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => sipAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }

        private async Task ProcessSopPendingItem(string sopEventId, string[] itemList)
        {
            var sopAdapter = new Entt_SopAdapter();
            var sopPendingPolly = await Policy.Handle<SqlException>(e => e.IsTimeout())
                .WaitAndRetryAsync(RetryCount, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                .ExecuteAndCaptureAsync(async () => await sopAdapter.LoadOneAsync(sopEventId));
            if (null != sopPendingPolly.FinalException)
                throw new Exception("Process Sop Pending Polly Error", sopPendingPolly.FinalException);

            var sop = sopPendingPolly.Result;
            var sops = new List<Entt_Sop>();
            foreach (var item in itemList)
            {
                var console = IsConsole(item);
                var child = sop.Clone();
                child.Id = GenerateId(34);
                child.ConsignmentNo = item;
                child.DataFlag = "1";
                child.ItemTypeCode = console ? "02" : "01";
                sops.Add(child);
            }
            foreach (var item in sops)
            {
                var pr = Policy.Handle<SqlException>(e => e.IsDeadlockOrTimeout())
                    .WaitAndRetryAsync(RetryCount, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => sopAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }

        private async Task ProcessHipPendingItem(string hipEventId, string[] itemList)
        {
            var hipAdapter = new Entt_HipAdapter();
            var hipPendingPolly = await Policy.Handle<SqlException>(e => e.IsTimeout())
                .WaitAndRetryAsync(RetryCount, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                .ExecuteAndCaptureAsync(async () => await hipAdapter.LoadOneAsync(hipEventId));
            if (null != hipPendingPolly.FinalException)
                throw new Exception("Process Hip Pending Polly Error", hipPendingPolly.FinalException);

            var hip = hipPendingPolly.Result;
            var hips = new List<Entt_Hip>();
            foreach (var item in itemList)
            {
                var console = IsConsole(item);
                var child = hip.Clone();
                child.Id = GenerateId(34);
                child.ConsignmentNo = item;
                child.DataFlag = "1";
                child.ItemTypeCode = console ? "02" : "01";
                hips.Add(child);
            }
            foreach (var item in hips)
            {
                var pr = Policy.Handle<SqlException>(e => e.IsDeadlockOrTimeout())
                    .WaitAndRetryAsync(RetryCount, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => hipAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }

        private async Task ProcessHopPendingItem(string hopEventId, string[] itemList)
        {
            var hopAdapter = new Entt_HopAdapter();
            var hopPendingPolly = await Policy.Handle<SqlException>(e => e.IsTimeout())
                .WaitAndRetryAsync(RetryCount, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                .ExecuteAndCaptureAsync(async () => await hopAdapter.LoadOneAsync(hopEventId));
            if (null != hopPendingPolly.FinalException)
                throw new Exception("Process Hop Pending Polly Error", hopPendingPolly.FinalException);

            var hop = hopPendingPolly.Result;
            var hops = new List<Entt_Hop>();
            foreach (var item in itemList)
            {
                var console = IsConsole(item);
                var child = hop.Clone();
                child.Id = GenerateId(34);
                child.ConsignmentNo = item;
                child.DataFlag = "1";
                child.ItemTypeCode = console ? "02" : "01";
                hops.Add(child);
            }
            foreach (var item in hops)
            {
                var pr = Policy.Handle<SqlException>(e => e.IsDeadlockOrTimeout())
                    .WaitAndRetryAsync(RetryCount, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => hopAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }

        private async Task ProcessStatPendingItem(string statEventId, string[] itemList)
        {
            var statAdapter = new Entt_StatusCodeAdapter();
            var statPendingPolly = await Policy.Handle<SqlException>(e => e.IsTimeout())
                .WaitAndRetryAsync(RetryCount, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                .ExecuteAndCaptureAsync(async () => await statAdapter.LoadOneAsync(statEventId));
            if (null != statPendingPolly.FinalException)
                throw new Exception("Process Stat Pending Polly Error", statPendingPolly.FinalException);

            var stat = statPendingPolly.Result;
            var stats = new List<Entt_StatusCode>();
            foreach (var item in itemList)
            {
                var console = IsConsole(item);
                var child = stat.Clone();
                child.Id = GenerateId(34);
                child.ConsignmentNo = item;
                child.DataFlag = "1";
                child.ItemTypeCode = console ? "02" : "01";
                stats.Add(child);
            }
            foreach (var item in stats)
            {
                var pr = Policy.Handle<SqlException>(e => e.IsDeadlockOrTimeout())
                    .WaitAndRetryAsync(RetryCount, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => statAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }

        private async Task ProcessVasnPendingItem(string vasnEventId, string[] itemList)
        {
            var vasnAdapter = new Entt_VasnAdapter();
            var vasnPendingPolly = await Policy.Handle<SqlException>(e => e.IsTimeout())
                .WaitAndRetryAsync(RetryCount, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                .ExecuteAndCaptureAsync(async () => await vasnAdapter.LoadOneAsync(vasnEventId));
            if (null != vasnPendingPolly.FinalException)
                throw new Exception("Process Vasn Pending Polly Error", vasnPendingPolly.FinalException);

            var vasn = vasnPendingPolly.Result;
            var vasns = new List<Entt_Vasn>();
            foreach (var item in itemList)
            {
                var console = IsConsole(item);
                var child = vasn.Clone();
                child.Id = GenerateId(34);
                child.ConsignmentNo = item;
                child.DataFlag = "1";
                child.ItemTypeCode = console ? "02" : "01";
                vasns.Add(child);
            }
            foreach (var item in vasns)
            {
                var pr = Policy.Handle<SqlException>(e => e.IsDeadlockOrTimeout())
                    .WaitAndRetryAsync(RetryCount, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => vasnAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }
    }
}
