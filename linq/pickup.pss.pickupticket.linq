<Query Kind="Statements">
  <Reference Relative="..\subscribers\domain.sph.dll">D:\project\work\entt.rts\subscribers\domain.sph.dll</Reference>
  <Reference Relative="..\output\PosEntt.Oal.dll">D:\project\work\entt.rts\output\PosEntt.Oal.dll</Reference>
  <Reference Relative="..\output\PosEntt.Pickup.dll">D:\project\work\entt.rts\output\PosEntt.Pickup.dll</Reference>
  <Reference Relative="..\output\PosEntt.Pss.dll">D:\project\work\entt.rts\output\PosEntt.Pss.dll</Reference>
  <Reference Relative="..\output\PosEntt.ReceivePort.RtsPickup.dll">D:\project\work\entt.rts\output\PosEntt.ReceivePort.RtsPickup.dll</Reference>
  <Reference Relative="..\output\PosEntt.RtsPickToPssPickupTicket.dll">D:\project\work\entt.rts\output\PosEntt.RtsPickToPssPickupTicket.dll</Reference>
  <Reference Relative="..\output\PosEntt.RtsPickup_oal_wwp_event_new_log.dll">D:\project\work\entt.rts\output\PosEntt.RtsPickup_oal_wwp_event_new_log.dll</Reference>
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
var map = new Bespoke.PosEntt.Integrations.Transforms.RtsPickupOalWwpEventNewLog();
var tasks = from input in entities
		select map.TransformAsync(input);
var list = await Task.WhenAll(tasks);

var adapter = new Bespoke.PosEntt.Adapters.Pss.DboPickupTicketAdapter();
var pickup = await adapter.LoadAsync("SELECT * FROM [dbo].[PickupTicket] WHERE PickupNumber = 'BA13057'");
foreach (var item in pickup.ItemCollection)
{
	item.PickupTicketStatus = "07";
	item.PickupUpdateStatusDate = DateTime.Now;	
	Console.WriteLine (item);
	var rows = await adapter.UpdateAsync(item);
	Console.WriteLine (rows);
}

//list.Dump();