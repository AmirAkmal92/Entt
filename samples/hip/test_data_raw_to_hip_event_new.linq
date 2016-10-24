<Query Kind="Statements">
  <Reference Relative="..\..\subscribers\domain.sph.dll">C:\project\work\entt.rts\subscribers\domain.sph.dll</Reference>
  <Reference Relative="..\..\subscribers\FileHelpers.dll">C:\project\work\entt.rts\subscribers\FileHelpers.dll</Reference>
  <Reference Relative="..\..\output\PosEntt.Hip.dll">C:\project\work\entt.rts\output\PosEntt.Hip.dll</Reference>
  <Reference Relative="..\..\output\PosEntt.Oal.dll">C:\project\work\entt.rts\output\PosEntt.Oal.dll</Reference>
  <Reference Relative="..\..\output\PosEntt.ReceivePort.RtsHip.dll">C:\project\work\entt.rts\output\PosEntt.ReceivePort.RtsHip.dll</Reference>
  <Reference Relative="..\..\output\PosEntt.RtsHipToOal_hip_event_new.dll">C:\project\work\entt.rts\output\PosEntt.RtsHipToOal_hip_event_new.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var port = new Bespoke.PosEntt.ReceivePorts.RtsHip(new Bespoke.Sph.Domain.Logger());
var lines = File.ReadLines(@"C:\project\work\entt.rts\samples\hip\20160128000651_0_sa_hip_20160128_000250_kjt02.txt.log");
var rawList  = port.Process(lines);

var entities = from i in rawList
			   where null != i
			   let json = i.ToJson()
			   select json.DeserializeFromJson<Bespoke.PosEntt.Hips.Domain.Hip>();

var map = new Bespoke.PosEntt.Integrations.Transforms.RtsHipToOalHipEventNew();
var tasks = from input in entities
		select map.TransformAsync(input);
var list = await Task.WhenAll(tasks);

list.Dump();



