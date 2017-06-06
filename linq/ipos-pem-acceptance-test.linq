<Query Kind="Statements">
  <Reference Relative="..\subscribers\domain.sph.dll">C:\project\work\entt.rts\subscribers\domain.sph.dll</Reference>
  <Reference Relative="..\output\PosEntt.IposPem.dll">C:\project\work\entt.rts\output\PosEntt.IposPem.dll</Reference>
  <Reference Relative="..\output\PosEntt.ReceivePort.IposPemPort.dll">C:\project\work\entt.rts\output\PosEntt.ReceivePort.IposPemPort.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var port = new Bespoke.PosEntt.ReceivePorts.IposPemPort(new Bespoke.Sph.Domain.Logger());
var data = File.ReadAllLines(@"C:\Users\azrolmn\Downloads\ipos_pem_0240_20170605-154713_8_8.txt");
var pems = port.Process(data);
//pems.Dump();
var entities = from i in pems
			   where null != i
			   let json = i.ToJson()
			   select json.DeserializeFromJson<Bespoke.PosEntt.IposPems.Domain.IposPem>();
entities.Dump();