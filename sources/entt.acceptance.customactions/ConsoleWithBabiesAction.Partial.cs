using Bespoke.PosEntt.Adapters.Entt;
using Bespoke.PosEntt.RecordConsoles.Domain;
using Bespoke.Sph.Domain;
using Polly;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entt.Acceptance.CustomActions
{
    public partial class ConsoleWithBabiesAction
    {
        private async Task<bool> AddToConsoleDetailsAsync(RecordConsole console)
        {
            var adapter = new Entt_ConsoleDetailsAdapter();
            var idPr = await Policy.Handle<SqlException>(e => e.IsTimeout())
                .WaitAndRetryAsync(RetryCount, WaitInterval)
                .ExecuteAndCaptureAsync(() => adapter.ExecuteScalarAsync<string>($"SELECT TOP 1 [Id] FROM [Entt].[ConsoleDetails] WHERE [ConsoleNo] = '{console.ConsignmentNo}' ORDER BY [DateTime] DESC"));
            var id = idPr.Result;
            var consoleTagNotInConsoleDetails = string.IsNullOrWhiteSpace(id);
            if (consoleTagNotInConsoleDetails)
            {
                var map = new Bespoke.PosEntt.Integrations.Transforms.ReportConsoleToEnttConsoleDetails();
                var row = await map.TransformAsync(console);
                var dpr = await Policy.Handle<SqlException>(e => e.IsTimeout())
                   .WaitAndRetryAsync(RetryCount, WaitInterval)
                   .ExecuteAndCaptureAsync(() => adapter.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM [Entt].[ConsoleDetails] WHERE [ConsoleNo] ='{row.ConsoleNo}'"));
                if (dpr.Result == 0 && !string.IsNullOrWhiteSpace(row.ConsoleNo))
                {
                    var pr = Policy.Handle<SqlException>(e => e.IsDeadlockOrTimeout())
                        .WaitAndRetryAsync(RetryCount, WaitInterval)
                        .ExecuteAndCaptureAsync(() => adapter.InsertAsync(row));
                    var result = await pr;
                    if (result.FinalException != null)
                        throw result.FinalException; // Send to dead letter queue
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

            if (details.CourierId == console.CourierId && details.OfficeNo == console.OfficeNo && (details.DateTime ?? DateTime.MinValue).AddHours(28) >= console.DateTime)
            {
                var notes = details.ItemConsignments.Split(new[] { ',', '\t' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                notes.AddRange(console.ItemConsignments.Split(new[] { ',', '\t' }, StringSplitOptions.RemoveEmptyEntries));
                details.ItemConsignments = string.Join("\t", notes.OrderBy(x => x).Where(x => x.Length > 3 /* remove anything less*/).Distinct());
                await adapter.UpdateAsync(details);
                return true;
            }

            await InsertConsoleDuplicationErrorAndEventExceptionAsync(console);
            return false;
        }

        private async Task InsertConsoleDuplicationErrorAndEventExceptionAsync(RecordConsole console)
        {
            Console.WriteLine("insert into console_duplicate_error");

            //
            var error = new Entt_ConsoleDuplicateError
            {
                Id = GenerateId(20),
                Version = 0, 
                DateTime = console.DateTime,
                CourierId = console.CourierId,
                CourierName = console.CourierName,
                OfficeName = console.OfficeName,
                OfficeNextCode = console.OfficeNextCode,
                OfficeNo = console.OfficeNo,
                BatchName = console.BatchName,
                BeatNo = console.BeatNo,
                ConsoleNo = console.ConsignmentNo,
                ItemConsignments = console.ItemConsignments,
                ConsoleType = console.ConsoleType,
                ConsoleTypeDesc = console.ConsoleTypeDesc,
                OtherConsoleType = console.OtherConsoleType,
                DestinationOffice = console.DestinationOffice,
                DestinationOfficeName = console.DestinationOfficeName,
                RoutingCode = console.RoutingCode,
                Comment = console.Comment,
                EventType = console.EventType,
                CreatedDate = console.CreatedDate                
            };
            var errorAdapter = new Entt_ConsoleDuplicateErrorAdapter();
            await Policy.Handle<SqlException>(e => e.IsDeadlockOrTimeout())
                .WaitAndRetryAsync(RetryCount, WaitInterval)
                .ExecuteAndCaptureAsync(() => errorAdapter.InsertAsync(error));

        }

        private async Task<IEnumerable<Entt_Console>> GetEventRowsAsync(
            Entt_Console eventRow,
            IList<string> pendingConsoles,
            string consignmentNotes,
            int level = 0)
        {
            Console.Write("." + level);

            var list = new ConcurrentBag<Entt_Console>();
            if (level > 4)
                return list;
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
                var children = await GetEventRowsAsync(eventRow,
                    pendingConsoles,
                    childConsignments.ToEmptyString().Trim(),
                    level + 1);
                list.AddRange(children);

            }

            return list;
        }

        private async Task<Entt_Console> CloneChildAsync(Entt_Console parent, string consignmentNo)
        {
            var console = IsConsole(consignmentNo);
            var child = parent.Clone();
            child.Id = GenerateId(34);
            child.ConsignmentNo = consignmentNo;
            child.DataFlag = "1";
            child.ItemTypeCode = console ? "02" : "01";
            if (console)
                child.ItemConsignments = await this.GetItemConsigmentsFromConsoleDetailsAsync(consignmentNo);

            return child;

        }

        private async Task InsertConsoleAsync(IEnumerable<Entt_Console> rows)
        {
            var consoleAdapter = new Entt_ConsoleAdapter();
            foreach (var item in rows)
            {
                Console.Write(".");
                if (string.IsNullOrWhiteSpace(item.ItemConsignments))
                    continue;

                var pr0 = Policy.Handle<SqlException>(e => e.IsDeadlockOrTimeout())
                    .WaitAndRetryAsync(RetryCount, WaitInterval)
                    .ExecuteAndCaptureAsync(() => consoleAdapter.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM [Entt].[Console] WHERE [Id]='{item.Id}'"));
                var result0 = await pr0;
                if (result0.FinalException != null)
                    throw result0.FinalException;
                if (result0.Result > 0) continue;

                var pr = Policy.Handle<SqlException>(e => e.IsDeadlockOrTimeout())
                    .WaitAndRetryAsync(RetryCount, WaitInterval)
                    .ExecuteAndCaptureAsync(() => consoleAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }

        private async Task InsertEventPendingConsoleAsync(Entt_Console parent, params string[] connoteNotes)
        {
            var pendingAdapter = new Entt_EventPendingConsoleAdapter();
            var rows = new List<Entt_EventPendingConsole>();

            foreach (var connoteNo in connoteNotes)
            {
                rows.Add(GetPendingConsoleRow(parent.Id, connoteNo, parent.EventType));
                foreach (var item in rows)
                {
                    // verify the id, is not duplicate
                    var onePr = await Policy.Handle<SqlException>(e => e.IsTimeout())
                        .WaitAndRetryAsync(RetryCount, WaitInterval)
                        .ExecuteAndCaptureAsync(() => pendingAdapter.LoadOneAsync(item.Id));
                    var one = onePr.Result;
                    if (null != one)
                        continue;
                    await Policy.Handle<SqlException>(e => e.IsDeadlockOrTimeout())
                       .WaitAndRetryAsync(RetryCount, WaitInterval)
                       .ExecuteAndCaptureAsync(() => pendingAdapter.InsertAsync(item));
                }
            }
        }

        private Entt_EventPendingConsole GetPendingConsoleRow(string parentId, string consignmentNo, string eventType)
        {
            var pendingConsole = new Entt_EventPendingConsole
            {
                Id = GenerateId(20),
                EventId = parentId,
                EventName = eventType,
                ConsoleNo = consignmentNo,
                Version = 0,
                DateTime = DateTime.Now
            };
            return pendingConsole;
        }
    }
}
