using Bespoke.PosEntt.MailItems.Domain;
using Bespoke.Sph.Domain;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.PosEntt.CustomActions
{
    [Export(typeof(CustomAction))]
    [DesignerMetadata(Name = "IpsExportAcceptance", TypeName = "Bespoke.PosEntt.CustomActions.IpsExportAcceptanceAction, rts.ips.export.trigger", Description = "RTS IPS Export acceptance", FontAwesomeIcon = "database")]
    public class IpsExportAcceptanceAction : CustomAction
    {
        public override bool UseAsync => true;
        public override async Task ExecuteAsync(RuleContext context)
        {
            var item = context.Item as MailItem;
            if (null == item) return;
            if (null == item.FromIPS) return;

            if (item.FromIPS.IPSEvent.Any(e => e.TNCd.Equals("TN030")))
                await RunAsync(item);

        }

        public async Task RunAsync(MailItem item)
        {
            var ipsExportMap = new Integrations.Transforms.IpsExportToEnttAcceptance();
            var acceptance = await ipsExportMap.TransformAsync(item);
            var tn030 = item.FromIPS.IPSEvent.Single(m => m.TNCd == "TN030");
            acceptance.DateTime = tn030.Date.Value;

            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(acceptance);
                await session.SubmitChanges();
            }
        }

        public override string GetEditorViewModel()
        {
            return @"
define([""services/datacontext"", 'services/logger', 'plugins/dialog', objectbuilders.system],
    function(context, logger, dialog, system) {

                bespoke.sph.domain.IpsExportAcceptanceAction = function(optionOrWebid) {

                    const v = new bespoke.sph.domain.CustomAction(optionOrWebid);
                    v[""$type""] = ""Bespoke.PosEntt.CustomActions.IpsExportAcceptanceAction, rts.ips.export.trigger"";
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

                    if (bespoke.sph.domain.IpsExportAcceptanceActionPartial)
                    {
                        return _(v).extend(new bespoke.sph.domain.IpsExportAcceptanceActionPartial(v));
                    }
                    return v;
                };



        const action = ko.observable(new bespoke.sph.domain.IpsExportAcceptanceAction(system.guid())),
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
                <h3>IPS EXPORT ACCEPTANCE ACTION</h3>
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
