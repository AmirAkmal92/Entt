<Query Kind="Statements">
  <Reference Relative="..\subscribers\domain.sph.dll">D:\project\work\entt.rts\subscribers\domain.sph.dll</Reference>
  <Reference Relative="..\output\PosEntt.Delivery.dll">D:\project\work\entt.rts\output\PosEntt.Delivery.dll</Reference>
  <Reference Relative="..\output\PosEntt.Oal.dll">D:\project\work\entt.rts\output\PosEntt.Oal.dll</Reference>
  <Reference Relative="..\output\PosEntt.ReceivePort.RtsDelivey.dll">D:\project\work\entt.rts\output\PosEntt.ReceivePort.RtsDelivey.dll</Reference>
  <Reference Relative="..\output\PosEntt.RtsDeliveryToOal_dbo_delivery_event_new.dll">D:\project\work\entt.rts\output\PosEntt.RtsDeliveryToOal_dbo_delivery_event_new.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var port = new Bespoke.PosEntt.ReceivePorts.RtsDelivey(new Bespoke.Sph.Domain.Logger());
var lines = File.ReadLines(@"D:\office\pos-malaysia\entt\Plan B1\Realtime Scanner\Data\sa_deli_20160101-1009\20161009\20161009182444_0_sa_deli_20161009_181732_spi68.txt.log");
port.AddHeader("Name", "20161009182444_0_sa_deli_20161009_181732_spi68.txt.log");
var rawList  = port.Process(lines);
var entities = from i in rawList
			   where null != i
			   let json = i.ToJson()
			   select json.DeserializeFromJson<Bespoke.PosEntt.Deliveries.Domain.Delivery>();
var map = new Bespoke.PosEntt.Integrations.Transforms.RtsDeliveryToOalDboDeliveryEventNew();
var tasks = from input in entities
		select map.TransformAsync(input);
var list = await Task.WhenAll(tasks);

list.Dump();