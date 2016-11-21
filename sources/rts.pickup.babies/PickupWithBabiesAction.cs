using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Bespoke.PosEntt.Pickups.Domain;
using Bespoke.Sph.Domain;
using Polly;

namespace Bespoke.PosEntt.CustomActions
{
    [Export(typeof(CustomAction))]
    [DesignerMetadata(Name = "PickupWithBabies", TypeName = "Bespoke.PosEntt.CustomActions.PickupWithBabiesAction, rts.pickup.babies", Description = "RTS Pickup with babies", FontAwesomeIcon = "calendar-check-o")]
    public class PickupWithBabiesAction : CustomAction
    {
        public override bool UseAsync => true;
        public override async Task ExecuteAsync(RuleContext context)
        {
            var pickup = context.Item as Pickup;
            if (null == pickup) return;

            await RunAsync(pickup);
        }

        public async Task RunAsync(Pickup pickup)
        {
            //pickup_event_new
            var eventNewMap = new Integrations.Transforms.RtsPickupToOalPickupEventNew();
            var pickupEventNewRows = new List<Adapters.Oal.dbo_pickup_event_new>();
            var parentRow = await eventNewMap.TransformAsync(pickup);
            pickupEventNewRows.Add(parentRow);

            if (null != pickup.BabyConsignment)
            {
                var babies = pickup.BabyConsignment.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var babyConsignmentNo in babies)
                {
                    var childRow = parentRow.Clone();
                    var ticks = System.DateTime.Now.Ticks;
                    var id = string.Format("en{0}{1}", ticks.ToString().Substring(6), System.Guid.NewGuid().ToString("N"));
                    childRow.id = id.Substring(0, 34);
                    childRow.consignment_no = babyConsignmentNo;
                    childRow.item_type_code = (babyConsignmentNo.StartsWith("CG") && babyConsignmentNo.EndsWith("MY") &&
                                               babyConsignmentNo.Length == 13)
                        ? "02"
                        : "01";
                    childRow.data_flag = "1";
                    childRow.parent_no = parentRow.consignment_no;
                    pickupEventNewRows.Add(childRow);
                }
            }

            var eventNewAdapter = new Adapters.Oal.dbo_pickup_event_newAdapter();
            foreach (var item in pickupEventNewRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => eventNewAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }

            //consigment_initial
            var consignmentInitialMap = new Integrations.Transforms.RtsPickupToOalConsigmentInitial();
            var consignmentInitialRows = new List<Adapters.Oal.dbo_consignment_initial>();
            var consignmentParentRow = await consignmentInitialMap.TransformAsync(pickup);
            consignmentInitialRows.Add(consignmentParentRow);

            if (null != pickup.BabyConsignment)
            {
                var consignmentBabies = pickup.BabyConsignment.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var babyConsignmentNo in consignmentBabies)
                {
                    var consignmentChildRow = consignmentParentRow.Clone();
                    var ticks = System.DateTime.Now.Ticks;
                    var id = string.Format("en{0}{1}", ticks.ToString().Substring(6), System.Guid.NewGuid().ToString("N"));
                    consignmentChildRow.id = id.Substring(0, 20);
                    consignmentChildRow.baby_item = consignmentParentRow.id;
                    consignmentChildRow.parent = consignmentParentRow.id;
                    consignmentChildRow.is_parent = 0;
                    consignmentChildRow.number = babyConsignmentNo;
                    consignmentInitialRows.Add(consignmentChildRow);

                }
            }

            var consignmentAdapter = new Adapters.Oal.dbo_consignment_initialAdapter();
            foreach (var item in consignmentInitialRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => consignmentAdapter.InsertAsync(item));
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

                bespoke.sph.domain.PickupWithBabiesAction = function(optionOrWebid) {

                    const v = new bespoke.sph.domain.CustomAction(optionOrWebid);
                    v[""$type""] = ""Bespoke.PosEntt.CustomActions.PickupWithBabiesAction, rts.pickup.babies"";
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

                    if (bespoke.sph.domain.PickupWithBabiesActionPartial)
                    {
                        return _(v).extend(new bespoke.sph.domain.PickupWithBabiesActionPartial(v));
                    }
                    return v;
                };



        const action = ko.observable(new bespoke.sph.domain.PickupWithBabiesAction(system.guid())),
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
                <h3>RTS pickup with babies</h3>
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