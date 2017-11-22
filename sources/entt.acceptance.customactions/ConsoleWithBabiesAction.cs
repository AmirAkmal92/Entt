using Bespoke.PosEntt.Adapters.Entt;
using Bespoke.PosEntt.Integrations.Transforms;
using Bespoke.PosEntt.RecordConsoles.Domain;
using Bespoke.Sph.Domain;
using Polly;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entt.Acceptance.CustomActions
{
    [Export(typeof(CustomAction))]
    [DesignerMetadata(Name = "ConsoleWithBabiesAction", TypeName = "Entt.Acceptance.CustomActions.ConsoleWithBabiesAction,entt.acceptance.customactions", Description = "Entt Console with babies", FontAwesomeIcon = "euro")]
    public partial class ConsoleWithBabiesAction : EventWithChildrenAction
    {
        public static int RetryCount = ConfigurationManager.GetEnvironmentVariableInt32("NormRetryCount", 3);
        public Func<int, TimeSpan> WaitInterval = x => TimeSpan.FromMilliseconds(ConfigurationManager.GetEnvironmentVariableInt32("NormWaitInterval", 500) * Math.Pow(2, x));

        public override async Task ExecuteAsync(RuleContext context)
        {
            var console = context.Item as RecordConsole;
            if (null == console) return;
            if (console.TotalConsignment <= 0) return;

            await RunAsync(console);
        }

        public async Task RunAsync(RecordConsole console)
        {
            var pendingConsoles = new List<string>();
            var success = await AddToConsoleDetailsAsync(console);
            if (!success) return;

            var rows = new ConcurrentBag<Entt_Console>();
            var map = new ReportConsoleToEnttConsole();
            var row = await map.TransformAsync(console);
            rows.Add(row);
            rows.AddRange(await GetEventRowsAsync(row, pendingConsoles, console.ItemConsignments));

            //
            await InsertConsoleAsync(rows);

            //
            if (pendingConsoles.Any())
                await InsertEventPendingConsoleAsync(row, pendingConsoles.ToArray());

            var consoleDetailsAdapter = new Entt_EventPendingConsoleAdapter();
            var pendingCountPr = await Policy.Handle<SqlException>(e => e.IsTimeout())
                .WaitAndRetryAsync(RetryCount, WaitInterval)
                .ExecuteAndCaptureAsync(
                    () =>
                        consoleDetailsAdapter.ExecuteScalarAsync<int>(
                            $"SELECT COUNT(*) FROM [Entt].[EventPendingConsole] WHERE [ConsoleNo] = '{console.ConsignmentNo}'"));
            var tagExistInPendingConsole = pendingCountPr.Result > 0;
            if (tagExistInPendingConsole)
                await ProcessPendingConsoleAsync(console, rows);

        }



        public override string GetEditorViewModel()
        {
            return @"
define([""services/datacontext"", 'services/logger', 'plugins/dialog', objectbuilders.system],
    function(context, logger, dialog, system) {

                bespoke.sph.domain.ConsoleWithBabiesAction = function(optionOrWebid) {

                    const v = new bespoke.sph.domain.CustomAction(optionOrWebid);
                    v[""$type""] = ""Entt.Acceptance.CustomActions.ConsoleWithBabiesAction,entt.acceptance.customactions"";
                    if (optionOrWebid && typeof optionOrWebid === ""object"")
                    {
                        for (var n in optionOrWebid)
                        {
                            if (typeof v[n] === ""function"")
                            {
                                v[n](optionOrWebid[n]);
                            }
                        }
                    }
                    if (optionOrWebid && typeof optionOrWebid === ""string"")
                    {
                        v.WebId(optionOrWebid);
                    }

                    if (bespoke.sph.domain.ConsoleWithBabiesActionPartial)
                    {
                        return _(v).extend(new bespoke.sph.domain.ConsoleWithBabiesActionPartial(v));
                    }
                    return v;
                };



        const action = ko.observable(new bespoke.sph.domain.ConsoleWithBabiesAction(system.guid())),
            trigger = ko.observable(),                   
            activate = function() {
                   return true;

                },
            attached = function(view) {
                },
            okClick = function(data, ev) {
                    if (bespoke.utils.form.checkValidity(ev.target))
                    {
                        dialog.close(this, ""OK"");
                    }
                },
            cancelClick = function() {
                    dialog.close(this, ""Cancel"");
                };

                const vm = {
                    trigger: trigger,
                    action: action,
                    activate: activate,
                    attached: attached,
                    okClick: okClick,
                    cancelClick: cancelClick
                };
            return vm;

        });
";
        }
    }
}
