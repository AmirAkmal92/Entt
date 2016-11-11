<Query Kind="Statements">
  <Reference Relative="..\subscribers\domain.sph.dll">D:\project\work\entt.rts\subscribers\domain.sph.dll</Reference>
  <Reference Relative="..\output\PosEntt.Miss.dll">D:\project\work\entt.rts\output\PosEntt.Miss.dll</Reference>
  <Reference Relative="..\output\PosEntt.Oal.dll">D:\project\work\entt.rts\output\PosEntt.Oal.dll</Reference>
  <Reference Relative="..\output\PosEntt.ReceivePort.RtsMiss.dll">D:\project\work\entt.rts\output\PosEntt.ReceivePort.RtsMiss.dll</Reference>
  <Reference Relative="..\output\PosEntt.RtsMissToOalMissortEventNew.dll">D:\project\work\entt.rts\output\PosEntt.RtsMissToOalMissortEventNew.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var port = new Bespoke.PosEntt.ReceivePorts.RtsMiss(new Bespoke.Sph.Domain.Logger());
var lines = File.ReadLines(@"D:\office\pos-malaysia\entt\Plan B1\Realtime Scanner\Data\sa_files_20160101-1009\20161007\20161007035826_0_sa_miss_20161007_035448_kl166.txt.log");
port.AddHeader("Name", "20161007035826_0_sa_miss_20161007_035448_kl166.txt.log");
var rawList  = port.Process(lines);
//Console.WriteLine (rawList);
var entities = from i in rawList
			   where null != i
			   let json = i.ToJson()
			   select json.DeserializeFromJson<Bespoke.PosEntt.Misses.Domain.Miss>();
var map = new Bespoke.PosEntt.Integrations.Transforms.RtsMissToOalMissortEventNew();
var tasks = from input in entities
		select map.TransformAsync(input);
var list = await Task.WhenAll(tasks);

list.Dump();