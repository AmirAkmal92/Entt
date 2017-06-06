<Query Kind="Statements">
  <Reference Relative="..\subscribers\domain.sph.dll">C:\project\work\entt.rts\subscribers\domain.sph.dll</Reference>
  <Reference Relative="..\output\PosEntt.IposPem.dll">C:\project\work\entt.rts\output\PosEntt.IposPem.dll</Reference>
  <Reference Relative="..\output\PosEntt.ReceivePort.IposPemPort.dll">C:\project\work\entt.rts\output\PosEntt.ReceivePort.IposPemPort.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var port = new Bespoke.PosEntt.ReceivePorts.IposBoemPort(new Bespoke.Sph.Domain.Logger());
var data = File.ReadAllLines(@"C:\Users\Amir Akmal\Downloads\Store Procedure\Sample Files\20170605080911_0_ipos_boem_5009_20170605-080908_9_8.txt");
var boem = port.Process(data);
//pems.Dump();
var entities = from i in boem
			   where null != i
			   let json = i.ToJson()
			   select json.DeserializeFromJson<Bespoke.PosEntt.IposBoem.Domain.IposBoem>();
entities.Dump();