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
    [DesignerMetadata(Name = "SopWithChildren", TypeName = "Bespoke.PosEntt.CustomActions.SopWithChildrenAction, rts.pickup.babies", Description = "RTS SOP with child items", FontAwesomeIcon = "calendar-check-o")]
    public class SopWithChildrenAction : CustomAction
    {
        public override bool UseAsync => true;
        public override async Task ExecuteAsync(RuleContext context)
        {
            var sop = context.Item as Sops.Domain.Sop;
            if (null == sop) return;
            await RunAsync(sop);
        }

        public async Task RunAsync(Sops.Domain.Sop sop)
        {
            //console_details
            var consoleList = new List<string>();
            if (IsConsole(sop.ConsignmentNo)) consoleList.Add(sop.ConsignmentNo);

            var consoleDetailsAdapter = new Adapters.Oal.dbo_console_detailsAdapter();
            var query = string.Format("SELECT * FROM [dbo].[console_details] WHERE console_no = '{0}'", sop.ConsignmentNo);
            var lo = await consoleDetailsAdapter.LoadAsync(query);
            var consoleItem = lo.ItemCollection.FirstOrDefault();
            if (null == consoleItem)
            {
                //insert into pending items
                var pending = new Adapters.Oal.dbo_event_pending_console();
                pending.id = GenerateId(20);
                pending.event_class = "pos.oal.SopEventNew";
                pending.console_no = sop.ConsignmentNo;
                pending.version = 0;
                pending.date_field = DateTime.Now;

                var pendingAdapter = new Adapters.Oal.dbo_event_pending_consoleAdapter();
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => pendingAdapter.InsertAsync(pending));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");

                return;
            }

            var children = consoleItem.item_consignments.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
            var sopEventMap = new Integrations.Transforms.RtsSopToOalSopEventNew();
            var sopEventRows = new List<Adapters.Oal.dbo_sop_event_new>();
            var sopWwpEventLogMap = new Integrations.Transforms.RtsSopToOalSopWwpEventNewLog();
            var sopWwpEventLogRows = new List<Adapters.Oal.dbo_wwp_event_new_log>();

            foreach (var item in children)
            {
                if (consoleList.Contains(item)) continue;

                var console = IsConsole(item);
                var sop1 = sop.Clone();
                var child = await sopEventMap.TransformAsync(sop1);
                child.id = GenerateId(34);
                child.consignment_no = item;
                child.data_flag = "1";
                child.item_type_code = console ? "02" : "01";
                sopEventRows.Add(child);

                if (console)
                {
                    consoleList.Add(item);

                    var wwp = await sopWwpEventLogMap.TransformAsync(sop1);
                    wwp.id = GenerateId(34);
                    wwp.consignment_note_number = item;
                    sopWwpEventLogRows.Add(wwp);
                }
            }
            
            var sopEventAdapter = new Adapters.Oal.dbo_sop_event_newAdapter();
            foreach (var item in sopEventRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => sopEventAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }

            var sopWwpEventAdapter = new Adapters.Oal.dbo_wwp_event_new_logAdapter();
            foreach (var item in sopWwpEventLogRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => sopWwpEventAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }

        public override string GetEditorViewModel()
        {
            return @"
define([""services/datacontext"", 'services/logger', 'plugins/dialog', objectbuilders.system],
    function(context, logger, dialog, system) {

                bespoke.sph.domain.SopWithChildrenAction = function(optionOrWebid) {

                    const v = new bespoke.sph.domain.CustomAction(optionOrWebid);
                    v[""$type""] = ""Bespoke.PosEntt.CustomActions.SopWithChildrenAction, rts.pickup.babies"";
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

        public override string GetEditorView()
        {
            //language=html
            var html = @"<section class=""view-model-modal"" id=""messaging-action-dialog"">
    <div class=""modal-dialog"">
        <div class=""modal-content"">

            <div class=""modal-header"">
                <button type=""button"" class=""close"" data-dismiss=""modal"" data-bind=""click : cancelClick"">&times;</button>
                <h3>RTS SOP with child items</h3>
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
            var ticks = System.DateTime.Now.Ticks;
            var id = string.Format("en{0}{1}", ticks.ToString().Substring(6), System.Guid.NewGuid().ToString("N"));
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