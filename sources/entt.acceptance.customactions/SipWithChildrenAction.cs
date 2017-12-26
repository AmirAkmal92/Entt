using Bespoke.PosEntt.Adapters.Entt;
using Bespoke.PosEntt.Integrations.Transforms;
using Bespoke.PosEntt.RecordSips.Domain;
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
    [DesignerMetadata(Name = "SipWithChildrenAction", TypeName = "Entt.Acceptance.CustomActions.SipWithChildrenAction,entt.acceptance.customactions", Description = "Entt SIP with child items", FontAwesomeIcon = "euro")]
    public class SipWithChildrenAction : EventWithChildrenAction
    {
        private List<Entt_Sip> m_sipEventRows;
        private List<Entt_EventPendingConsole> m_sipEventPendingConsoleRows;

        public override async Task ExecuteAsync(RuleContext context)
        {
            var sip = context.Item as RecordSip;
            if (null == sip) return;
            var isConsole = IsConsole(sip.ConsignmentNo);
            if (!isConsole) return;
            
            var swca = new SipWithChildrenAction()
            {
                m_sipEventRows = new List<Entt_Sip>(),
                m_sipEventPendingConsoleRows = new List<Entt_EventPendingConsole>()
            };
            await swca.RunAsync(sip);
        }

        public async Task RunAsync(RecordSip sip)
        {
            //console_details
            var consoleList = new List<string>();
            if (IsConsole(sip.ConsignmentNo)) consoleList.Add(sip.ConsignmentNo);
            
            var sipEventMap = new ReportSipToEnttSip();
            var sipConsoleRow = await sipEventMap.TransformAsync(sip);
            sipConsoleRow.Id = GenerateId(34);
            m_sipEventRows.Add(sipConsoleRow);

            var consoleItem = await GetItemConsigmentsFromConsoleDetailsAsync(sip.ConsignmentNo);
            if (null != consoleItem)
            {
                var children = consoleItem.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in children)
                {
                    if (consoleList.Contains(item)) continue;
                    ProcessChild(sipConsoleRow, item);

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
                                ProcessChild(sipConsoleRow, cc);

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
                                            ProcessChild(sipConsoleRow, ccc);
                                        }
                                    }
                                    else
                                    {
                                        AddPendingItems(sipConsoleRow.Id, cc);
                                    }
                                }
                            }
                        }
                        else
                        {
                            AddPendingItems(sipConsoleRow.Id, item);
                        }
                    }
                }
            }
            else
            {
                AddPendingItems(sipConsoleRow.Id, sip.ConsignmentNo);
            }

            var sipEventAdapter = new Entt_SipAdapter();
            foreach (var item in m_sipEventRows)
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
            foreach (var item in m_sipEventPendingConsoleRows)
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

        private void ProcessChild(Entt_Sip parent, string consignmentNo)
        {
            var isConsole = IsConsole(consignmentNo);
            var child = parent.Clone();
            child.Id = GenerateId(34);
            child.ConsignmentNo = consignmentNo;
            child.DataFlag = "1";
            child.ItemTypeCode = isConsole ? "02" : "01";
            m_sipEventRows.Add(child);
        }

        private void AddPendingItems(string parentId, string consignmentNo)
        {
            var pendingConsole = new Entt_EventPendingConsole
            {
                Id = GenerateId(20),
                EventId = parentId,
                EventName = "Sip",
                ConsoleNo = consignmentNo,
                Version = 0,
                DateTime = DateTime.Now
            };
            m_sipEventPendingConsoleRows.Add(pendingConsole);
        }


        public override string GetEditorViewModel()
        {
            return @"
define([""services/datacontext"", 'services/logger', 'plugins/dialog', objectbuilders.system],
    function(context, logger, dialog, system) {

                bespoke.sph.domain.SipWithChildrenAction = function(optionOrWebid) {

                    const v = new bespoke.sph.domain.CustomAction(optionOrWebid);
                    v[""$type""] = ""Entt.Acceptance.CustomActions.SipWithChildrenAction,entt.acceptance.customactions"";
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

                    if (bespoke.sph.domain.SipWithChildrenActionPartial)
                    {
                        return _(v).extend(new bespoke.sph.domain.SipWithChildrenActionPartial(v));
                    }
                    return v;
                };



        const action = ko.observable(new bespoke.sph.domain.SipWithChildrenAction(system.guid())),
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
