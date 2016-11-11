<Query Kind="Statements">
  <Reference Relative="..\subscribers\domain.sph.dll">D:\project\work\entt.rts\subscribers\domain.sph.dll</Reference>
  <Reference Relative="..\output\PosEntt.Norm.dll">D:\project\work\entt.rts\output\PosEntt.Norm.dll</Reference>
  <Reference Relative="..\output\PosEntt.Oal.dll">D:\project\work\entt.rts\output\PosEntt.Oal.dll</Reference>
  <Reference Relative="..\output\PosEntt.ReceivePort.RtsNorm.dll">D:\project\work\entt.rts\output\PosEntt.ReceivePort.RtsNorm.dll</Reference>
  <Reference Relative="..\output\PosEntt.RtsNormToOal_console_details.dll">D:\project\work\entt.rts\output\PosEntt.RtsNormToOal_console_details.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var port = new Bespoke.PosEntt.ReceivePorts.RtsNorm(new Bespoke.Sph.Domain.Logger());
port.AddHeader("Name", "20160203102133_0_sa_norm_20160102_110708_tph59.txt.log");
var lines = File.ReadLines(@"D:\office\pos-malaysia\entt\Plan B1\Realtime Scanner\Data\sa_files_20160101-1009\20160102\20160102\20160203102133_0_sa_norm_20160102_110708_tph59.txt.log");
var rawList  = port.Process(lines);
//Console.WriteLine (rawList);
var entities = from i in rawList
			   where null != i
			   let json = i.ToJson()
			   select json.DeserializeFromJson<Bespoke.PosEntt.Norms.Domain.Norm>();
var map = new Bespoke.PosEntt.Integrations.Transforms.RtsNormToOalConsoleDetails();
var tasks = from input in entities
		select map.TransformAsync(input);
var list = await Task.WhenAll(tasks);

list.Dump();