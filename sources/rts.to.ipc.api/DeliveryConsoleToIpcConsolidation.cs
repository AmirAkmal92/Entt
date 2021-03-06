﻿using Bespoke.Sph.Domain;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Net.Http;
using Bespoke.PosEntt.Decos.Domain;
using System;
using System.Text;
using System.Net.Http.Headers;
using rts.to.ipc.api;

namespace RTS.TO.IPC.API
{
    [Export(typeof(CustomAction))]
    [DesignerMetadata(Name = "DeliveryConsoleToIpcConsolidation", TypeName = "RTS.TO.IPC.API.DeliveryConsoleToIpcConsolidation,rts.to.ipc.api", Description = "Delivery Console to IPC Consolidation", FontAwesomeIcon = "envelope")]
    public class DeliveryConsoleToIpcConsolidation : CustomAction
    {
        public override bool UseAsync => true;

        public override async Task ExecuteAsync(RuleContext context)
        {
            var deco = context.Item as Deco;
            if (null == deco) return; 
            await Run(deco);
        }

        public async Task Run(Deco deco)
        {
            await SendToIpcApiAsync(deco);
        }

        private async Task SendToIpcApiAsync(Deco deco)
        {
            //var baseurl = @"http://rx-stg.pos.com.my/api/rts/";
            //var baseurl = @"http://192.168.1.100:8080/api/rts/";
            var baseurl = @"http://172.19.0.111/rest/iism_proxy/";

            var client = new HttpClient { BaseAddress = new Uri(baseurl) };
            //var ipcuserkey = ConfigurationManager.GetEnvironmentVariable("ipcuserkey");
            //client.DefaultRequestHeaders.Add("X-User-Key", ipcuserkey);
            //var ipcapiurl = ConfigurationManager.GetEnvironmentVariable("Ipc_ApiUrl");

            var sb = new StringBuilder();


            var fileName = string.Format("sa_deli_{0:yyyyMMdd}_{1:HHmmss}_{2}.txt", deco.Date, deco.Time, deco.ScannerId);
            var data = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\r\n",
                    deco.CourierId, deco.LocationId, deco.BeatNo, deco.Date.ToString("ddMMyyyy"), deco.Time.ToString("HHmmss"), deco.ConsoleTag, deco.Comment.ToNullStringDash(), deco.AllConsignmentnNotes, deco.TotalConsignment);

            sb.Append(data);
            var request = new StringContent(sb.ToString());
            //request.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
            //request.Headers.Add("X-Name", "sa_deli_0100_20180201_182736_14_1209.txt");
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyIjoiYWRtaW4iLCJyb2xlcyI6WyJhZG1pbmlzdHJhdG9ycyIsImNhbl9lZGl0X2VudGl0eSIsImNhbl9lZGl0X3dvcmtmbG93IiwiZGV2ZWxvcGVycyJdLCJlbWFpbCI6ImFkbWluQHlvdXJjb21wYW55LmNvbSIsInN1YiI6IjYzNjY3ODk4OTcwMDgwOTk0MjZiNTRlOTRlIiwibmJmIjoxNTQ4MjI0OTcwLCJpYXQiOjE1MzIzMjczNzAsImV4cCI6MTU0NjMwMDgwMCwiYXVkIjoiUG9zRW50dCJ9.pBpQ_JPFi3yso6577r9id0PRtivh1FmkdtZxt_T7j7Y");
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            var response = await client.PostAsync("saveDeliveryConsoleFromRTS", request);
            var content = await response.Content.ReadAsStringAsync();
        }


        public override string GetEditorViewModel()
        {
            return @"
define([""services/datacontext"", 'services/logger', 'plugins/dialog', objectbuilders.system],
    function(context, logger, dialog, system) {

                bespoke.sph.domain.DeliveryConsoleToIpcConsolidation = function(optionOrWebid) {

                    const v = new bespoke.sph.domain.CustomAction(optionOrWebid);
                    v[""$type""] = ""RTS.TO.IPC.API.DeliveryConsoleToIpcConsolidation,rts.to.ipc.api"";
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

                    if (bespoke.sph.domain.DeliveryConsoleToIpcConsolidationPartial)
                    {
                        return _(v).extend(new bespoke.sph.domain.DeliveryConsoleToIpcConsolidationPartial(v));
                    }
                    return v;
                };



        const action = ko.observable(new bespoke.sph.domain.DeliveryConsoleToIpcConsolidation(system.guid())),
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
