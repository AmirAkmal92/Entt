﻿using Bespoke.PosEntt.IposBoems.Domain;
using Bespoke.Sph.Domain;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Bespoke.PosEntt.CustomActions
{
    [Export(typeof(CustomAction))]
    [DesignerMetadata(Name = "IposBoemAction", TypeName = "Bespoke.PosEntt.CustomActions.IposBoemAction, rts.ipos.boems.trigger.actions", Description = "RTS IPOS BOEM action", FontAwesomeIcon = "database")]
    public class IposBoemAction : CustomAction
    {
        public override bool UseAsync => true;
        public override async Task ExecuteAsync(RuleContext context)
        {
            var item = context.Item as IposBoem;
            if (null == item) return;

            await RunAsync(item);

        }

        public async Task RunAsync(IposBoem boem)
        {
            var consignmentInitial = await SearchConsignmentInitialAsync(boem.ConsignmentNo);
            if (null == consignmentInitial)
            {
                //insert consignment initial

                //var initialMap = new Integrations.Transforms.RtsDeliveryToOalDboDeliveryEventNew();
            }

            var consignmentUpdate = await SearchConsignmentUpdateAsync(boem.ConsignmentNo);
            if (null == consignmentUpdate)
            {
                //insert consignment update
            }
            else
            {
                //update consignment update
            }

        }

        protected async Task<Adapters.Oal.dbo_consignment_initial> SearchConsignmentInitialAsync(string consignmentNo)
        {
            var adapter = new Adapters.Oal.dbo_consignment_initialAdapter();
            var query = $"SELECT [dt_created_date_field] FROM [dbo].[consignment_initial] WHERE [number] = '{consignmentNo}'";

            Adapters.Oal.dbo_consignment_initial consignmentInitial = null;

            using (var conn = new System.Data.SqlClient.SqlConnection(adapter.ConnectionString))
            using (var cmd = new System.Data.SqlClient.SqlCommand(query, conn))
            {
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        consignmentInitial = new Adapters.Oal.dbo_consignment_initial
                        {
                            dt_created_date_field = reader["dt_created_date_field"].ReadNullable<DateTime>()
                        };

                    }
                }

            }

            return consignmentInitial;
        }

        protected async Task<Adapters.Oal.dbo_consignment_update> SearchConsignmentUpdateAsync(string consignmentNo)
        {
            var adapter = new Adapters.Oal.dbo_consignment_initialAdapter();
            var query = $"SELECT [weight_double] FROM [dbo].[consignment_update] WHERE [number] = '{consignmentNo}'";

            Adapters.Oal.dbo_consignment_update consignmentUpdate = null;

            using (var conn = new System.Data.SqlClient.SqlConnection(adapter.ConnectionString))
            using (var cmd = new System.Data.SqlClient.SqlCommand(query, conn))
            {
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        consignmentUpdate = new Adapters.Oal.dbo_consignment_update
                        {
                            weight_double = reader["weight_double"].ReadNullable<double>()
                        };

                    }
                }

            }

            return consignmentUpdate;
        }

        public override string GetEditorViewModel()
        {
            return @"
define([""services/datacontext"", 'services/logger', 'plugins/dialog', objectbuilders.system],
    function(context, logger, dialog, system) {

                bespoke.sph.domain.IposBoemAction = function(optionOrWebid) {

                    const v = new bespoke.sph.domain.CustomAction(optionOrWebid);
                    v[""$type""] = ""Bespoke.PosEntt.CustomActions.IposBoemAction, rts.ipos.boems.trigger.actions"";
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

                    if (bespoke.sph.domain.IposBoemActionPartial)
                    {
                        return _(v).extend(new bespoke.sph.domain.IposBoemActionPartial(v));
                    }
                    return v;
                };



        const action = ko.observable(new bespoke.sph.domain.IposBoemAction(system.guid())),
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
                <h3>IPOS BOEM Acceptance Action</h3>
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
