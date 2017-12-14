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
    public class PickupWithBabiesAction : EventWithChildrenAction
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
                    childRow.id = GenerateId(34);
                    childRow.date_created_oal_date_field = DateTime.Now;
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
            var consignmentInitialRows = new List<Adapters.Oal.UspConsigmentInitialRtsRequest>();
            var consignmentParentRow = await consignmentInitialMap.TransformAsync(pickup);
            consignmentInitialRows.Add(consignmentParentRow);



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
                    var consignmentChildRow = consignmentParentRow.Clone();
                    consignmentChildRow.id = GenerateId(20);
                    consignmentChildRow.dt_created_date_field = DateTime.Now;
                    consignmentChildRow.baby_item = consignmentParentRow.id;
                    consignmentChildRow.parent = consignmentParentRow.id;
                    consignmentChildRow.is_parent = 0;
                    consignmentChildRow.number = babyConsignmentNo;
                    if (babiesWeight.Length == consignmentBabies.Length)
                        consignmentChildRow.weight_double = GetDoubleValue(babiesWeight[index]);
                    else
                        consignmentChildRow.weight_double = 0;
                    consignmentChildRow.height_double = GetDoubleValue(babiesHeight[index]);
                    consignmentChildRow.length_double = GetDoubleValue(babiesLength[index]);
                    consignmentChildRow.width_double = GetDoubleValue(babiesWidth[index]);

                    consignmentInitialRows.Add(consignmentChildRow);
                    index++;
                }
            }

            var oal = new Adapters.Oal.Oal();
            foreach (var item in consignmentInitialRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => oal.UspConsigmentInitialRtsAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue

            }

            //wwp_event_new
            var wwpEventNewLogMap = new Integrations.Transforms.RtsPickupOalWwpEventNewLog();
            var wwpEventNewLogRows = new List<Adapters.Oal.dbo_wwp_event_new_log>();
            var wwpEventNewLogParentRow = await wwpEventNewLogMap.TransformAsync(pickup);
            wwpEventNewLogRows.Add(wwpEventNewLogParentRow);

            if (null != pickup.BabyConsignment)
            {
                var wwpLogBabies = pickup.BabyConsignment.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var babyConsignmentNo in wwpLogBabies)
                {
                    var wwpLogChildRow = wwpEventNewLogParentRow.Clone();
                    wwpLogChildRow.id = GenerateId(34);
                    wwpLogChildRow.dt_created_oal_date_field = DateTime.Now;
                    wwpLogChildRow.consignment_note_number = babyConsignmentNo;
                    wwpEventNewLogRows.Add(wwpLogChildRow);
                }
            }

            var wwpLogAdapter = new Adapters.Oal.dbo_wwp_event_new_logAdapter();
            foreach (var item in wwpEventNewLogRows)
            {
                var pr = Policy.Handle<SqlException>()
                    .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(500 * Math.Pow(2, x)))
                    .ExecuteAndCaptureAsync(() => wwpLogAdapter.InsertAsync(item));
                var result = await pr;
                if (result.FinalException != null)
                    throw result.FinalException; // send to dead letter queue
                System.Diagnostics.Debug.Assert(result.Result > 0, "Should be at least 1 row");
            }
        }

        private double? GetDoubleValue(string text)
        {
            double value = 0;
            if (double.TryParse(text, out value))
                return value;
            return value;
        }
    }
}