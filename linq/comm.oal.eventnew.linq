<Query Kind="Statements">
  <Reference Relative="..\subscribers\domain.sph.dll">D:\project\work\entt.rts\subscribers\domain.sph.dll</Reference>
  <Reference Relative="..\output\PosEntt.Comm.dll">D:\project\work\entt.rts\output\PosEntt.Comm.dll</Reference>
  <Reference Relative="..\output\PosEntt.Oal.dll">D:\project\work\entt.rts\output\PosEntt.Oal.dll</Reference>
  <Reference Relative="..\output\PosEntt.ReceivePort.RtsComm.dll">D:\project\work\entt.rts\output\PosEntt.ReceivePort.RtsComm.dll</Reference>
  <Reference Relative="..\output\PosEntt.RtsCommToOalCommEventNew.dll">D:\project\work\entt.rts\output\PosEntt.RtsCommToOalCommEventNew.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var port = new Bespoke.PosEntt.ReceivePorts.RtsComm(new Bespoke.Sph.Domain.Logger());
var lines = File.ReadLines(@"D:\office\pos-malaysia\entt\Plan B1\Realtime Scanner\Data\sa_files_20160101-1009\20161007\20161007082231_0_sa_comm_20161007_082448_kua80.txt.log");
port.AddHeader("Name", "20161007082231_0_sa_comm_20161007_082448_kua80.txt.log");
var rawList  = port.Process(lines);
//Console.WriteLine (rawList);
var entities = from i in rawList
			   where null != i
			   let json = i.ToJson()
			   select json.DeserializeFromJson<Bespoke.PosEntt.Comms.Domain.Comm>();
var map = new Bespoke.PosEntt.Integrations.Transforms.RtsCommToOalCommEventNew();
var tasks = from input in entities
		select map.TransformAsync(input);
var list = await Task.WhenAll(tasks);

list.Dump();