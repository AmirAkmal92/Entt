using Bespoke.PosEntt.EnttAcceptances.Domain;
using Bespoke.PosEntt.Stats.Domain;
using Bespoke.Sph.Domain;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Bespoke.PosEntt.CustomActions
{
    [Export(typeof(CustomAction))]
    [DesignerMetadata(Name = "RtsStatusCodePupForEnttAcceptanceUpdate", TypeName = "Bespoke.PosEntt.CustomActions.RtsStatusCodePupForEnttAcceptanceUpdate, rts.ipos.boems.trigger.actions", Description = "RTS Status Code PUP Entt Platform Acceptance", FontAwesomeIcon = "database")]
    public class RtsStatusCodePupForEnttAcceptanceUpdate : CustomAction
    {
        public override bool UseAsync => true;

        public override async Task ExecuteAsync(RuleContext context)
        {
            var stat = context.Item as Stat;
            if (null == stat) return;
            await RunAsync(stat);

        }

        public async Task RunAsync(Stat stat)
        {
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                var item = await context.LoadOneAsync<EnttAcceptance>(a => a.ConsignmentNo == stat.ConsignmentNo);
                if (null != item)
                {
                    item.IsPupStatCode = true;
                    item.PupStatCodeId = stat.StatusCode;
                    item.PupStatCodeLocation = stat.LocationId;
                    item.PupStatCodeDateTime = stat.Date;

                    session.Attach(item);
                    await session.SubmitChanges();
                }
            }
        }

        public override string GetEditorViewModel()
        {
            return @"
define([""services/datacontext"", 'services/logger', 'plugins/dialog', objectbuilders.system],
    function(context, logger, dialog, system) {

                bespoke.sph.domain.RtsStatusCodePupForEnttAcceptanceUpdate = function(optionOrWebid) {

                    const v = new bespoke.sph.domain.CustomAction(optionOrWebid);
                    v[""$type""] = ""Bespoke.PosEntt.CustomActions.RtsStatusCodePupForEnttAcceptanceUpdate, rts.ipos.boems.trigger.actions"";
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

                    if (bespoke.sph.domain.RtsStatusCodePupForEnttAcceptanceUpdatePartial)
                    {
                        return _(v).extend(new bespoke.sph.domain.RtsStatusCodePupForEnttAcceptanceUpdatePartial(v));
                    }
                    return v;
                };



        const action = ko.observable(new bespoke.sph.domain.RtsStatusCodePupForEnttAcceptanceUpdate(system.guid())),
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
            // Note : remove $ string interpolation to use resharper HTML syntax
            //language=html
            var html = $@"<section class=""view-model-modal"" id=""messaging-action-dialog"">
    <div class=""modal-dialog"">
        <div class=""modal-content"">

            <div class=""modal-header"">
                <button type=""button"" class=""close"" data-dismiss=""modal"" data-bind=""click : cancelClick"">&times;</button>
                <h3>RTS Status Code PUP Entt Acceptance Action</h3>
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
    }
}
