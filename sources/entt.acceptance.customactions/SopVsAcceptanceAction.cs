using Bespoke.PosEntt.Adapters.Entt;
using Bespoke.PosEntt.Integrations.Transforms;
using Bespoke.PosEntt.RecordSops.Domain;
using Bespoke.Sph.Domain;
using Polly;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Entt.Acceptance.CustomActions
{
    [Export(typeof(CustomAction))]
    [DesignerMetadata(Name = "SopVsAcceptanceAction", TypeName = "Entt.Acceptance.CustomActions.SopVsAcceptanceAction,entt.acceptance.customactions", Description = "Entt SOP vs Acceptance", FontAwesomeIcon = "truck")]
    public class SopVsAcceptanceAction : EventWithChildrenAction
    {
        private List<Entt_Unknown> m_unknownRows;

        public override async Task ExecuteAsync(RuleContext context)
        {
            var sop = context.Item as RecordSop;
            if (null == sop) return;
            var isConsole = IsConsole(sop.ConsignmentNo);
            if (!isConsole) return;

            var swca = new SopVsAcceptanceAction()
            {
                m_unknownRows = new List<Entt_Unknown>()
            };
            await swca.RunAsync(sop);
        }

        public async Task RunAsync(RecordSop sop)
        {
            //console_details
            var consoleList = new List<string>();
            if (IsConsole(sop.ConsignmentNo)) consoleList.Add(sop.ConsignmentNo);
            
            var sopEventMap = new RecordSopToEnttSop();
            var sopConsoleRow = await sopEventMap.TransformAsync(sop);

            var consoleItem = await GetItemConsigmentsFromConsoleDetailsAsync(sop.ConsignmentNo);
            if (null != consoleItem)
            {
                var children = consoleItem.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in children)
                {
                    if (consoleList.Contains(item)) continue;
                    await ValidateConsoleChild(sopConsoleRow, item);

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
                                await ValidateConsoleChild(sopConsoleRow, cc);

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
                                            await ValidateConsoleChild(sopConsoleRow, ccc);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            var adapter = new Entt_UnknownAdapter();
            foreach (var item in m_unknownRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => adapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }            
        }

        private async Task ValidateConsoleChild(Entt_Sop parent, string consignmentNo)
        {
            var isConsole = IsConsole(consignmentNo);
            if (isConsole) return;

            var itemExist = await IsExistAsync(consignmentNo);
            if (!itemExist)
            {
                var unknown = new Entt_Unknown
                {
                    Id = Guid.NewGuid().ToString(),
                    ConsignmentNo = consignmentNo,
                    EventType = "sop",
                    ScannerId = parent.ScannerId,
                    CourierId = parent.CourierId,
                    LocationId = parent.OfficeNo,
                    Date = parent.DateTime?.ToString("ddMMyyyy"),
                    Time = parent.DateTime?.ToString("HHmmss"),
                    Status = "1",
                    CreatedDate = DateTime.Now,
                    CreatedBy = parent.CreatedBy,
                    ChangedDate = DateTime.Now,
                    ChangedBy = parent.ChangedBy
                };
                m_unknownRows.Add(unknown);
            }            
        }

        private async Task<bool> IsExistAsync(string consignmentNo)
        {
            var adapter = new Entt_AcceptanceAdapter();
            var query = $"SELECT TOP 1 [Id] FROM [Entt].[Acceptance] WITH (NOLOCK) WHERE [ConsignmentNo] = '{consignmentNo}'";
            var id = await adapter.ExecuteScalarAsync<string>(query);
            return !string.IsNullOrEmpty(id);
        }

        public override string GetEditorViewModel()
        {
            return @"
define([""services/datacontext"", 'services/logger', 'plugins/dialog', objectbuilders.system],
    function(context, logger, dialog, system) {

                bespoke.sph.domain.SopVsAcceptanceAction = function(optionOrWebid) {

                    const v = new bespoke.sph.domain.CustomAction(optionOrWebid);
                    v[""$type""] = ""Entt.Acceptance.CustomActions.SopVsAcceptanceAction,entt.acceptance.customactions"";
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

                    if (bespoke.sph.domain.SopVsAcceptanceActionPartial)
                    {
                        return _(v).extend(new bespoke.sph.domain.SopVsAcceptanceActionPartial(v));
                    }
                    return v;
                };



        const action = ko.observable(new bespoke.sph.domain.SopVsAcceptanceAction(system.guid())),
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
