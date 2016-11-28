using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Polly;
using System.Linq;

namespace Bespoke.PosEntt.CustomActions
{
    [Export(typeof(CustomAction))]
    [DesignerMetadata(Name = "StatWithChildren", TypeName = "Bespoke.PosEntt.CustomActions.StatWithChildrenAction, rts.pickup.babies", Description = "RTS STAT with child items", FontAwesomeIcon = "calendar-check-o")]
    public class StatWithChildrenAction : CustomAction
    {
        public override bool UseAsync => true;
        private List<Adapters.Oal.dbo_status_code_event_new> m_statEventRows;
        private List<Adapters.Oal.dbo_wwp_event_new_log> m_statWwpEventLogRows;
        private List<Adapters.Oal.dbo_event_pending_console> m_statEventPendingConsoleRows;

        public override async Task ExecuteAsync(RuleContext context)
        {
            var stat = context.Item as Stats.Domain.Stat;
            if (null == stat) return;
            var isConsole = IsConsole(stat.ConsignmentNo);
            if (!isConsole) return;

            m_statEventRows = new List<Adapters.Oal.dbo_status_code_event_new>();
            m_statWwpEventLogRows = new List<Adapters.Oal.dbo_wwp_event_new_log>();
            m_statEventPendingConsoleRows = new List<Adapters.Oal.dbo_event_pending_console>();

            await RunAsync(stat);
        }

        public async Task RunAsync(Stats.Domain.Stat stat)
        {
            //console_details
            var consoleList = new List<string>();
            if (IsConsole(stat.ConsignmentNo)) consoleList.Add(stat.ConsignmentNo);

            var statEventAdapter = new Adapters.Oal.dbo_status_code_event_newAdapter();
            var statWwpEventAdapter = new Adapters.Oal.dbo_wwp_event_new_logAdapter();
            var statEventMap = new Integrations.Transforms.RtsStatToOalStatusCodeEventNew();
            var parentRow = await statEventMap.TransformAsync(stat);
            parentRow.id = GenerateId(34);
            m_statEventRows.Add(parentRow);

            var statWwpEventLogMap = new Integrations.Transforms.RtsStatOalWwpEventNewLog();
            var parentWwpRow = await statWwpEventLogMap.TransformAsync(stat);
            m_statWwpEventLogRows.Add(parentWwpRow);

            var consoleItem = await SearchConsoleDetails(stat.ConsignmentNo);
            if (null != consoleItem)
            {
                var children = consoleItem.item_consignments.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in children)
                {
                    if (consoleList.Contains(item)) continue;
                    ProcessChild(parentRow, item);
                    ProcessChildWwp(parentWwpRow, item);

                    //2 level
                    var console = IsConsole(item);
                    if (console)
                    {
                        consoleList.Add(item);
                        var childConsole = await SearchConsoleDetails(item);
                        if (null != childConsole)
                        {
                            var childConsoleItems = childConsole.item_consignments.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var cc in childConsoleItems)
                            {
                                if (consoleList.Contains(cc)) continue;
                                ProcessChild(parentRow, cc);
                                ProcessChildWwp(parentWwpRow, cc);

                                //3 level
                                var anotherConsole = IsConsole(cc);
                                if (anotherConsole)
                                {
                                    consoleList.Add(cc);
                                    var anotherChildConsole = await SearchConsoleDetails(cc);
                                    if (null != anotherChildConsole)
                                    {
                                        var anotherChildConsoleItems = anotherChildConsole.item_consignments.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (var ccc in anotherChildConsoleItems)
                                        {
                                            if (consoleList.Contains(ccc)) continue;
                                            ProcessChild(parentRow, ccc);
                                            ProcessChildWwp(parentWwpRow, ccc);
                                        }
                                    }
                                    else
                                    {
                                        AddPendingItems(parentRow.id, cc);
                                    }
                                }
                            }
                        }
                        else
                        {
                            AddPendingItems(parentRow.id, item);
                        }
                    }
                }
            }
            else
            {
                AddPendingItems(parentRow.id, stat.ConsignmentNo);
            }

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

            foreach (var item in m_statWwpEventLogRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => statWwpEventAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }

            foreach (var item in m_statEventPendingConsoleRows)
            {
                var pendingAdapter = new Adapters.Oal.dbo_event_pending_consoleAdapter();
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => pendingAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }

        async private Task<Adapters.Oal.dbo_console_details> SearchConsoleDetails(string consignmentNo)
        {
            var consoleDetailsAdapter = new Adapters.Oal.dbo_console_detailsAdapter();
            var query = string.Format("SELECT * FROM [dbo].[console_details] WHERE console_no = '{0}'", consignmentNo);
            var lo = await consoleDetailsAdapter.LoadAsync(query);
            return lo.ItemCollection.FirstOrDefault();
        }

        private void ProcessChild(Adapters.Oal.dbo_status_code_event_new parent, string consignmentNo)
        {
            var console = IsConsole(consignmentNo);
            var child = parent.Clone();
            child.id = GenerateId(34);
            child.consignment_no = consignmentNo;
            child.data_flag = "1";
            child.item_type_code = console ? "02" : "01";
            m_statEventRows.Add(child);
        }

        private void AddPendingItems(string parentId, string consignmentNo)
        {
            //insert into pending items
            var pendingConsole = new Adapters.Oal.dbo_event_pending_console();
            pendingConsole.id = GenerateId(20);
            pendingConsole.event_id = parentId;
            pendingConsole.event_class = "pos.oal.StatusEventNew";
            pendingConsole.console_no = consignmentNo;
            pendingConsole.version = 0;
            pendingConsole.date_field = DateTime.Now;
            m_statEventPendingConsoleRows.Add(pendingConsole);

            var pendingWwp = new Adapters.Oal.dbo_event_pending_console();
            pendingWwp.id = GenerateId(20);
            pendingWwp.event_id = parentId;
            pendingWwp.event_class = "pos.oal.WwpEventNewLog";
            pendingWwp.console_no = consignmentNo;
            pendingWwp.version = 0;
            pendingWwp.date_field = DateTime.Now;
            m_statEventPendingConsoleRows.Add(pendingWwp);
        }

        private void ProcessChildWwp(Adapters.Oal.dbo_wwp_event_new_log statWwp, string childConnoteNo)
        {
            var wwp = statWwp.Clone();
            wwp.id = GenerateId(34);
            wwp.consignment_note_number = childConnoteNo;
            m_statWwpEventLogRows.Add(wwp);
        }

        public override string GetEditorViewModel()
        {
            return @"
define([""services/datacontext"", 'services/logger', 'plugins/dialog', objectbuilders.system],
    function(context, logger, dialog, system) {

                bespoke.sph.domain.StatWithChildrenAction = function(optionOrWebid) {

                    const v = new bespoke.sph.domain.CustomAction(optionOrWebid);
                    v[""$type""] = ""Bespoke.PosEntt.CustomActions.StatWithChildrenAction, rts.pickup.babies"";
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

        public override string GetEditorView()
        {
            //language=html
            var html = @"<section class=""view-model-modal"" id=""messaging-action-dialog"">
    <div class=""modal-dialog"">
        <div class=""modal-content"">

            <div class=""modal-header"">
                <button type=""button"" class=""close"" data-dismiss=""modal"" data-bind=""click : cancelClick"">&times;</button>
                <h3>RTS STAT with child items</h3>
            </div>
            <div class=""modal-body"" data-bind=""with: action"">

                <form class=""form-horizontal"" id=""messaging-dialog-form"">


                </form>
            </div>
            <div class=""modal-footer"">
                <input form=""messaging-dialog-form"" data-dismiss=""modal"" type=""submit"" class=""btn btn-default"" value=""OK"" data-bind=""click: okClick"" />
                <a href=""#"" class=""btn btn-default"" data-dismiss=""modal"" data-bind=""click : cancelClick"">Cancel</a>
            </div>
        </div>
    </div>
</section>";

            return html;
        }

        private string GenerateId(int length)
        {
            var id = string.Format("en{0}", System.Guid.NewGuid().ToString("N"));
            return id.Substring(0, length);
        }

        private bool IsConsole(string connoteNo)
        {
            var pattern = "CG[0-9]{9}MY";
            var match = System.Text.RegularExpressions.Regex.Match(connoteNo, pattern);
            return match.Success;
        }
    }
}