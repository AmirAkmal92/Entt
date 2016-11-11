<Query Kind="Statements">
  <Reference Relative="..\subscribers\domain.sph.dll">D:\project\work\entt.rts\subscribers\domain.sph.dll</Reference>
  <Reference Relative="..\output\PosEntt.Deco.dll">D:\project\work\entt.rts\output\PosEntt.Deco.dll</Reference>
  <Reference Relative="..\output\PosEntt.Oal.dll">D:\project\work\entt.rts\output\PosEntt.Oal.dll</Reference>
  <Reference Relative="..\output\PosEntt.ReceivePort.RtsDeco.dll">D:\project\work\entt.rts\output\PosEntt.ReceivePort.RtsDeco.dll</Reference>
  <Reference Relative="..\output\PosEntt.RtsDeco_Oal_delivery_console_event_new.dll">D:\project\work\entt.rts\output\PosEntt.RtsDeco_Oal_delivery_console_event_new.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var port = new Bespoke.PosEntt.ReceivePorts.RtsDeco(new Bespoke.Sph.Domain.Logger());
var lines = File.ReadLines(@"D:\office\pos-malaysia\entt\Plan B1\Realtime Scanner\Data\sa_files_20160101-1009\20160102\20160102\20160104114450_0_sa_deco_20160102_123616_sbn77.txt.log");
port.AddHeader("Name", "20160104114450_0_sa_deco_20160102_123616_sbn77.txt.log");
var rawList  = port.Process(lines);
//Console.WriteLine (rawList);
var entities = from i in rawList
			   where null != i
			   let json = i.ToJson()
			   select json.DeserializeFromJson<Bespoke.PosEntt.Decos.Domain.Deco>();
var map = new Bespoke.PosEntt.Integrations.Transforms.RtsDecoOalDeliveryConsoleEventNew();
var tasks = from input in entities
		select map.TransformAsync(input);
var list = await Task.WhenAll(tasks);
list.Dump();
//var req = list.First();
//var adapter = new Bespoke.PosEntt.Adapters.Oal.dbo_delivery_console_event_newAdapter();
//adapter.ConnectionString = "Data Source=10.1.1.120;Initial Catalog=oal_dbo;Application Name=posentt;User Id=enttadm;password=P@ssw0rd";
//var result = await adapter.InsertAsync(req);

//result.Dump();