<Query Kind="Statements">
  <Reference Relative="..\tools\domain.sph.dll">F:\project\work\entt.rts\tools\domain.sph.dll</Reference>
  <Reference Relative="..\tools\Newtonsoft.Json.dll">F:\project\work\entt.rts\tools\Newtonsoft.Json.dll</Reference>
  <Reference Relative="..\output\PosEntt.Deco.dll">F:\project\work\entt.rts\output\PosEntt.Deco.dll</Reference>
  <Reference Relative="..\output\PosEntt.Oal.dll">F:\project\work\entt.rts\output\PosEntt.Oal.dll</Reference>
  <Reference Relative="..\output\PosEntt.RtsDeco_oal_console_details.dll">F:\project\work\entt.rts\output\PosEntt.RtsDeco_oal_console_details.dll</Reference>
  <Reference Relative="..\output\PosEntt.RtsDeco_Oal_delivery_console_event_new.dll">F:\project\work\entt.rts\output\PosEntt.RtsDeco_Oal_delivery_console_event_new.dll</Reference>
  <Reference Relative="..\tools\rts.pickup.babies.dll">F:\project\work\entt.rts\tools\rts.pickup.babies.dll</Reference>
  <AppConfig>
    <Content>
      <configuration>
        <connectionStrings>
          <add name="oal" connectionString="Data Source=S301\DEV2016;Initial Catalog=oal_dbo_azrol_20161102;Integrated Security=True" providerName="System.Data.SqlClient" />
        </connectionStrings>
      </configuration>
    </Content>
  </AppConfig>
</Query>

var action = new Bespoke.PosEntt.CustomActions.DecoWithBabiesAction();
var deco = new Bespoke.PosEntt.Decos.Domain.Deco
{
	LocationId = "8800",
    AllConsignmentnNotes = "EH251745750MY, EH251745729MY, EH335237235MY, EH251747438MY, EH251749266MY, EH251749411MY, EN165464822MY, EN165464779MY, EH251427221MY, EH335205515MY, EH335221629MY, EH251745613MY, EH251747027MY, EH251749884MY, EH251745746MY, EH251749822MY, EH251746928MY, EH251749685MY, EP299749146MY, EP299749129MY, EH335221589MY, EH335221592MY, EH251746976MY, EH251749796MY, EH269017258MY, EE048707053MY, EH251745732MY, EN815662529MY, EH251749235MY, EH253677139MY, EH237608673MY, EH335221632MY, EN165830225MY, EH335305301MY, EH123343491MY, EH251745692MY, EM986906995MY, EH251749694MY, EN274991555MY, EH251749575MY, EH152868426MY, EH251745701MY, EM986916269MY, EH255124892MY, EH255124889MY, EH255124861MY, EH251745383MY, EN274991569MY, EN274991590MY, EH335221663MY, EH251745661MY, EH251798341MY, EE028090213MY, EH325841101MY, EH251745644MY, EH335208905MY, EH237429310MY, EH251749867MY, EH269065868MY, EH251749765MY, EH251745627MY, EH251749853MY, EH325755632MY, EH251745763MY, EP229955194MY, EH335265491MY, EP299749132MY, EH251747013MY, EF730648623MY, EH251936428MY, EH335265528MY, EH325841163MY, EP229955132MY, EH325908545MY, EH326009623MY, EH251749632MY, EH335203792MY, EH253677156MY, EH325908537MY, EH335265465MY, EH251749725MY, EH251749663MY, EH326006437MY, EH251749495MY, EH335224024MY, EH251749629MY, EH123343236MY, EH123343324MY, EH123343275MY, EH123343267MY, EH251749734MY, EH251745658MY, EH251745715MY, EH123343240MY, EH123343284MY, EH147688165MY, EH251749439MY, EN034172512MY, EH123343253MY, EH251749473MY, EN306424981MY, EH079896215MY, EN275649041MY, EH255249610MY, EH253677142MY, EH326006423MY, EH326006410MY, EH251749805MY, EH255154086MY, EH251749535MY, EH079896224MY, EE048707022MY, EH237525571MY, EN274991541MY, EH251749601MY, EH371595014MY, EH335203850MY, EH251745352MY, EH251745635MY, EH251745675MY, EH251747146MY, EE048708819MY",
	BeatNo = "1",
	Comment = "1",
	ConsoleTag = "CG050311175MY",
	Id = Guid.NewGuid().ToString(),
	Date = DateTime.Parse("2017-01-01"),
	ScannerId = "",
	Time = DateTime.Parse("2017-01-01 16:24:06"),
	TotalConsignment = 5,
	CourierId = "20501"
};

await action.RunAsync(deco);