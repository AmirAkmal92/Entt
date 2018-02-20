using Bespoke.PosEntt.Adapters.Entt;
using Bespoke.PosEntt.Integrations.Transforms;
using Bespoke.PosEntt.RecordStatuses.Domain;
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
    [DesignerMetadata(Name = "StatWithChildrenAction", TypeName = "Entt.Acceptance.CustomActions.StatWithChildrenAction,entt.acceptance.customactions", Description = "Entt STAT with child items", FontAwesomeIcon = "euro")]
    public class StatWithChildrenAction : EventWithChildrenAction
    {
        private List<Entt_StatusCode> m_statEventRows;
        private List<Entt_EventPendingConsole> m_statEventPendingConsoleRows;

        public override async Task ExecuteAsync(RuleContext context)
        {
            var stat = context.Item as RecordStatus;
            if (null == stat) return;
            var isConsole = IsConsole(stat.ConsignmentNo);
            if (!isConsole) return;
            
            var swca = new StatWithChildrenAction()
            {
                m_statEventRows = new List<Entt_StatusCode>(),
                m_statEventPendingConsoleRows = new List<Entt_EventPendingConsole>()
            };
            await swca.RunAsync(stat);
        }

        public async Task RunAsync(RecordStatus stat)
        {
            //console_details
            var consoleList = new List<string>();
            if (IsConsole(stat.ConsignmentNo)) consoleList.Add(stat.ConsignmentNo);
            
            var statEventMap = new ReportStatusToEnttStatus();
            var statConsoleRow = await statEventMap.TransformAsync(stat);
            statConsoleRow.Id = GenerateId(34);
            m_statEventRows.Add(statConsoleRow);

            var consoleItem = await GetItemConsigmentsFromConsoleDetailsAsync(stat.ConsignmentNo);
            if (null != consoleItem)
            {
                var children = consoleItem.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in children)
                {
                    if (consoleList.Contains(item)) continue;
                    ProcessChild(statConsoleRow, item);

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
                                ProcessChild(statConsoleRow, cc);

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
                                            ProcessChild(statConsoleRow, ccc);
                                        }
                                    }
                                    else
                                    {
                                        AddPendingItems(statConsoleRow.Id, cc);
                                    }
                                }
                            }
                        }
                        else
                        {
                            AddPendingItems(statConsoleRow.Id, item);
                        }
                    }
                }
            }
            else
            {
                AddPendingItems(statConsoleRow.Id, stat.ConsignmentNo);
            }

            var statEventAdapter = new Entt_StatusCodeAdapter();
            foreach (var item in m_statEventRows)
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
            foreach (var item in m_statEventPendingConsoleRows)
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

        private void ProcessChild(Entt_StatusCode parent, string consignmentNo)
        {
            var isConsole = IsConsole(consignmentNo);
            var child = parent.Clone();
            child.Id = GenerateId(34);
            child.ConsignmentNo = consignmentNo;
            child.DataFlag = "1";
            child.ItemTypeCode = isConsole ? "02" : "01";
            m_statEventRows.Add(child);
        }

        private void AddPendingItems(string parentId, string consignmentNo)
        {
            var pendingConsole = new Entt_EventPendingConsole
            {
                Id = GenerateId(20),
                EventId = parentId,
                EventName = "StatusCode",
                ConsoleNo = consignmentNo,
                Version = 0,
                DateTime = DateTime.Now
            };
            m_statEventPendingConsoleRows.Add(pendingConsole);
        }


        public override string GetEditorViewModel()
        {
            return @"
define([""services/datacontext"", 'services/logger', 'plugins/dialog', objectbuilders.system],
    function(context, logger, dialog, system) {

                bespoke.sph.domain.StatWithChildrenAction = function(optionOrWebid) {

                    const v = new bespoke.sph.domain.CustomAction(optionOrWebid);
                    v[""$type""] = ""Entt.Acceptance.CustomActions.StatWithChildrenAction,entt.acceptance.customactions"";
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

                    if (bespoke.sph.domain.StatWithChildrenActionPartial)
                    {
                        return _(v).extend(new bespoke.sph.domain.StatWithChildrenActionPartial(v));
                    }
                    return v;
                };



        const action = ko.observable(new bespoke.sph.domain.StatWithChildrenAction(system.guid())),
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
