<Query Kind="Statements">
  <Reference Relative="..\subscribers\domain.sph.dll">C:\project\work\entt.rts\subscribers\domain.sph.dll</Reference>
  <Reference Relative="..\output\PosEntt.IPosBoem.dll">C:\project\work\entt.rts\output\PosEntt.IPosBoem.dll</Reference>
  <Reference Relative="..\output\PosEntt.IposBoemToOalConsignmentUpdate.dll">C:\project\work\entt.rts\output\PosEntt.IposBoemToOalConsignmentUpdate.dll</Reference>
  <Reference Relative="..\output\PosEntt.ReceivePort.IposBoemPort.dll">C:\project\work\entt.rts\output\PosEntt.ReceivePort.IposBoemPort.dll</Reference>
  <Reference Relative="..\subscribers\PosEntt.Oal.dll">C:\project\work\entt.rts\subscribers\PosEntt.Oal.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var port = new Bespoke.PosEntt.ReceivePorts.IposBoemPort(new Bespoke.Sph.Domain.Logger());
var data = File.ReadAllLines(@"C:\Users\azrolmn\Downloads\20170607113110_0_ipos_boem_8125_20170607-110621_12_2195.txt.log");
var boem = port.Process(data);
//pems.Dump();
var entities = from i in boem
			   where null != i
			   let json = i.ToJson()
			   select json.DeserializeFromJson<Bespoke.PosEntt.IposBoems.Domain.IposBoem>();
//entities.Dump();
var map = new Bespoke.PosEntt.Integrations.Transforms.IposBoemToOalConsignmentUpdate();
var tasks = from input in entities
			select map.TransformAsync(input);
var list = await Task.WhenAll(tasks);
list.Dump();