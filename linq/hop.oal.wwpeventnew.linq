<Query Kind="Statements">
  <Reference Relative="..\subscribers\domain.sph.dll">D:\project\work\entt.rts\subscribers\domain.sph.dll</Reference>
  <Reference Relative="..\output\PosEntt.Hop.dll">D:\project\work\entt.rts\output\PosEntt.Hop.dll</Reference>
  <Reference Relative="..\output\PosEntt.Oal.dll">D:\project\work\entt.rts\output\PosEntt.Oal.dll</Reference>
  <Reference Relative="..\output\PosEntt.ReceivePort.RtsHop.dll">D:\project\work\entt.rts\output\PosEntt.ReceivePort.RtsHop.dll</Reference>
  <Reference Relative="..\output\PosEntt.RtsHopToOalWwpEventNewLog.dll">D:\project\work\entt.rts\output\PosEntt.RtsHopToOalWwpEventNewLog.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var port = new Bespoke.PosEntt.ReceivePorts.RtsHop(new Bespoke.Sph.Domain.Logger());
var lines = File.ReadLines(@"D:\office\pos-malaysia\entt\Plan B1\Realtime Scanner\Data\sa_files_20160101-1009\20160102\20160102\20160102232236_0_sa_hop_20160102_233139_gtm14.txt.log");
port.AddHeader("Name", "20160102232236_0_sa_hop_20160102_233139_gtm14.txt.log");
var rawList  = port.Process(lines);
//Console.WriteLine (rawList);
var entities = from i in rawList
			   where null != i
			   let json = i.ToJson()
			   select json.DeserializeFromJson<Bespoke.PosEntt.Hops.Domain.Hop>();
var map = new Bespoke.PosEntt.Integrations.Transforms.RtsHopToOalWwpEventNewLog();
var tasks = from input in entities
		select map.TransformAsync(input);
var list = await Task.WhenAll(tasks);

list.Dump();