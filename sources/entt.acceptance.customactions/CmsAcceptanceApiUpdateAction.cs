using Bespoke.Sph.Domain;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using Bespoke.PosEntt.EnttAcceptances.Domain;

namespace Entt.Acceptance.CustomActions
{
    [Export(typeof(CustomAction))]
    [DesignerMetadata(Name = "CmsAcceptanceApiUpdateAction", TypeName = "Entt.Acceptance.CustomActions.CmsAcceptanceApiUpdateAction,entt.acceptance.customactions", Description = "Entt Acceptance update for CMS", FontAwesomeIcon = "envelope")]
    public class CmsAcceptanceApiUpdateAction : CustomAction
    {
        public override bool UseAsync => true;

        public override async Task ExecuteAsync(RuleContext context)
        {
            var acceptance = context.Item as EnttAcceptance;
            if (null == acceptance) return;
            await Run(acceptance);
        }

        public async Task Run(EnttAcceptance acceptance)
        {
            await UpdateCmsAcceptanceAsync(acceptance);
        }

        private async Task UpdateCmsAcceptanceAsync(EnttAcceptance acceptance)
        {
            var client = new HttpClient();
            var cmsXUserKey = ConfigurationManager.GetEnvironmentVariable("Cms_XUserKey");
            client.DefaultRequestHeaders.Add("X-User-Key", cmsXUserKey);
            var cmsApiUrl = ConfigurationManager.GetEnvironmentVariable("Cms_ApiUrl");

            var keyValuePairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("id", acceptance.ConsignmentNo),
                new KeyValuePair<string, string>("AcceptanceBy", acceptance.CourierId)
            };
            var response = await client.PutAsync(cmsApiUrl, new FormUrlEncodedContent(keyValuePairs.ToArray()));
            var content = await response.Content.ReadAsStringAsync();
        }

        public override string GetEditorViewModel()
        {
            return @"
define([""services/datacontext"", 'services/logger', 'plugins/dialog', objectbuilders.system],
    function(context, logger, dialog, system) {

                bespoke.sph.domain.CmsAcceptanceApiUpdateAction = function(optionOrWebid) {

                    const v = new bespoke.sph.domain.CustomAction(optionOrWebid);
                    v[""$type""] = ""Entt.Acceptance.CustomActions.CmsAcceptanceApiUpdateAction,entt.acceptance.customactions"";
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

                    if (bespoke.sph.domain.CmsAcceptanceApiUpdateActionPartial)
                    {
                        return _(v).extend(new bespoke.sph.domain.CmsAcceptanceApiUpdateActionPartial(v));
                    }
                    return v;
                };



        const action = ko.observable(new bespoke.sph.domain.CmsAcceptanceApiUpdateAction(system.guid())),
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
                <h3>{this.GetType().Name}</h3>
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
