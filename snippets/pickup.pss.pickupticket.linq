<Query Kind="Statements">
  <Reference Relative="..\subscribers\domain.sph.dll">F:\project\work\entt.rts\subscribers\domain.sph.dll</Reference>
  <Reference Relative="..\output\PosEntt.Oal.dll">F:\project\work\entt.rts\output\PosEntt.Oal.dll</Reference>
  <Reference Relative="..\output\PosEntt.Pickup.dll">F:\project\work\entt.rts\output\PosEntt.Pickup.dll</Reference>
  <Reference Relative="..\output\PosEntt.Pss.dll">F:\project\work\entt.rts\output\PosEntt.Pss.dll</Reference>
  <Reference Relative="..\output\PosEntt.ReceivePort.RtsPickup.dll">F:\project\work\entt.rts\output\PosEntt.ReceivePort.RtsPickup.dll</Reference>
  <Reference Relative="..\output\PosEntt.RtsPickupToPssUpdateStatus.dll">F:\project\work\entt.rts\output\PosEntt.RtsPickupToPssUpdateStatus.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var port = new Bespoke.PosEntt.ReceivePorts.RtsPickup(new Bespoke.Sph.Domain.Logger());
var lines = File.ReadLines(@"D:\office\pos-malaysia\entt\Plan B1\Realtime Scanner\Data\sa_files_20160101-1009\20160102\20160102\20160102160334_0_sa_pick_20160102_160544_bb297.txt.log");
port.AddHeader("Name", "20161009082318_0_sa_vasn_20161009_081832_ck296.txt.log");
var rawList  = port.Process(lines);
//Console.WriteLine (rawList);
var entities = from i in rawList
			   where null != i
			   let json = i.ToJson()
			   select json.DeserializeFromJson<Bespoke.PosEntt.Pickups.Domain.Pickup>();
var map = new Bespoke.PosEntt.Integrations.Transforms.RtsPickupToPssUpdateStatus();
var tasks = from input in entities
		select map.TransformAsync(input);
var list = await Task.WhenAll(tasks);

var adapter = new Bespoke.PosEntt.Adapters.Pss.Pss();
foreach (var request in list)
{
	Console.WriteLine($"Updating pickup status for {request.PickupNumber}");
	var rt = await adapter.UpdatePickupStatusAsync(request);
	Console.WriteLine (rt);
}

//list.Dump();