<Query Kind="Statements">
  <Reference Relative="..\subscribers\domain.sph.dll">D:\project\work\entt.rts\subscribers\domain.sph.dll</Reference>
  <Reference Relative="..\output\PosEntt.Oal.dll">D:\project\work\entt.rts\output\PosEntt.Oal.dll</Reference>
  <Reference Relative="..\output\PosEntt.ReceivePort.RtsVasn.dll">D:\project\work\entt.rts\output\PosEntt.ReceivePort.RtsVasn.dll</Reference>
  <Reference Relative="..\output\PosEntt.RtsVasnToOal_ips_import.dll">D:\project\work\entt.rts\output\PosEntt.RtsVasnToOal_ips_import.dll</Reference>
  <Reference Relative="..\output\PosEntt.Vasn.dll">D:\project\work\entt.rts\output\PosEntt.Vasn.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var port = new Bespoke.PosEntt.ReceivePorts.RtsVasn(new Bespoke.Sph.Domain.Logger());
//var lines = File.ReadLines(@"D:\office\pos-malaysia\entt\Plan B1\Realtime Scanner\Data\sa_vasn_20160101-1009\20161009\20161009104028_0_sa_vasn_20161009_103908_kjt06.txt.log");
//var lines = File.ReadLines(@"D:\office\pos-malaysia\entt\Plan B1\Realtime Scanner\Data\sa_vasn_20160101-1009\20161009\20161009184335_0_sa_vasn_20161009_184314_jhb98.txt.log");
var lines = File.ReadLines(@"D:\office\pos-malaysia\entt\Plan B1\Realtime Scanner\Data\sa_vasn_20160101-1009\20161009\20161009113137_0_sa_vasn_20161009_113336_tgg45.txt.log");


port.AddHeader("Name", "20161009184335_0_sa_vasn_20161009_184314_jhb98.txt.log");
var rawList  = port.Process(lines);
//Console.WriteLine (rawList);
var entities = from i in rawList
			   where null != i
			   let json = i.ToJson()
			   select json.DeserializeFromJson<Bespoke.PosEntt.Vasns.Domain.Vasn>();
var map = new Bespoke.PosEntt.Integrations.Transforms.RtsVasnToOalIpsImport();
var tasks = from input in entities
		select map.TransformAsync(input);
var list = await Task.WhenAll(tasks);

list.Dump();