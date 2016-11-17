<Query Kind="Statements">
  <Reference Relative="..\subscribers\domain.sph.dll">D:\project\work\entt.rts\subscribers\domain.sph.dll</Reference>
  <Reference Relative="..\output\PosEntt.Oal.dll">D:\project\work\entt.rts\output\PosEntt.Oal.dll</Reference>
  <Reference Relative="..\output\PosEntt.ReceivePort.RtsStat.dll">D:\project\work\entt.rts\output\PosEntt.ReceivePort.RtsStat.dll</Reference>
  <Reference Relative="..\output\PosEntt.RtsStatToOal_ips_import.dll">D:\project\work\entt.rts\output\PosEntt.RtsStatToOal_ips_import.dll</Reference>
  <Reference Relative="..\output\PosEntt.Stat.dll">D:\project\work\entt.rts\output\PosEntt.Stat.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var port = new Bespoke.PosEntt.ReceivePorts.RtsStat(new Bespoke.Sph.Domain.Logger());
var lines = File.ReadLines(@"D:\office\pos-malaysia\entt\Plan B1\Realtime Scanner\Data\sa_stat_20160101-1009\20161009\20161009\20161009111705_0_sa_stat_20161009_110922_klm15.txt.log");
port.AddHeader("Name", "20161009111705_0_sa_stat_20161009_110922_klm15.txt.log");
var rawList  = port.Process(lines);
//Console.WriteLine (rawList);
var entities = from i in rawList
			   where null != i
			   let json = i.ToJson()
			   select json.DeserializeFromJson<Bespoke.PosEntt.Stats.Domain.Stat>();
var map = new Bespoke.PosEntt.Integrations.Transforms.RtsStatToOalIpsImport();
var tasks = from input in entities
		select map.TransformAsync(input);
var list = await Task.WhenAll(tasks);

list.Dump();