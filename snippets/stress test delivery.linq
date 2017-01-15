<Query Kind="Statements">
  <Reference Relative="..\web\bin\domain.sph.dll">F:\project\work\entt.rts\web\bin\domain.sph.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>


var client = new HttpClient() { BaseAddress = new Uri("http://localhost:8080") };
client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyIjoiYWRtaW4iLCJyb2xlcyI6WyJhZG1pbmlzdHJhdG9ycyIsImNhbl9lZGl0X2VudGl0eSIsImNhbl9lZGl0X3dvcmtmbG93IiwiZGV2ZWxvcGVycyJdLCJlbWFpbCI6ImFkbWluQHlvdXJjb21wYW55LmNvbSIsInN1YiI6IjYzNjE5MTI3NjQzODk2MDcxNWZlMDFkMTMzIiwibmJmIjoxNDk5MTQwNDQ0LCJpYXQiOjE0ODM1MDIwNDQsImV4cCI6MTQ4NTgyMDgwMCwiYXVkIjoiUG9zRW50dCJ9.qnPFiz5OyrydBWMW1tP0t8TbTWJkuiFdDjnuc2xGme8");
client.DefaultRequestHeaders.Add("X-Name", "sa_deli_20090226-2301_00001.txt");
client.DefaultRequestHeaders.Add("Accept", "application/json");

var delivery = File.ReadAllLines(@"F:\project\work\entt.rts\samples\pick\20160101000017_0_sa_pick_20160101_001218_sbn71.txt.log");
Parallel.For(0, 50000, async (i) =>
{

	var count = 0;
	foreach (var line in delivery)
	{
		await Task.Delay(80);
		Console.Write("*");
		var content = new StringContent(line.Replace("48211", i.ToString("0000") + count));
		content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");

		var response = await client.PostAsync("api/rts/pick", content);
		if(!response.IsSuccessStatusCode) 
			Console.Write("x");
		else
			Console.Write(".");
	}

});
