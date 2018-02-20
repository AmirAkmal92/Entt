using Bespoke.PosEntt.Adapters.Entt;
using Bespoke.PosEntt.Integrations.Transforms;
using Bespoke.PosEntt.RecordVasns.Domain;
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
    [DesignerMetadata(Name = "VasnWithChildrenAction", TypeName = "Entt.Acceptance.CustomActions.VasnWithChildrenAction,entt.acceptance.customactions", Description = "Entt VASN with child items", FontAwesomeIcon = "euro")]
    public class VasnWithChildrenAction : EventWithChildrenAction
    {
        private List<Entt_Vasn> m_vasnEventRows;
        private List<Entt_EventPendingConsole> m_vasnEventPendingConsoleRows;

        public override async Task ExecuteAsync(RuleContext context)
        {
            var vasn = context.Item as RecordVasn;
            if (null == vasn) return;
            var isConsole = IsConsole(vasn.ConsignmentNo);
            if (!isConsole) return;
            
            var swca = new VasnWithChildrenAction()
            {
                m_vasnEventRows = new List<Entt_Vasn>(),
                m_vasnEventPendingConsoleRows = new List<Entt_EventPendingConsole>()
            };
            await swca.RunAsync(vasn);
        }

        public async Task RunAsync(RecordVasn vasn)
        {
            //console_details
            var consoleList = new List<string>();
            if (IsConsole(vasn.ConsignmentNo)) consoleList.Add(vasn.ConsignmentNo);
            
            var vasnEventMap = new RecordVasnToEnttVasn();
            var vasnConsoleRow = await vasnEventMap.TransformAsync(vasn);
            vasnConsoleRow.Id = GenerateId(34);
            m_vasnEventRows.Add(vasnConsoleRow);

            var consoleItem = await GetItemConsigmentsFromConsoleDetailsAsync(vasn.ConsignmentNo);
            if (null != consoleItem)
            {
                var children = consoleItem.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in children)
                {
                    if (consoleList.Contains(item)) continue;
                    ProcessChild(vasnConsoleRow, item);

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
                                ProcessChild(vasnConsoleRow, cc);

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
                                            ProcessChild(vasnConsoleRow, ccc);
                                        }
                                    }
                                    else
                                    {
                                        AddPendingItems(vasnConsoleRow.Id, cc);
                                    }
                                }
                            }
                        }
                        else
                        {
                            AddPendingItems(vasnConsoleRow.Id, item);
                        }
                    }
                }
            }
            else
            {
                AddPendingItems(vasnConsoleRow.Id, vasn.ConsignmentNo);
            }

            var statEventAdapter = new Entt_VasnAdapter();
            foreach (var item in m_vasnEventRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => statEventAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }

            var pendingAdapter = new Entt_EventPendingConsoleAdapter();
            foreach (var item in m_vasnEventPendingConsoleRows)
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

        private void ProcessChild(Entt_Vasn parent, string consignmentNo)
        {
            var isConsole = IsConsole(consignmentNo);
            var child = parent.Clone();
            child.Id = GenerateId(34);
            child.ConsignmentNo = consignmentNo;
            child.DataFlag = "1";
            child.ItemTypeCode = isConsole ? "02" : "01";
            m_vasnEventRows.Add(child);
        }

        private void AddPendingItems(string parentId, string consignmentNo)
        {
            var pendingConsole = new Entt_EventPendingConsole
            {
                Id = GenerateId(20),
                EventId = parentId,
                EventName = "Vasn",
                ConsoleNo = consignmentNo,
                Version = 0,
                DateTime = DateTime.Now
            };
            m_vasnEventPendingConsoleRows.Add(pendingConsole);
        }


        public override string GetEditorViewModel()
        {
            return @"
define([""services/datacontext"", 'services/logger', 'plugins/dialog', objectbuilders.system],
    function(context, logger, dialog, system) {

                bespoke.sph.domain.VasnWithChildrenAction = function(optionOrWebid) {

                    const v = new bespoke.sph.domain.CustomAction(optionOrWebid);
                    v[""$type""] = ""Entt.Acceptance.CustomActions.VasnWithChildrenAction,entt.acceptance.customactions"";
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

                    if (bespoke.sph.domain.VasnWithChildrenActionPartial)
                    {
                        return _(v).extend(new bespoke.sph.domain.VasnWithChildrenActionPartial(v));
                    }
                    return v;
                };



        const action = ko.observable(new bespoke.sph.domain.VasnWithChildrenAction(system.guid())),
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
