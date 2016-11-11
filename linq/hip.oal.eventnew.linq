<Query Kind="Statements">
  <Reference Relative="..\subscribers\domain.sph.dll">D:\project\work\entt.rts\subscribers\domain.sph.dll</Reference>
  <Reference Relative="..\output\PosEntt.Hip.dll">D:\project\work\entt.rts\output\PosEntt.Hip.dll</Reference>
  <Reference Relative="..\output\PosEntt.Oal.dll">D:\project\work\entt.rts\output\PosEntt.Oal.dll</Reference>
  <Reference Relative="..\output\PosEntt.ReceivePort.RtsHip.dll">D:\project\work\entt.rts\output\PosEntt.ReceivePort.RtsHip.dll</Reference>
  <Reference Relative="..\output\PosEntt.RtsHipToOal_hip_event_new.dll">D:\project\work\entt.rts\output\PosEntt.RtsHipToOal_hip_event_new.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var port = new Bespoke.PosEntt.ReceivePorts.RtsHip(new Bespoke.Sph.Domain.Logger());
var lines = File.ReadLines(@"D:\office\pos-malaysia\entt\Plan B1\Realtime Scanner\Data\sa_files_20160101-1009\20160102\20160102\20160102152819_0_sa_hip_20160102_153544_pkg62.txt.log");
port.AddHeader("Name", "20160102152819_0_sa_hip_20160102_153544_pkg62.txt.log");
var rawList  = port.Process(lines);
//Console.WriteLine (rawList);
var entities = from i in rawList
			   where null != i
			   let json = i.ToJson()
			   select json.DeserializeFromJson<Bespoke.PosEntt.Hips.Domain.Hip>();
var map = new Bespoke.PosEntt.Integrations.Transforms.RtsHipToOalHipEventNew();
var tasks = from input in entities
		select map.TransformAsync(input);
var list = await Task.WhenAll(tasks);

list.Dump();