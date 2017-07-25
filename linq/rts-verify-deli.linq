<Query Kind="Program">
  <Reference>C:\project\work\entt.rts\subscribers.host\Newtonsoft.Json.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <Namespace>System.Globalization</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>System.Dynamic</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net.Http</Namespace>
</Query>

async Task Main()
{
	var sb = new StringBuilder();
	var folder = @"C:\Users\azrolmn\Desktop\rts\26052017";
	var files = System.IO.Directory.GetFiles(folder, "*sa_deli*", SearchOption.AllDirectories);
	Console.WriteLine("{0} file(s) found", files.Count());
	sb.AppendFormat("Folder: {0}", folder);
	sb.AppendFormat("{0} file(s) found\r\n", files.Count());
	var i = 1;
	
	foreach (var f in files)
	{
		Console.Write("{0}|{1}|",i,Path.GetFileName(f));
		sb.AppendLine(Path.GetFileName(f));
		
		var lines = File.ReadAllLines(f);
		foreach (var item in lines)
		{
			//Console.WriteLine(item);
			var data = item.Split('\t');
			try
			{
				var dt = DateTime.ParseExact(data[3] + data[4], "ddMMyyyyHHmmss", CultureInfo.InvariantCulture);
				var deli = new Deli { CourierId = data[0], Location = data[1], ConnoteNo = data[5].Trim(), DateTime = dt };				
				
				var exist = await CheckConnoteExist(deli);
				if (!exist)
				{
					Console.Write("{0}|", deli.ConnoteNo);
					sb.AppendFormat("\t***NOTFOUND***|{0}|{1}|{2}\r\n", deli.ConnoteNo, deli.CourierId,deli.DateTime);
				}
				else
				{
					sb.AppendFormat("\tOK|{0}|{1}|{2}\r\n", deli.ConnoteNo, deli.CourierId, deli.DateTime);
					Console.Write(".");
				}
			}
			catch (IndexOutOfRangeException ex)
			{
				Console.WriteLine("\t*** File cannot be read ***");
				sb.AppendLine("\t*** File cannot be read ***");
			}
		}
		Console.WriteLine();
		i++;
	}
	
	var filename = string.Format("sa-deli-{0:ddMMyyyyHHmmsss}.log", DateTime.Now);
	var filePath = Path.Combine(folder,filename);
	File.WriteAllText(filePath,sb.ToString());
	Console.WriteLine("***Program Ended***");
}

async Task<bool> CheckConnoteExist(Deli item)
{
	string req = "http://10.1.16.110:9200/posentt_rts/delivery/_search?pretty";
	string pattern = "{{\"query\": {{\"bool\": {{\"must\": [{{\"range\": {{\"CreatedDate\": {{\"from\": \"2017-05-26T00:00:00+08:00\",\"to\": \"2017-05-27T23:59:59+08:00\"}}}}}},{{\"term\": {{\"ConsignmentNo\": \"{0}\"}}}}]}}}},\"from\": 0,\"size\": 2,\"sort\": [{{\"CreatedDate\": {{\"order\": \"desc\"}}}}]}}";
	var query = string.Format(pattern, item.ConnoteNo);
	using (var client = new HttpClient())
	{
		var data = new StringContent(query);
		var task = await client.PostAsync(new Uri(req), data);
		task.EnsureSuccessStatusCode();
		var response = await task.Content.ReadAsStringAsync();
		dynamic results = JsonConvert.DeserializeObject<ExpandoObject>(response);
		return results.hits.total > 0;
		//Console.WriteLine(results.hits.hits[0]._source);
	}
}

public class Deli
{
	public string CourierId { set; get; }
	public string Location { set; get; }
	public string ConnoteNo { set; get; }
	public DateTime DateTime { set; get; }
}