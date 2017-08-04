using Bespoke.PosEntt.EnttAcceptances.Domain;
using Bespoke.PosEntt.Pickups.Domain;
using Bespoke.Sph.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Bespoke.PosEntt.CustomActions
{
    [Export(typeof(CustomAction))]
    [DesignerMetadata(Name = "RtsPickupWithBabiesForEnttAcceptance", TypeName = "Bespoke.PosEntt.CustomActions.RtsPickupWithBabiesForEnttAcceptance, rts.ipos.boems.trigger.actions", Description = "RTS Pickup Babies EnTT Platform acceptance", FontAwesomeIcon = "database")]
    public class RtsPickupWithBabiesForEnttAcceptance : CustomAction
    {

        public override bool UseAsync => true;

        public override async Task ExecuteAsync(RuleContext context)
        {
            var pickup = context.Item as Pickup;
            if (null == pickup) return;
            if (pickup.TotalBaby <= 0) return;
            await RunAsync(pickup);

        }

        public async Task RunAsync(Pickup pickup)
        {
            //pickup_event_new
            var pickupToEnttAcceptanceMap = new Integrations.Transforms.RtsPickupToEnttPlatformAcceptance();
            var acceptanceList = new List<EnttAcceptance>();
            var parentRow = await pickupToEnttAcceptanceMap.TransformAsync(pickup);
            parentRow.IsParent = true;

            acceptanceList.Add(parentRow);

            if (null != pickup.BabyConsignment)
            {
                var consignmentBabies = pickup.BabyConsignment.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] babiesWeight = { };
                if (!string.IsNullOrEmpty(pickup.BabyWeigth))
                {
                    babiesWeight = pickup.BabyWeigth.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                }
                var babiesHeight = pickup.BabyHeigth.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var babiesWidth = pickup.BabyWidth.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var babiesLength = pickup.BabyLength.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var index = 0;

                foreach (var babyConsignmentNo in consignmentBabies)
                {
                    var consignmentChildRow = parentRow.Clone();
                    consignmentChildRow.Id = Guid.NewGuid().ToString();
                    consignmentChildRow.Parent = parentRow.ConsignmentNo;
                    consignmentChildRow.IsParent = false;
                    consignmentChildRow.ConsignmentNo = babyConsignmentNo;
                    if (babiesWeight.Length == consignmentBabies.Length)
                        consignmentChildRow.Weight = GetDoubleValue(babiesWeight[index]);
                    else
                        consignmentChildRow.Weight = 0;
                    consignmentChildRow.Height = GetDoubleValue(babiesHeight[index]);
                    consignmentChildRow.Length = GetDoubleValue(babiesLength[index]);
                    consignmentChildRow.Width = GetDoubleValue(babiesWidth[index]);

                    acceptanceList.Add(consignmentChildRow);
                    index++;
                }
            }

            //            
            foreach (var item in acceptanceList)
            {
                var context = new SphDataContext();
                using (var session = context.OpenSession())
                {
                    session.Attach(item);
                    await session.SubmitChanges();
                }
            }
            
            
        }

        private decimal? GetDoubleValue(string text)
        {
            decimal value = 0;
            if (decimal.TryParse(text, out value))
                return value;
            return value;
        }

        public override string GetEditorViewModel()
        {
            return @"
define([""services/datacontext"", 'services/logger', 'plugins/dialog', objectbuilders.system],
    function(context, logger, dialog, system) {

                bespoke.sph.domain.RtsPickupWithBabiesForEnttAcceptance = function(optionOrWebid) {

                    const v = new bespoke.sph.domain.CustomAction(optionOrWebid);
                    v[""$type""] = ""Bespoke.PosEntt.CustomActions.RtsPickupWithBabiesForEnttAcceptance, rts.ipos.boems.trigger.actions"";
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

                    if (bespoke.sph.domain.RtsPickupWithBabiesForEnttAcceptancePartial)
                    {
                        return _(v).extend(new bespoke.sph.domain.RtsPickupWithBabiesForEnttAcceptancePartial(v));
                    }
                    return v;
                };



        const action = ko.observable(new bespoke.sph.domain.RtsPickupWithBabiesForEnttAcceptance(system.guid())),
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
                <h3>RTS Pickup Babies Entt Acceptance Action</h3>
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
