using Bespoke.PosEntt.EnttAcceptances.Domain;
using Bespoke.PosEntt.Unknowns.Domain;
using Bespoke.Sph.Domain;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Bespoke.PosEntt.CustomActions
{
    [Export(typeof(CustomAction))]
    [DesignerMetadata(Name = "RtsUnknownForEnttAcceptanceUpdate", TypeName = "Bespoke.PosEntt.CustomActions.RtsUnknownForEnttAcceptanceUpdate, rts.ipos.boems.trigger.actions", Description = "RTS Missort update Entt Platform acceptance", FontAwesomeIcon = "database")]
    public class RtsUnknownForEnttAcceptanceUpdate : CustomAction
    {
        public override bool UseAsync => true;

        public override async Task ExecuteAsync(RuleContext context)
        {
            var acceptance = context.Item as EnttAcceptance;
            if (null == acceptance) return;
            await RunAsync(acceptance);
        }

        public async Task RunAsync(EnttAcceptance acceptance)
        {
            var unknown = await GetEnttUnknownAsync(acceptance.ConsignmentNo);
            if (null == unknown) return;

            unknown.Status = "0";
            unknown.PupDateTime = acceptance.DateTime;

            await UpdateEnttUnknownAsync(unknown);
        }

        private async Task UpdateEnttUnknownAsync(Unknown unknown)
        {
            var json = unknown.ToJsonString();
            var query = $"UPDATE [PosEntt].[Unknown] SET [Status] = '{unknown.Status}', [PupDateTime] = '{unknown.PupDateTime}', [Json] = '{json}' WHERE [Id] = '{unknown.Id}'";
            using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        protected async Task<Unknown> GetEnttUnknownAsync(string consignmentNo)
        {
            Unknown unknown = null;
            var query = $"SELECT TOP 1 [Json] FROM [PosEntt].[Unknown]  WHERE [ConsignmentNo] = '{consignmentNo}' ORDER BY [CreatedDate] DESC";
            using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var json = reader["Json"].ReadNullableString();
                        unknown = JsonSerializerService.DeserializeFromJson<Unknown>(json);
                    }
                }
            }
            return unknown;
        }

        public override string GetEditorViewModel()
        {
            return @"
define([""services/datacontext"", 'services/logger', 'plugins/dialog', objectbuilders.system],
    function(context, logger, dialog, system) {

                bespoke.sph.domain.RtsUnknownForEnttAcceptanceUpdate = function(optionOrWebid) {

                    const v = new bespoke.sph.domain.CustomAction(optionOrWebid);
                    v[""$type""] = ""Bespoke.PosEntt.CustomActions.RtsUnknownForEnttAcceptanceUpdate, rts.ipos.boems.trigger.actions"";
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

                    if (bespoke.sph.domain.RtsUnknownForEnttAcceptanceUpdatePartial)
                    {
                        return _(v).extend(new bespoke.sph.domain.RtsUnknownForEnttAcceptanceUpdatePartial(v));
                    }
                    return v;
                };



        const action = ko.observable(new bespoke.sph.domain.RtsUnknownForEnttAcceptanceUpdate(system.guid())),
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
                <h3>RTS Unknown Update Entt Acceptance Action</h3>
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
