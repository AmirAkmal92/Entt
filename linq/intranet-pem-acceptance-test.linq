<Query Kind="Statements">
  <Reference Relative="..\subscribers\domain.sph.dll">C:\project\work\entt.rts\subscribers\domain.sph.dll</Reference>
  <Reference Relative="..\output\PosEntt.IntranetPem.dll">C:\project\work\entt.rts\output\PosEntt.IntranetPem.dll</Reference>
  <Reference Relative="..\output\PosEntt.ReceivePort.IntranetPemPort.dll">C:\project\work\entt.rts\output\PosEntt.ReceivePort.IntranetPemPort.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var port = new Bespoke.PosEntt.ReceivePorts.IntranetPemPort(new Bespoke.Sph.Domain.Logger());
var data = File.ReadAllLines(@"C:\Users\Amir Akmal\Entt\rts\acceptance example\intranet\20170607120408_0_ipos_pem_9615_20170607-120653_4_2757.txt.log");
var pems = port.Process(data);
//pems.Dump();
var entities = from i in pems
			   where null != i
			   let json = i.ToJson()
			   select json.DeserializeFromJson<Bespoke.PosEntt.IntranetPems.Domain.IntranetPem>();
entities.Dump();