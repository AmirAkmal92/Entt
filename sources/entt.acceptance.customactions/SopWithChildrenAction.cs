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
    [DesignerMetadata(Name = "SopWithChildrenAction", TypeName = "Entt.Acceptance.CustomActions.SopWithChildrenAction,entt.acceptance.customactions", Description = "Entt SOP with child items", FontAwesomeIcon = "euro")]
    public class SopWithChildrenAction : EventWithChildrenAction
    {
        private List<Entt_Sop> m_sopEventRows = new List<Entt_Sop>();
        private List<Entt_EventPendingConsole> m_sopEventPendingConsoleRows = new List<Entt_EventPendingConsole>();

        public override async Task ExecuteAsync(RuleContext context)
        {
            var sop = context.Item as RecordSop;
            if (null == sop) return;
            var isConsole = IsConsole(sop.ConsignmentNo);
            if (!isConsole) return;

            await RunAsync(sop);
        }

        public async Task RunAsync(RecordSop sop)
        {
            //console_details
            var consoleList = new List<string>();
            if (IsConsole(sop.ConsignmentNo)) consoleList.Add(sop.ConsignmentNo);
            
            var sopEventMap = new RecordSopToEnttSop();
            var sopConsoleRow = await sopEventMap.TransformAsync(sop);
            sopConsoleRow.Id = GenerateId(34);
            m_sopEventRows.Add(sopConsoleRow);

            var consoleItem = await GetItemConsigmentsFromConsoleDetailsAsync(sop.ConsignmentNo);
            if (null != consoleItem)
            {
                var children = consoleItem.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in children)
                {
                    if (consoleList.Contains(item)) continue;
                    ProcessChild(sopConsoleRow, item);

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
                                ProcessChild(sopConsoleRow, cc);

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
                                            ProcessChild(sopConsoleRow, ccc);
                                        }
                                    }
                                    else
                                    {
                                        AddPendingItems(sopConsoleRow.Id, cc);
                                    }
                                }
                            }
                        }
                        else
                        {
                            AddPendingItems(sopConsoleRow.Id, item);
                        }
                    }
                }
            }
            else
            {
                AddPendingItems(sopConsoleRow.Id, sop.ConsignmentNo);
            }

            var sipEventAdapter = new Entt_SopAdapter();
            foreach (var item in m_sopEventRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => sipEventAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }

            var pendingAdapter = new Entt_EventPendingConsoleAdapter();
            foreach (var item in m_sopEventPendingConsoleRows)
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


        private void ProcessChild(Entt_Sop parent, string consignmentNo)
        {
            var isConsole = IsConsole(consignmentNo);
            var child = parent.Clone();
            child.Id = GenerateId(34);
            child.ConsignmentNo = consignmentNo;
            child.DataFlag = "1";
            child.ItemTypeCode = isConsole ? "02" : "01";
            m_sopEventRows.Add(child);
        }

        private void AddPendingItems(string parentId, string consignmentNo)
        {
            var pendingConsole = new Entt_EventPendingConsole
            {
                Id = GenerateId(20),
                EventId = parentId,
                EventName = "Sop",
                ConsoleNo = consignmentNo,
                Version = 0,
                DateTime = DateTime.Now
            };
            m_sopEventPendingConsoleRows.Add(pendingConsole);
        }


        public override string GetEditorViewModel()
        {
            return @"
define([""services/datacontext"", 'services/logger', 'plugins/dialog', objectbuilders.system],
    function(context, logger, dialog, system) {

                bespoke.sph.domain.SopWithChildrenAction = function(optionOrWebid) {

                    const v = new bespoke.sph.domain.CustomAction(optionOrWebid);
                    v[""$type""] = ""Entt.Acceptance.CustomActions.SopWithChildrenAction,entt.acceptance.customactions"";
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

                    if (bespoke.sph.domain.SopWithChildrenActionPartial)
                    {
                        return _(v).extend(new bespoke.sph.domain.SopWithChildrenActionPartial(v));
                    }
                    return v;
                };



        const action = ko.observable(new bespoke.sph.domain.SopWithChildrenAction(system.guid())),
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
