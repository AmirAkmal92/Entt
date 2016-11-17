<Query Kind="Statements">
  <Reference Relative="..\subscribers\domain.sph.dll">D:\project\work\entt.rts\subscribers\domain.sph.dll</Reference>
  <Reference Relative="..\output\PosEntt.Oal.dll">D:\project\work\entt.rts\output\PosEntt.Oal.dll</Reference>
  <Reference Relative="..\output\PosEntt.Pickup.dll">D:\project\work\entt.rts\output\PosEntt.Pickup.dll</Reference>
  <Reference Relative="..\output\PosEntt.ReceivePort.RtsPickup.dll">D:\project\work\entt.rts\output\PosEntt.ReceivePort.RtsPickup.dll</Reference>
  <Reference Relative="..\output\PosEntt.RtsPickupToOalIpsImport.dll">D:\project\work\entt.rts\output\PosEntt.RtsPickupToOalIpsImport.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var port = new Bespoke.PosEntt.ReceivePorts.RtsPickup(new Bespoke.Sph.Domain.Logger());
var lines = File.ReadLines(@"D:\office\pos-malaysia\entt\Plan B1\Realtime Scanner\Data\sa_files_20160101-1009\20161007\20161007194859_0_sa_pick_20161007_194836_jhb30.txt.log");
port.AddHeader("Name", "20161007194859_0_sa_pick_20161007_194836_jhb30.txt.log");
var rawList  = port.Process(lines);
//Console.WriteLine (rawList);
var entities = from i in rawList
			   where null != i
			   let json = i.ToJson()
			   select json.DeserializeFromJson<Bespoke.PosEntt.Pickups.Domain.Pickup>();
var map = new Bespoke.PosEntt.Integrations.Transforms.RtsPickupToOalIpsImport();
var tasks = from input in entities
		select map.TransformAsync(input);
var list = await Task.WhenAll(tasks);

var adapter = new Bespoke.PosEntt.Adapters.Oal.dbo_ips_importAdapter();
foreach (var item in list)
{
	var result = await adapter.InsertAsync(item);
	result.Dump();
}

list.Dump();