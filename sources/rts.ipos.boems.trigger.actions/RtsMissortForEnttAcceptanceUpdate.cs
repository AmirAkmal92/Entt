using Bespoke.PosEntt.EnttAcceptances.Domain;
using Bespoke.PosEntt.Misses.Domain;
using Bespoke.Sph.Domain;
using Polly;
using System;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Bespoke.PosEntt.CustomActions
{
    [Export(typeof(CustomAction))]
    [DesignerMetadata(Name = "RtsMissortForEnttAcceptanceUpdate", TypeName = "Bespoke.PosEntt.CustomActions.RtsMissortForEnttAcceptanceUpdate, rts.ipos.boems.trigger.actions", Description = "RTS Missort update Entt Platform acceptance", FontAwesomeIcon = "database")]
    public class RtsMissortForEnttAcceptanceUpdate : CustomAction
    {
        public override bool UseAsync => true;

        public override async Task ExecuteAsync(RuleContext context)
        {
            var missort = context.Item as Miss;
            if (null == missort) return;
            Run(missort);
        }

        public void Run(Miss miss)
        {
            var pr = Policy.Handle<SqlException>()
                  .WaitAndRetryAsync(3, c => TimeSpan.FromMilliseconds(c * 200))
                  .ExecuteAndCaptureAsync(async () => await UpdateEnttAcceptanceMissortAsync(miss));

            pr.Wait();
            if (null != pr.Result.FinalException)
                throw new Exception("Fail updating PUP Stat", pr.Result.FinalException);
        }

        private async Task UpdateEnttAcceptanceMissortAsync(Miss miss)
        {
            var date = miss.Date;
            date = date.AddHours(miss.Time.Hour).AddMinutes(miss.Time.Minute).AddSeconds(miss.Time.Second).AddMilliseconds(miss.Time.Millisecond);

            var query = $"UPDATE [Entt].[Acceptance] SET [IsMissort] = 1, [MissortLocation] = '{miss.LocationId}', [MissortDateTime] = '{date}' WHERE [ConsignmentNo] = '{miss.ConsignmentNo}'";
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Entt"].ConnectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public override string GetEditorViewModel()
        {
            return @"
define([""services/datacontext"", 'services/logger', 'plugins/dialog', objectbuilders.system],
    function(context, logger, dialog, system) {

                bespoke.sph.domain.RtsMissortForEnttAcceptanceUpdate = function(optionOrWebid) {

                    const v = new bespoke.sph.domain.CustomAction(optionOrWebid);
                    v[""$type""] = ""Bespoke.PosEntt.CustomActions.RtsMissortForEnttAcceptanceUpdate, rts.ipos.boems.trigger.actions"";
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

                    if (bespoke.sph.domain.RtsMissortForEnttAcceptanceUpdatePartial)
                    {
                        return _(v).extend(new bespoke.sph.domain.RtsMissortForEnttAcceptanceUpdatePartial(v));
                    }
                    return v;
                };



        const action = ko.observable(new bespoke.sph.domain.RtsMissortForEnttAcceptanceUpdate(system.guid())),
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
                <h3>RTS Missort Update Entt Acceptance Action</h3>
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
