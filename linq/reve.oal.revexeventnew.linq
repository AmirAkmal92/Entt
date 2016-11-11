<Query Kind="Statements">
  <Reference Relative="..\subscribers\domain.sph.dll">D:\project\work\entt.rts\subscribers\domain.sph.dll</Reference>
  <Reference Relative="..\output\PosEntt.Oal.dll">D:\project\work\entt.rts\output\PosEntt.Oal.dll</Reference>
  <Reference Relative="..\output\PosEntt.ReceivePort.RtsReve.dll">D:\project\work\entt.rts\output\PosEntt.ReceivePort.RtsReve.dll</Reference>
  <Reference Relative="..\output\PosEntt.Reve.dll">D:\project\work\entt.rts\output\PosEntt.Reve.dll</Reference>
  <Reference Relative="..\output\PosEntt.RtsReveToOalRevexEventNew.dll">D:\project\work\entt.rts\output\PosEntt.RtsReveToOalRevexEventNew.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var port = new Bespoke.PosEntt.ReceivePorts.RtsReve(new Bespoke.Sph.Domain.Logger());
var lines = File.ReadLines(@"D:\office\pos-malaysia\entt\Plan B1\Realtime Scanner\Data\sa_files_20160101-1009\20161007\20161007163947_0_sa_reve_20161007_163454_bwh02.txt.log");
port.AddHeader("Name", "20161007163947_0_sa_reve_20161007_163454_bwh02.txt.log");
var rawList  = port.Process(lines);
//Console.WriteLine (rawList);
var entities = from i in rawList
			   where null != i
			   let json = i.ToJson()
			   select json.DeserializeFromJson<Bespoke.PosEntt.Reves.Domain.Reve>();
var map = new Bespoke.PosEntt.Integrations.Transforms.RtsReveToOalRevexEventNew();
var tasks = from input in entities
		select map.TransformAsync(input);
var list = await Task.WhenAll(tasks);

list.Dump();