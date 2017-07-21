<Query Kind="Statements">
  <Reference Relative="..\subscribers\domain.sph.dll">C:\project\work\entt.rts\subscribers\domain.sph.dll</Reference>
  <Reference Relative="..\output\PosEntt.IposPem.dll">C:\project\work\entt.rts\output\PosEntt.IposPem.dll</Reference>
  <Reference Relative="..\output\PosEntt.IposPemToWwpEventNewLog.dll">C:\project\work\entt.rts\output\PosEntt.IposPemToWwpEventNewLog.dll</Reference>
  <Reference Relative="..\output\PosEntt.Oal.dll">C:\project\work\entt.rts\output\PosEntt.Oal.dll</Reference>
  <Reference Relative="..\output\PosEntt.ReceivePort.IposPemPort.dll">C:\project\work\entt.rts\output\PosEntt.ReceivePort.IposPemPort.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <AppConfig>
    <Content>
      <configuration>
        <connectionStrings>
          <add name="Oal" connectionString="Data Source=(localdb)\ProjectsV13;Initial Catalog=oal;Integrated Security=True;Connect Timeout=30;Asynchronous Processing=True" providerName="System.Data.SqlClient"></add>
        </connectionStrings>
      </configuration>
    </Content>
  </AppConfig>
</Query>

var port = new Bespoke.PosEntt.ReceivePorts.IposPemPort(new Bespoke.Sph.Domain.Logger());
var lines = File.ReadLines(@"C:\Users\Amir Akmal\Entt\rts\acceptance example\iPos\Store Procedure\Sample Files\test.txt");
//port.AddHeader("Name", "20161007193818_0_sa_pick_20161007_194730_KCH30.txt.log");
var rawList  = port.Process(lines);
//Console.WriteLine (rawList);
var entities = from i in rawList
			   where null != i
			   let json = i.ToJson()
			   select json.DeserializeFromJson<Bespoke.PosEntt.IposPems.Domain.IposPem>();
var map = new Bespoke.PosEntt.Integrations.Transforms.IposPemToWwpEventNewLog();
var tasks = from input in entities
		select map.TransformAsync(input);
var list = await Task.WhenAll(tasks);
//list.Dump();

//var adapter = new Bespoke.PosEntt.Adapters.Oal.dbo_pickup_event_newAdapter();
//foreach (var item in list)
//{
//	var result = await adapter.InsertAsync(item);
//	result.Dump();
//}
list.Dump();