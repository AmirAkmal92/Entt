<Query Kind="Statements">
  <Reference Relative="..\subscribers\domain.sph.dll">D:\project\work\entt.rts\subscribers\domain.sph.dll</Reference>
  <Reference Relative="..\output\PosEntt.Oal.dll">D:\project\work\entt.rts\output\PosEntt.Oal.dll</Reference>
  <Reference Relative="..\output\PosEntt.ReceivePort.RtsSip.dll">D:\project\work\entt.rts\output\PosEntt.ReceivePort.RtsSip.dll</Reference>
  <Reference Relative="..\output\PosEntt.RtsSipToOalSipEventNew.dll">D:\project\work\entt.rts\output\PosEntt.RtsSipToOalSipEventNew.dll</Reference>
  <Reference Relative="..\output\PosEntt.Sip.dll">D:\project\work\entt.rts\output\PosEntt.Sip.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var port = new Bespoke.PosEntt.ReceivePorts.RtsSip(new Bespoke.Sph.Domain.Logger());
var lines = File.ReadLines(@"D:\office\pos-malaysia\entt\Plan B1\Realtime Scanner\Data\sa_files_20160101-1009\20161007\20161007004236_0_sa_sip_20161007_003906_kl170.txt.log");
port.AddHeader("Name", "20161007004236_0_sa_sip_20161007_003906_kl170.txt.log");
var rawList  = port.Process(lines);
//Console.WriteLine (rawList);
var entities = from i in rawList
			   where null != i
			   let json = i.ToJson()
			   select json.DeserializeFromJson<Bespoke.PosEntt.Sips.Domain.Sip>();
var map = new Bespoke.PosEntt.Integrations.Transforms.RtsSipToOalSipEventNew();
var tasks = from input in entities
		select map.TransformAsync(input);
var list = await Task.WhenAll(tasks);

list.Dump();