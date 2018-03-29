using Bespoke.PosEntt.Adapters.Entt;
using Bespoke.PosEntt.Integrations.Transforms;
using Bespoke.PosEntt.RecordDeliveries.Domain;
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
    [DesignerMetadata(Name = "DeliWithChildrenAction", TypeName = "Entt.Acceptance.CustomActions.DeliWithChildrenAction,entt.acceptance.customactions", Description = "Entt DELI with child items", FontAwesomeIcon = "euro")]
    public class DeliWithChildrenAction : EventWithChildrenAction
    {
        private List<Entt_Delivery> m_deliEventRows;
        private List<Entt_EventPendingConsole> m_deliEventPendingConsoleRows;

        public override async Task ExecuteAsync(RuleContext context)
        {
            var deli = context.Item as RecordDelivery;
            if (null == deli) return;
            var isConsole = IsConsole(deli.ConsignmentNo);
            if (!isConsole) return;
            
            var swca = new DeliWithChildrenAction()
            {
                m_deliEventRows = new List<Entt_Delivery>(),
                m_deliEventPendingConsoleRows = new List<Entt_EventPendingConsole>()
            };
            await swca.RunAsync(deli);
        }

        public async Task RunAsync(RecordDelivery deli)
        {
            //console_details
            var consoleList = new List<string>();
            if (IsConsole(deli.ConsignmentNo)) consoleList.Add(deli.ConsignmentNo);
            
            var deliEventMap = new ReportDeliveryToEnttDelivery();
            var deliConsoleRow = await deliEventMap.TransformAsync(deli);
            deliConsoleRow.Id = GenerateId(34);
            m_deliEventRows.Add(deliConsoleRow);

            var consoleItem = await GetItemConsigmentsFromConsoleDetailsAsync(deli.ConsignmentNo);
            if (null != consoleItem)
            {
                var children = consoleItem.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in children)
                {
                    if (consoleList.Contains(item)) continue;
                    ProcessChild(deliConsoleRow, item);

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
                                ProcessChild(deliConsoleRow, cc);

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
                                            ProcessChild(deliConsoleRow, ccc);
                                        }
                                    }
                                    else
                                    {
                                        AddPendingItems(deliConsoleRow.Id, cc);
                                    }
                                }
                            }
                        }
                        else
                        {
                            AddPendingItems(deliConsoleRow.Id, item);
                        }
                    }
                }
            }
            else
            {
                AddPendingItems(deliConsoleRow.Id, deli.ConsignmentNo);
            }

            var deliEventAdapter = new Entt_DeliveryAdapter();
            foreach (var item in m_deliEventRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => deliEventAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }

            var pendingAdapter = new Entt_EventPendingConsoleAdapter();
            foreach (var item in m_deliEventPendingConsoleRows)
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

        private void ProcessChild(Entt_Delivery parent, string consignmentNo)
        {
            var isConsole = IsConsole(consignmentNo);
            var child = parent.Clone();
            child.Id = GenerateId(34);
            child.ConsignmentNo = consignmentNo;
            child.DataFlag = "1";
            child.ItemTypeCode = isConsole ? "02" : "01";
            m_deliEventRows.Add(child);
        }

        private void AddPendingItems(string parentId, string consignmentNo)
        {
            var pendingConsole = new Entt_EventPendingConsole
            {
                Id = GenerateId(20),
                EventId = parentId,
                EventName = "Deli",
                ConsoleNo = consignmentNo,
                Version = 0,
                DateTime = DateTime.Now
            };
            m_deliEventPendingConsoleRows.Add(pendingConsole);
        }


        public override string GetEditorViewModel()
        {
            return @"
define([""services/datacontext"", 'services/logger', 'plugins/dialog', objectbuilders.system],
    function(context, logger, dialog, system) {

                bespoke.sph.domain.DeliWithChildrenAction = function(optionOrWebid) {

                    const v = new bespoke.sph.domain.CustomAction(optionOrWebid);
                    v[""$type""] = ""Entt.Acceptance.CustomActions.DeliWithChildrenAction,entt.acceptance.customactions"";
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

                    if (bespoke.sph.domain.DeliWithChildrenActionPartial)
                    {
                        return _(v).extend(new bespoke.sph.domain.DeliWithChildrenActionPartial(v));
                    }
                    return v;
                };



        const action = ko.observable(new bespoke.sph.domain.DeliWithChildrenAction(system.guid())),
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
