<Query Kind="Statements">
  <Reference Relative="..\subscribers\domain.sph.dll">D:\project\work\entt.rts\subscribers\domain.sph.dll</Reference>
  <Reference Relative="..\output\PosEntt.Oal.dll">D:\project\work\entt.rts\output\PosEntt.Oal.dll</Reference>
  <Reference Relative="..\output\PosEntt.Pickup.dll">D:\project\work\entt.rts\output\PosEntt.Pickup.dll</Reference>
  <Reference Relative="..\output\PosEntt.ReceivePort.RtsPickup.dll">D:\project\work\entt.rts\output\PosEntt.ReceivePort.RtsPickup.dll</Reference>
  <Reference Relative="..\output\PosEntt.RtsPickupToOalPickupEventNew.dll">D:\project\work\entt.rts\output\PosEntt.RtsPickupToOalPickupEventNew.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var port = new Bespoke.PosEntt.ReceivePorts.RtsPickup(new Bespoke.Sph.Domain.Logger());
var lines = File.ReadLines(@"D:\office\pos-malaysia\entt\Plan B1\Realtime Scanner\Data\sa_files_20160101-1009\20161007\20161007183020_0_sa_pick_20161007_181534_cs113xxx.txt.log");
port.AddHeader("Name", "20161007183020_0_sa_pick_20161007_181534_cs113.txt.log");
var rawList  = port.Process(lines);
//Console.WriteLine (rawList);

var entities = from i in rawList
			   where null != i
			   let json = i.ToJson()
			   select json.DeserializeFromJson<Bespoke.PosEntt.Pickups.Domain.Pickup>();
var map = new Bespoke.PosEntt.Integrations.Transforms.RtsPickupToOalPickupEventNew();
var tasks = from input in entities
		select map.TransformAsync(input);
var list =( await Task.WhenAll(tasks)).ToList();
//list.Dump();

var pickupWithBabies = from i in entities
			   where i.TotalBaby > 0
			   select i;

foreach (var pickup in pickupWithBabies)
{
	var babies = pickup.BabyConsignment.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries);
	foreach (var babyConsignmentNo in babies)
	{
		var pickup1 = pickup;
		var parent = list.Single (x => x.consignment_no == pickup1.ConsignmentNo);
		parent.WebId = Guid.NewGuid().ToString();
		var baby = parent.Clone();
		baby.consignment_no = babyConsignmentNo;
		baby.item_type_code = (babyConsignmentNo.StartsWith("CG") && babyConsignmentNo.EndsWith("MY") && babyConsignmentNo.Length == 13) ? "02" : "01";
		baby.data_flag = "1";
	
		list.Add(baby);
		
	}
}
list.Where (x => !string.IsNullOrWhiteSpace(x.WebId)).OrderBy (x => x.WebId).Dump();
//var adapter = new Bespoke.PosEntt.Adapters.Oal.dbo_pickup_event_newAdapter();
//foreach (var item in list)
//{
//	var result = await adapter.InsertAsync(item);
//	result.Dump();
//}
//list.Dump();