using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Entt.Tracker.Service;

namespace Bespoke.PosEntt.CustomActions
{
    public class EventWithChildrenAction : CustomAction
    {
        public override bool UseAsync => true;

        protected async Task<int> TrackEvents<T>(Func<T, Task> operation, T item) where T : DomainObject
        {
            var tracker = new EnttTracker();
            var done = await tracker.GetStatusAsync(item.HashEvent(), item.GetConsignmentNo(), item.GetDateTime(), typeof(T).Name).ConfigureAwait(false);
            if (done) return 1;

            await operation(item);
            tracker.AddStatusAsync(item.HashEvent(), item.GetConsignmentNo(), item.GetDateTime(), typeof(T).Name)
                .ContinueWith(_ =>
                {
                    System.Diagnostics.Debug.Assert(_.Exception == null);
                })
                .ConfigureAwait(false);
            return 1;
        }

        protected Task<string> GetItemConsigmentsFromConsoleDetailsAsync(string consignmentNo)
        {
            var consoleDetailsAdapter = new Adapters.Oal.dbo_console_detailsAdapter();
            var query = $"SELECT [item_consignments] FROM [dbo].[console_details] WHERE console_no = '{consignmentNo}'";
            return consoleDetailsAdapter.ExecuteScalarAsync<string>(query);
        }

        protected async Task<Adapters.Oal.dbo_ips_import> CreateStatIpsImport(Adapters.Oal.dbo_status_code_event_new stat, string consignmentNo)
        {
            //recreate stat item for mapping ipsimport
            var statusItem = new Stats.Domain.Stat
            {
                Date = stat.date_field.Value.Date,
                Time = stat.date_field.Value,
                LocationId = stat.office_no,
                BeatNo = stat.beat_no,
                CourierId = stat.courier_id,
                StatusCode = stat.status_code_id,
                ConsignmentNo = consignmentNo,
                Comment = stat.event_comment
            };
            var map = new Integrations.Transforms.RtsStatToOalIpsImport();
            var ipsImport = await map.TransformAsync(statusItem);

            ipsImport.dt_created_oal_date_field = stat.date_created_oal_date_field;

            return ipsImport;
        }

        protected async Task<Adapters.Oal.dbo_consignment_initial> SearchConsignmentInitialAsync(string consignmentNo)
        {
            var adapter = new Adapters.Oal.dbo_consignment_initialAdapter();
            var query = $"SELECT [weight_double], [item_category], [shipper_address_country] FROM [dbo].[consignment_initial] WHERE [number] = '{consignmentNo}'";

            Adapters.Oal.dbo_consignment_initial consignment = null;

            using (var conn = new System.Data.SqlClient.SqlConnection(adapter.ConnectionString))
            using (var cmd = new System.Data.SqlClient.SqlCommand(query, conn))
            {
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        consignment = new Adapters.Oal.dbo_consignment_initial
                        {
                            weight_double = reader["weight_double"].ReadNullable<double>(),
                            item_category = reader["item_category"].ReadNullableString(),
                            shipper_address_country = reader["shipper_address_country"].ReadNullableString()
                        };

                    }
                }

            }

            return consignment;
        }

        protected string GetClassCode(string consignmentNo)
        {
            string code = null;
            var isConsole = consignmentNo.StartsWith("CG") && consignmentNo.EndsWith("MY");
            if (!isConsole)
            {
                var firstLetter = consignmentNo.Substring(0, 1);
                switch (firstLetter)
                {
                    case "E": code = "E"; break;
                    case "C": code = "C"; break;
                    case "L": code = "U"; break;
                }
            }

            return code;
        }

        protected string GetNonDeliveryReason(string reasonCode, string damageCode)
        {
            string display = null;
            switch (reasonCode)
            {
                case "02": display = "12"; break;
                case "03": display = "10"; break;
                case "04": if (damageCode == "01") display = null; else display = "18"; break;
                case "05": display = "13"; break;
                case "06": display = "16"; break;
                case "07": display = "16"; break;
                case "08": display = "13"; break;
                case "09": display = "12"; break;
            }
            return display;
        }

        protected string GetNonDeliveryMeasure(string reasonCode, string damageCode)
        {
            string display = null;
            switch (reasonCode)
            {
                case "02": display = "C"; break;
                case "03": display = "M"; break;
                case "04": if (damageCode == "01") display = null; else display = "C"; break;
                case "05": display = "E"; break;
                case "06": display = "B"; break;
                case "07": display = "B"; break;
                case "08": display = "E"; break;
                case "09": display = "C"; break;
            }
            return display;
        }

        protected string GetDeliveryTransactionCode(string reasonCode, string damageCode)
        {
            string code = null;
            switch (reasonCode)
            {
                case "01": return "TN037";
                case "02": return "TN036";
                case "03": return "TN036";
                case "04": return damageCode.Equals("01") ? "TN037" : "TN036";
                case "05": return "TN036";
                case "06": return "TN036";
                case "07": return "TN036";
                case "08": return "TN036";
                case "09": return "TN036";
                case "10": return "TN037";
                case "11": return "TN037";
            }

            return code;
        }

        public override string GetEditorViewModel()
        {
            return @"
define([""services/datacontext"", 'services/logger', 'plugins/dialog', objectbuilders.system],
    function(context, logger, dialog, system) {

                bespoke.sph.domain.DeliWithChildrenAction = function(optionOrWebid) {

                    const v = new bespoke.sph.domain.CustomAction(optionOrWebid);
                    v[""$type""] = ""Bespoke.PosEntt.CustomActions.DeliWithChildrenAction, rts.pickup.babies"";
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

        protected static string GenerateId(int length)
        {
            var id = $"en{Guid.NewGuid():N}";
            if (length == 20)
                return GenerateShortId("en");
            else
                return id.Substring(0, length);
        }

        private static string GenerateShortId(string prefix)
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0}{1:x}", prefix, i - DateTime.Now.Ticks);
        }

        protected static bool IsConsole(string connoteNo)
        {
            const string PATTERN = "CG[0-9]{9}MY";
            var match = System.Text.RegularExpressions.Regex.Match(connoteNo, PATTERN);
            return match.Success;
        }

        protected static bool IsIpsImportItem(string connoteNo)
        {
            if (!string.IsNullOrEmpty(connoteNo))
            {
                if (!connoteNo.EndsWith("MY") && connoteNo.Length == 13)
                    return true;
            }
            return false;
        }
    }
}