<Query Kind="Statements">
  <Reference Relative="..\subscribers\domain.sph.dll">D:\project\work\entt.rts\subscribers\domain.sph.dll</Reference>
  <Reference Relative="..\output\PosEntt.Oal.dll">D:\project\work\entt.rts\output\PosEntt.Oal.dll</Reference>
  <Reference Relative="..\output\PosEntt.Pickup.dll">D:\project\work\entt.rts\output\PosEntt.Pickup.dll</Reference>
  <Reference Relative="..\output\PosEntt.ReceivePort.RtsPickup.dll">D:\project\work\entt.rts\output\PosEntt.ReceivePort.RtsPickup.dll</Reference>
  <Reference Relative="..\output\PosEntt.RtsPickupToOalConsigmentInitial.dll">D:\project\work\entt.rts\output\PosEntt.RtsPickupToOalConsigmentInitial.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var port = new Bespoke.PosEntt.ReceivePorts.RtsPickup(new Bespoke.Sph.Domain.Logger());
var lines = File.ReadLines(@"D:\office\pos-malaysia\entt\Plan B1\Realtime Scanner\Data\sa_files_20160101-1009\20161007\20161007175053_0_sa_pick_20161007_175004_kd305.txt.log");
port.AddHeader("Name", "20161007193818_0_sa_pick_20161007_194730_KCH30.txt.log");
var rawList  = port.Process(lines);
//Console.WriteLine (rawList);
var entities = from i in rawList
			   where null != i
			   let json = i.ToJson()
			   select json.DeserializeFromJson<Bespoke.PosEntt.Pickups.Domain.Pickup>();
var map = new Bespoke.PosEntt.Integrations.Transforms.RtsPickupToOalConsigmentInitial();
var tasks = from input in entities
		select map.TransformAsync(input);
var list = await Task.WhenAll(tasks);
//Console.WriteLine (list);
var adapter = new Bespoke.PosEntt.Adapters.Oal.dbo_consignment_initialAdapter();
foreach (var item in list)
{
	var result = await adapter.InsertAsync(item);
	result.Dump();
}
list.Dump();