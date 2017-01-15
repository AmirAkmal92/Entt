<Query Kind="Statements">
  <Connection>
    <ID>250ef9b7-32a0-4fd9-b29d-106136ea89ee</ID>
    <Persist>true</Persist>
    <Server>PMBIPVSSQL01</Server>
    <SqlSecurity>true</SqlSecurity>
    <Database>oal_dbo</Database>
    <UserName>enttuser</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAMvhJDAascU6e2SccX0C+JgAAAAACAAAAAAADZgAAwAAAABAAAADU2HpQAAeuPUjpF8ohy8/wAAAAAASAAACgAAAAEAAAAIVpP6fJGRI6IITc/73eiewQAAAA0eN0Ta8xrAUnZfO2ehsD4BQAAADnwuF6rBvtqZwiRXUPjFN8Gk+lOA==</Password>
  </Connection>
  <Reference Relative="..\tools\domain.sph.dll">C:\project\work\entt.rts\tools\domain.sph.dll</Reference>
  <Reference Relative="..\tools\Newtonsoft.Json.dll">C:\project\work\entt.rts\tools\Newtonsoft.Json.dll</Reference>
  <Reference Relative="..\output\PosEntt.Deco.dll">C:\project\work\entt.rts\output\PosEntt.Deco.dll</Reference>
  <Reference Relative="..\output\PosEntt.Oal.dll">C:\project\work\entt.rts\output\PosEntt.Oal.dll</Reference>
  <Reference Relative="..\output\PosEntt.RtsDeco_oal_console_details.dll">C:\project\work\entt.rts\output\PosEntt.RtsDeco_oal_console_details.dll</Reference>
  <Reference Relative="..\output\PosEntt.RtsDeco_Oal_delivery_console_event_new.dll">C:\project\work\entt.rts\output\PosEntt.RtsDeco_Oal_delivery_console_event_new.dll</Reference>
  <Reference Relative="..\tools\rts.pickup.babies.dll">C:\project\work\entt.rts\tools\rts.pickup.babies.dll</Reference>
  <AppConfig>
    <Content>
      <configuration>
        <connectionStrings>
          <add name="oal" connectionString="Data Source=PMBIPVSSQL01;Initial Catalog=oal_dbo;User Id=enttuser;password=P@ssw0rd;Connection Timeout=30" providerName="System.Data.SqlClient" />
        </connectionStrings>
      </configuration>
    </Content>
  </AppConfig>
</Query>

var action = new Bespoke.PosEntt.CustomActions.DecoWithBabiesAction();
var json = @"{""CourierId"":""34641"",""LocationId"":""5009"",""BeatNo"":""220"",""Date"":""2017-01-09T00:00:00"",""Time"":""2017-01-09T08:55:23"",""ConsoleTag"":""CG051495318MY"",""Comment"":""MBSB"",""AllConsignmentnNotes"":""EN138385467MY,EN261525210MY,EH346002496MY,EF698474675MY,EH366968610MY,EN320249325MY,EP286446447MY,EF753654794MY,EP305251689MY,EH350614557MY,EN089698869MY,EN309760126MY,EF698474667MY,EF698474653MY,EN317904927MY,EN089698886MY,EN290203305MY,EN089696681MY,EN180164723MY,EF824458217MY,EF866019636MY,EP299425627MY,EH365145722MY,EM964354133MY,EN106165986MY,EN256978447MY,EH365145753MY,EF866019295MY,EN232904691MY,EN261526498MY,EN261526507MY,EN261525183MY,EN136937763MY,EN817822489MY,EN115164621MY,EN256306066MY,EF854845550MY,EN119658477MY,EN216253294MY,EN246460936MY,EN303860660MY,EN261526475MY,EH429329187MY,EN246460882MY,EN246460940MY,EN316181649MY,EN316181635MY,EN316081685MY,EN289502094MY,EN289501862MY,EF854820694MY,EN078710453MY,EF477661425MY,EN216253303MY,EN310117687MY,EN277514926MY,ED281196098MY,EN159422593MY"",""TotalConsignment"":58,""ScannerId"":""KUL57"",""CreatedBy"":""KUL57"",""Id"":""271e3191-aadd-4cc3-98e8-e7c9ffe10e41"",""CreatedDate"":""2017-01-09T09:10:53.7724467+08:00"",""ChangedBy"":""KUL57"",""ChangedDate"":""2017-01-09T09:10:53.7724467+08:00"",""WebId"":""271e3191-aadd-4cc3-98e8-e7c9ffe10e41""}";
var deco = Bespoke.Sph.Domain.JsonSerializerService.DeserializeFromJson<Bespoke.PosEntt.Decos.Domain.Deco>(json);


await action.RunAsync(deco);