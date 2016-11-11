<Query Kind="Statements">
  <Reference Relative="..\subscribers\domain.sph.dll">D:\project\work\entt.rts\subscribers\domain.sph.dll</Reference>
  <Reference Relative="..\output\PosEntt.Oal.dll">D:\project\work\entt.rts\output\PosEntt.Oal.dll</Reference>
  <Reference Relative="..\output\PosEntt.ReceivePort.RtsSop.dll">D:\project\work\entt.rts\output\PosEntt.ReceivePort.RtsSop.dll</Reference>
  <Reference Relative="..\output\PosEntt.RtsSopToOalSopEventNew.dll">D:\project\work\entt.rts\output\PosEntt.RtsSopToOalSopEventNew.dll</Reference>
  <Reference Relative="..\output\PosEntt.Sop.dll">D:\project\work\entt.rts\output\PosEntt.Sop.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var port = new Bespoke.PosEntt.ReceivePorts.RtsSop(new Bespoke.Sph.Domain.Logger());
var lines = File.ReadLines(@"D:\office\pos-malaysia\entt\Plan B1\Realtime Scanner\Data\sa_files_20160101-1009\20161007\20161007004401_0_sa_sop_20161007_003000_cs113.txt.log");
port.AddHeader("Name", "20161007004401_0_sa_sop_20161007_003000_cs113.txt.log");
var rawList  = port.Process(lines);
//Console.WriteLine (rawList);
var entities = from i in rawList
			   where null != i
			   let json = i.ToJson()
			   select json.DeserializeFromJson<Bespoke.PosEntt.Sops.Domain.Sop>();
var map = new Bespoke.PosEntt.Integrations.Transforms.RtsSopToOalSopEventNew();
var tasks = from input in entities
		select map.TransformAsync(input);
var list = await Task.WhenAll(tasks);

list.Dump();