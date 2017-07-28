<Query Kind="Statements">
  <Reference Relative="..\subscribers\domain.sph.dll">C:\project\work\entt.rts\subscribers\domain.sph.dll</Reference>
  <Reference Relative="..\subscribers\PosEntt.EnttAcceptance.dll">C:\project\work\entt.rts\subscribers\PosEntt.EnttAcceptance.dll</Reference>
  <Reference Relative="..\output\PosEntt.IpsExportToEnttAcceptance.dll">C:\project\work\entt.rts\output\PosEntt.IpsExportToEnttAcceptance.dll</Reference>
  <Reference>C:\temp\PosEntt.MailItem.dll</Reference>
  <Reference>C:\temp\PosEntt.ReceivePort.IpsExportPort.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var port = new Bespoke.PosEntt.ReceivePorts.IpsExportPort(new Bespoke.Sph.Domain.Logger());
var lines = File.ReadLines(@"C:\Users\azrolmn\Desktop\ips\IPSExpC_20170728_092543_45350.xml");
//lines.Dump();
//port.AddHeader("Name", "20161007193818_0_sa_pick_20161007_194730_KCH30.txt.log");
var records  = port.Process(lines).ToArray();
//records.Dump();
var entities = from i in records
			   where null != i
			   let json = i.ToJson()
			   select json.DeserializeFromJson<Bespoke.PosEntt.MailItems.Domain.MailItem>();
entities.Dump();
var map = new Bespoke.PosEntt.Integrations.Transforms.IpsExportToEnttAcceptance();
var tasks = from input in entities
			select map.TransformAsync(input);
var list = await Task.WhenAll(tasks);
list.Dump();