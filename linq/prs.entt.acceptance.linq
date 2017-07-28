<Query Kind="Statements">
  <Reference Relative="..\subscribers\domain.sph.dll">C:\project\work\entt.rts\subscribers\domain.sph.dll</Reference>
  <Reference Relative="..\output\PosEntt.AcceptanceData.dll">C:\project\work\entt.rts\output\PosEntt.AcceptanceData.dll</Reference>
  <Reference Relative="..\subscribers\PosEntt.EnttAcceptance.dll">C:\project\work\entt.rts\subscribers\PosEntt.EnttAcceptance.dll</Reference>
  <Reference Relative="..\output\PosEntt.PrsAcceptanceToEnttAcceptance.dll">C:\project\work\entt.rts\output\PosEntt.PrsAcceptanceToEnttAcceptance.dll</Reference>
  <Reference Relative="..\output\PosEntt.ReceivePort.PrsAcceptancePort.dll">C:\project\work\entt.rts\output\PosEntt.ReceivePort.PrsAcceptancePort.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var port = new Bespoke.PosEntt.ReceivePorts.PrsAcceptancePort(new Bespoke.Sph.Domain.Logger());
var lines = File.ReadLines(@"D:\office\pos-malaysia\entt\sample\prs\AcceptanceData\AcceptanceData20170629213114.xml");
//lines.Dump();
//port.AddHeader("Name", "20161007193818_0_sa_pick_20161007_194730_KCH30.txt.log");
var records  = port.Process(lines).ToArray();
//records.Dump();
var entities = from i in records
			   where null != i
			   let json = i.ToJson()
			   select json.DeserializeFromJson<Bespoke.PosEntt.AcceptanceDatas.Domain.AcceptanceData>();
entities.Dump();
var map = new Bespoke.PosEntt.Integrations.Transforms.PrsAcceptanceToEnttAcceptance();
var tasks = from input in entities
			select map.TransformAsync(input);
var list = await Task.WhenAll(tasks);
list.Dump();