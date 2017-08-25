using Bespoke.PosEntt.EnttAcceptances.Domain;
using Bespoke.PosEntt.Stats.Domain;
using Bespoke.Sph.Domain;
using Polly;
using System;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Linq;
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
            var validCodes = new [] { "11", "12", "21", "22", "26", "27", "28", "29", "30", "31", "38", "43", "45", "47", "49", "55", "56" };
            if (!validCodes.Contains(stat.StatusCode)) return;
            await RunAsync(stat);

        }

        public async Task RunAsync(Stat stat)
        {
            var date = stat.Date;
            date = date.AddHours(stat.Time.Hour).AddMinutes(stat.Time.Minute).AddSeconds(stat.Time.Second).AddMilliseconds(stat.Time.Millisecond);
            var item = new EnttAcceptance
            {
                ConsignmentNo = stat.ConsignmentNo,
                IsPupStatCode = true,
                PupStatCodeId = stat.StatusCode,
                PupStatCodeLocation = stat.LocationId,
                PupStatCodeDateTime = date
            };
            var pr = Policy.Handle<SqlException>()
                  .WaitAndRetryAsync(3, c => TimeSpan.FromMilliseconds(c * 200))
                  .ExecuteAndCaptureAsync(async () => await UpdateStatAsync(item));

            pr.Wait();
            if (null != pr.Result.FinalException)
                throw new Exception("Fail updating PUP Stat", pr.Result.FinalException);
        }

        private async Task UpdateStatAsync(EnttAcceptance item)
        {

            var query = $"UPDATE [PosEntt].[EnttAcceptance] SET [IsPupStatCode] = 1, [PupStatCodeId] = '{item.PupStatCodeId}', [PupStatCodeLocation] = '{item.PupStatCodeLocation}', [PupStatCodeDateTime] = '{item.PupStatCodeDateTime}' WHERE [ConsignmentNo] = '{item.ConsignmentNo}'";
            using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
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
