<Query Kind="Statements">
  <Reference Relative="..\..\subscribers.host\domain.sph.dll">C:\project\work\entt.rts\subscribers.host\domain.sph.dll</Reference>
  <Reference Relative="..\..\output\PosEntt.Deco.dll">C:\project\work\entt.rts\output\PosEntt.Deco.dll</Reference>
  <Reference Relative="..\..\output\PosEntt.Oal.dll">C:\project\work\entt.rts\output\PosEntt.Oal.dll</Reference>
  <Reference Relative="..\..\output\PosEntt.RtsDeco_oal_console_details.dll">C:\project\work\entt.rts\output\PosEntt.RtsDeco_oal_console_details.dll</Reference>
</Query>

var input = new Bespoke.PosEntt.Decos.Domain.Deco {
BeatNo = "123"
};
var map = new Bespoke.PosEntt.Integrations.Transforms.RtsDecoOalConsoleDetails();

var output =await map.TransformAsync(input);

output.Dump();