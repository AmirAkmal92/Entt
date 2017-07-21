<Query Kind="Statements">
  <Reference Relative="..\subscribers\domain.sph.dll">C:\project\work\entt.rts\subscribers\domain.sph.dll</Reference>
  <Reference Relative="..\output\PosEntt.AcceptanceData.dll">C:\project\work\entt.rts\output\PosEntt.AcceptanceData.dll</Reference>
  <Reference Relative="..\output\PosEntt.ReceivePort.PrsAcceptancePort.dll">C:\project\work\entt.rts\output\PosEntt.ReceivePort.PrsAcceptancePort.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var port = new Bespoke.PosEntt.ReceivePorts.PrsAcceptancePort(new Bespoke.Sph.Domain.Logger());
var lines = File.ReadLines(@"C:\Users\azrolmn\Desktop\AcceptanceData20170605144701.xml");
//lines.Dump();
//port.AddHeader("Name", "20161007193818_0_sa_pick_20161007_194730_KCH30.txt.log");
var records  = port.Process(lines).ToArray();
records.Dump();
