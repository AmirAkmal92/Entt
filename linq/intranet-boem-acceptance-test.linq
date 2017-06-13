<Query Kind="Statements">
  <Reference Relative="..\subscribers\domain.sph.dll">C:\project\work\entt.rts\subscribers\domain.sph.dll</Reference>
  <Reference Relative="..\output\PosEntt.IntranetBoem.dll">C:\project\work\entt.rts\output\PosEntt.IntranetBoem.dll</Reference>
  <Reference Relative="..\output\PosEntt.ReceivePort.IntranetBoemPort.dll">C:\project\work\entt.rts\output\PosEntt.ReceivePort.IntranetBoemPort.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var port = new Bespoke.PosEntt.ReceivePorts.IntranetBoemPort(new Bespoke.Sph.Domain.Logger());
var data = File.ReadAllLines(@"C:\Users\Amir Akmal\Entt\rts\acceptance example\intranet\20170607113110_0_ipos_boem_8125_20170607-110621_12_2195.txt.log");
var boem = port.Process(data);
//pems.Dump();
var entities = from i in boem
			   where null != i
			   let json = i.ToJson()
			   select json.DeserializeFromJson<Bespoke.PosEntt.IntranetBoems.Domain.IntranetBoem>();
entities.Dump();