<Query Kind="Statements">
  <Reference Relative="..\subscribers\domain.sph.dll">C:\project\work\entt.rts\subscribers\domain.sph.dll</Reference>
  <Reference>C:\temp\PosEntt.MailItem.dll</Reference>
  <Reference>C:\temp\PosEntt.ReceivePort.IpsExportPort.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var port = new Bespoke.PosEntt.ReceivePorts.IpsExportPort(new Bespoke.Sph.Domain.Logger());
var lines = File.ReadLines(@"C:\Users\azrolmn\Desktop\ips\IPSExport.xml");
//lines.Dump();
//port.AddHeader("Name", "20161007193818_0_sa_pick_20161007_194730_KCH30.txt.log");
var records  = port.Process(lines).ToArray();
records.Dump();