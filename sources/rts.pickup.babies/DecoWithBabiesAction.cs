﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Polly;

namespace Bespoke.PosEntt.CustomActions
{
    [Export(typeof(CustomAction))]
    [DesignerMetadata(Name = "DecoWithBabies", TypeName = "Bespoke.PosEntt.CustomActions.DecoWithBabiesAction, rts.pickup.babies", Description = "RTS Deco with child items", FontAwesomeIcon = "calendar-check-o")]
    public class DecoWithBabiesAction : CustomAction
    {
        public override bool UseAsync => true;
        public override async Task ExecuteAsync(RuleContext context)
        {
            var deco = context.Item as Decos.Domain.Deco;
            if (null == deco) return;
            if (deco.TotalConsignment <= 0) return;
            await RunAsync(deco);
        }

        public async Task RunAsync(Decos.Domain.Deco deco)
        {
            //deco console_details
            var decoEventMap = new Integrations.Transforms.RtsDecoOalDeliveryConsoleEventNew();
            var decoEventRows = new List<Adapters.Oal.dbo_delivery_console_event_new>();
            var parentRow = await decoEventMap.TransformAsync(deco);
            decoEventRows.Add(parentRow);

            var itemList = deco.AllConsignmentnNotes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in itemList)
            {
                var childItem = parentRow.Clone();
                childItem.id = GenerateId(34);
                childItem.consignment_no = item;
                childItem.item_type_code = IsConsole(item) ? "02" : "01";
                childItem.data_flag = "1";
                decoEventRows.Add(childItem);
            }

            var decoEventAdapter = new Adapters.Oal.dbo_delivery_console_event_newAdapter();
            foreach (var item in decoEventRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => decoEventAdapter.InsertAsync(item));
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

                bespoke.sph.domain.DecoWithBabiesAction = function(optionOrWebid) {

                    const v = new bespoke.sph.domain.CustomAction(optionOrWebid);
                    v[""$type""] = ""Bespoke.PosEntt.CustomActions.DecoWithBabiesAction, rts.pickup.babies"";
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

                    if (bespoke.sph.domain.DecoWithBabiesActionPartial)
                    {
                        return _(v).extend(new bespoke.sph.domain.DecoWithBabiesActionPartial(v));
                    }
                    return v;
                };



        const action = ko.observable(new bespoke.sph.domain.DecoWithBabiesAction(system.guid())),
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
                <h3>RTS deco with child items</h3>
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