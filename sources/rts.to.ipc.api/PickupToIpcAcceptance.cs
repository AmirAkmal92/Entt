using Bespoke.Sph.Domain;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Net.Http;
using Bespoke.PosEntt.Pickups.Domain;
using System;
using System.Text;
using System.Net.Http.Headers;
using rts.to.ipc.api;

namespace RTS.TO.IPC.API
{
    [Export(typeof(CustomAction))]
    [DesignerMetadata(Name = "PickupToIpcAcceptance", TypeName = "RTS.TO.IPC.API.PickupToIpcAcceptance, rts.to.ipc.api", Description = "Pickup sent to IPC Acceptance", FontAwesomeIcon = "envelope")]
    public class PickupToIpcAcceptance : CustomAction
    {
        public override bool UseAsync => true;

        public override async Task ExecuteAsync(RuleContext context)
        {
            var EnttAcc = context.Item as Pickup;
            if (null == EnttAcc) return;
            await Run(EnttAcc);
        }

        public async Task Run(Pickup EnttAcc)
        {
            await SendToIpcApiAsync(EnttAcc);
        }

        private async Task SendToIpcApiAsync(Pickup EnttAcc)
        {
            //var baseurl = @"http://rx-stg.pos.com.my/api/rts/";
            //var baseurl = @"http://192.168.1.100:8080/api/rts/";
            var baseurl = @"http://172.19.0.111/rest/iism_proxy/";

            var client = new HttpClient { BaseAddress = new Uri(baseurl) };
            //var ipcuserkey = ConfigurationManager.GetEnvironmentVariable("ipcuserkey");
            //client.DefaultRequestHeaders.Add("X-User-Key", ipcuserkey);
            //var ipcapiurl = ConfigurationManager.GetEnvironmentVariable("Ipc_ApiUrl");

            var sb = new StringBuilder();


            var fileName = string.Format("sa_deli_{0:yyyyMMdd}_{1:HHmmss}_{2}.txt", EnttAcc.Date, EnttAcc.Time, EnttAcc.ScannerId);
           var data = string.Format("{0}\t{1}\t{2}\t{3}\t{4:ddMMyyyy}\t{5:HHmmss}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}\t{15}\t{16}\t{17}\t{18}\t{19}\t{20}\t{21}\t{22}\t{23}\t{24}\t{25}\t{26}\t{27}\t{28}\t{29}\t{30}\t{31}\t{32}\t{33}\t{34}\t{35}\r\n",
					EnttAcc.ModuleId, EnttAcc.CourierId, EnttAcc.LocationId, EnttAcc.BeatNo, EnttAcc.Date, EnttAcc.Time, EnttAcc.AccountNo, EnttAcc.PickupNo, EnttAcc.ConsignmentNo, EnttAcc.Postcode.ToNullStringDash(), EnttAcc.ParentWeight.ToNullStringDash(), EnttAcc.TotalItem, EnttAcc.ProductType.ToNullStringDash(), EnttAcc.PackageType.ToNullStringDash(), EnttAcc.Country.ToNullStringDash(), EnttAcc.Height.ToNullStringDash(), EnttAcc.Witdh.ToNullStringDash(), EnttAcc.Length.ToNullStringDash(), EnttAcc.ItemCategory.ToNullStringDash(), EnttAcc.BabyConsignment.ToNullStringDash(), EnttAcc.TotalBaby, EnttAcc.TotalParent, EnttAcc.BabyWeigth.ToNullStringDash(), EnttAcc.BabyHeigth.ToNullStringDash(), EnttAcc.BabyWidth.ToNullStringDash(), EnttAcc.BabyLength.ToNullStringDash(), EnttAcc.RoutingCode.ToNullStringDash(), EnttAcc.TotalWeight.ToNullStringDash(), EnttAcc.TotalDimWeight.ToNullStringDash(), EnttAcc.FailPickupReason.ToNullStringDash(), EnttAcc.Comment.ToNullStringDash(), EnttAcc.Price.ToNullStringDash(), EnttAcc.ConsignmentFee.ToNullStringDash(), EnttAcc.DropCode.ToNullStringDash(), EnttAcc.LatePickup.ToNullStringDash(), EnttAcc.PlNine.ToNullStringDash());

            sb.Append(data);
            var request = new StringContent(sb.ToString());
            //request.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
            //request.Headers.Add("X-Name", "sa_deli_0100_20180201_182736_14_1209.txt");
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyIjoiYWRtaW4iLCJyb2xlcyI6WyJhZG1pbmlzdHJhdG9ycyIsImNhbl9lZGl0X2VudGl0eSIsImNhbl9lZGl0X3dvcmtmbG93IiwiZGV2ZWxvcGVycyJdLCJlbWFpbCI6ImFkbWluQHlvdXJjb21wYW55LmNvbSIsInN1YiI6IjYzNjY3ODk4OTcwMDgwOTk0MjZiNTRlOTRlIiwibmJmIjoxNTQ4MjI0OTcwLCJpYXQiOjE1MzIzMjczNzAsImV4cCI6MTU0NjMwMDgwMCwiYXVkIjoiUG9zRW50dCJ9.pBpQ_JPFi3yso6577r9id0PRtivh1FmkdtZxt_T7j7Y");
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            var response = await client.PostAsync("saveAcceptanceFromRTS", request);
            var content = await response.Content.ReadAsStringAsync();
        }


        public override string GetEditorViewModel()
        {
            return @"
define([""services/datacontext"", 'services/logger', 'plugins/dialog', objectbuilders.system],
    function(context, logger, dialog, system) {

                bespoke.sph.domain.PickupToIpcAcceptance = function(optionOrWebid) {

                    const v = new bespoke.sph.domain.CustomAction(optionOrWebid);
                    v[""$type""] = ""RTS.TO.IPC.API.PickupToIpcAcceptance,rts.to.ipc.api"";
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

                    if (bespoke.sph.domain.PickupToIpcAcceptancePartial)
                    {
                        return _(v).extend(new bespoke.sph.domain.PickupToIpcAcceptancePartial(v));
                    }
                    return v;
                };



        const action = ko.observable(new bespoke.sph.domain.PickupToIpcAcceptance(system.guid())),
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
