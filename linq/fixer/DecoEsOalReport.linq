<Query Kind="Program">
  <Reference Relative="..\..\subscribers.host\Newtonsoft.Json.dll">C:\project\work\entt.rts\subscribers.host\Newtonsoft.Json.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>System.Linq</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Net.Http.Headers</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	var elasticSearchUrl = "http://10.1.16.110:9200/";
	var indexName = "posentt_rts";
	var checkDate = new DateTime(2017, 8, 23);
	Console.WriteLine("{0:dd-MM-yyyy}\r\n========", checkDate);

	var branches = GetAllBranch();
	var reports = new List<DeliveryReport>();
	
	foreach (var branch in branches)
	{
		Console.WriteLine("{0} [{1}]", branch.Key, branch.Value);		
		
		var esDecoList = await GetEsDeco(elasticSearchUrl, indexName, branch.Key, checkDate);
		//esDecoList.Dump();

		var oalDecoList = GetConsoleDetails(branch.Key, checkDate);
		//oalDecoList.Dump();

		var toDateOalDecoList = GetToDateConsoleDetails(branch.Key, checkDate);
		var diff2 = esDecoList.Except(toDateOalDecoList, new DecoComparer());
		diff2.Dump();

		reports.Add(new DeliveryReport { BranchCode = branch.Key, BranchName = branch.Value, TotalEs = esDecoList.Count, TotalOal = oalDecoList.Count, MissingToDate = diff2.Count()});	
		
	}
	
	Console.WriteLine("\r\nSummary Reports\r\n============");
	reports.Dump();
}

private Dictionary<string, string> GetAllBranch()
{
	var list = new Dictionary<string, string>();
//	list.Add("0100", "Pos Laju Kangar");
//	list.Add("0500", "Pos Laju Alor Setar");
//	list.Add("0607", "Pos Laju Jitra");
//	list.Add("0700", "Pos Laju Langkawi");
//	list.Add("0805", "Pos Laju Sg. Petani");
//	list.Add("0901", "Pos Laju Kulim");
//	list.Add("1000", "Pos Laju P.Pinang");
//	list.Add("1200", "Pos Laju Butterworth");
//	list.Add("1321", "Pos Laju Kepala Batas");
//	list.Add("1400", "Pos Laju Bukit Mertajam");
//	list.Add("1500", "Pos Laju Kota Bharu");
//	list.Add("1701", "Pos Laju Pasir Mas");
//	list.Add("1830", "Pos Laju Gua Musang");
//	list.Add("1851", "Pos Laju Machang");
//	list.Add("2000", "Pos Laju K. Terengganu");
//	list.Add("2200", "Pos Laju Jerteh");
//	list.Add("2301", "Pos Laju Dungun");
//	list.Add("2400", "Pos Laju Chukai");
//	list.Add("2510", "Pos Laju Kuantan");
//	list.Add("2662", "Pos Laju Pekan");
//	list.Add("2671", "Pos Laju Muadzam");
//	list.Add("2804", "Pos Laju Temerloh");
//	list.Add("2870", "Pos Laju Bentong");
//	list.Add("3000", "Pos Laju Ipoh");
//	list.Add("3201", "Pos Laju Sitiawan");
//	list.Add("3300", "Pos Laju Kuala Kangsar");
//	list.Add("3400", "Pos Laju Taiping");
//	list.Add("3423", "Pos Laju Parit Buntar");
//	list.Add("3501", "Pos Laju Tapah");
//	list.Add("3591", "Pos Laju Tanjung Malim");
//	list.Add("3600", "Pos Laju Teluk Intan");
//	list.Add("4000", "Pos Laju Shah Alam");
//	list.Add("4100", "Pos Laju Klang");
//	list.Add("4272", "Pos Laju Banting");
//	list.Add("4300", "Pos Laju Bangi");
//	list.Add("4332", "Pos Laju Balakong");
//	list.Add("4399", "Pos Laju Putrajaya");
//	list.Add("4501", "Pos Laju Kuala Selangor");
//	list.Add("4609", "Pos Laju Petaling Jaya");
//	list.Add("4709", "Pos Laju Kota Damansara");
//	list.Add("4715", "Pos Laju Puchong");
//	list.Add("4802", "Pos Laju Rawang");
//	list.Add("5009", "Pos Laju Brickfields");
//	list.Add("5222", "Pos Laju Kepong");
//	list.Add("5613", "Pos Laju Cheras");
//	list.Add("6101", "Pos Laju Transit Office (KLIA Hub)");
//	list.Add("6815", "Pos Laju Batu Caves");
//	list.Add("7005", "Pos Laju Seremban");
//	list.Add("7102", "Pos Laju Port Dickson");
//	list.Add("7185", "Pos Laju Nilai");
//	list.Add("7214", "Pos Laju Bahau");
//	list.Add("7500", "Pos Laju Melaka");
//	list.Add("7801", "Pos Laju Alor Gajah");
//	list.Add("8135", "Pos Laju Johor Bharu");
//	list.Add("8151", "Pos Laju Pekan Nanas");
//	list.Add("8170", "Pos Laju Pasir Gudang");
//	list.Add("8192", "Pos Laju Kota Tinggi");
//	list.Add("8302", "Pos Laju Batu Pahat");
//	list.Add("8400", "Pos Laju Muar");
//	list.Add("8500", "Pos Laju Segamat");
//	list.Add("8600", "Pos Laju Kluang");
//	list.Add("8609", "Pos Laju Ayer Hitam");
//	list.Add("8681", "Pos Laju Mersing");
//	list.Add("8700", "Pos Laju Labuan");
//	list.Add("8800", "Pos Laju Kota Kinabalu");
//	list.Add("8901", "Pos Laju Keningau");
//	list.Add("9000", "Pos Laju Sandakan");
//	list.Add("9100", "Pos Laju Tawau");
//	list.Add("9110", "Pos Laju Lahad Datu");
//	list.Add("9345", "Pos Laju Kuching");
//	list.Add("9500", "Pos Laju Sri Aman");
//	list.Add("9601", "Pos Laju Sibu");
//	list.Add("9610", "Pos Laju Sarikei");
//	list.Add("9700", "Pos Laju Bintulu");
//	list.Add("9800", "Pos Laju Miri");
//	list.Add("9870", "Pos Laju Limbang");
	return list;
}

private async Task<List<Deco>> GetEsDeco(string elasticSearchUrl, string indexName, string officeNo, DateTime date)
{
	var esUrl = String.Concat(elasticSearchUrl, indexName, "/");
	
	var client = new HttpClient { BaseAddress = new Uri(esUrl) };
	client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

	var contentFormat = "{\"query\":{\"bool\":{\"must\":[{\"range\":{\"Date\":{\"from\":\"[DATE]T00:00:00+08:00\",\"to\":\"[DATE]T23:59:59+08:00\"}}},{\"term\":{\"LocationId\":\"[OFFICENO]\"}}]}},\"from\":0,\"size\":10000,\"sort\":[{\"CreatedDate\":{\"order\":\"desc\"}}]}";
	var content = contentFormat.Replace("[OFFICENO]",officeNo).Replace("[DATE]", date.ToString("yyyy-MM-dd"));	
    var request = new StringContent(content);
	request.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

	var response = await client.PostAsync("deco/_search", request);
	var list = new List<Deco>();
	
	if (response.StatusCode == System.Net.HttpStatusCode.OK)
	{
		var json = response.Content.ReadAsStringAsync().Result;
		dynamic data = JsonConvert.DeserializeObject(json);
		//Console.WriteLine(data.hits.total);
		var hits = data.hits.hits;

		foreach (var deli in hits)
		{
			list.Add(new Deco
			{
				CourierId = deli["_source"]["CourierId"],
				LocationId = deli["_source"]["LocationId"],
				BeatNo = deli["_source"]["BeatNo"],
				Date = deli["_source"]["Date"],
				Time = deli["_source"]["Time"],
				ConsoleTag = deli["_source"]["ConsoleTag"],
				Comment = deli["_source"]["Comment"],
				AllConsignmentnNotes = deli["_source"]["AllConsignmentnNotes"],
				TotalConsignment = deli["_source"]["TotalConsignment"],
				ScannerId = deli["_source"]["ScannerId"]
			});
		}
	}
	return list;
}

private List<Deco> GetConsoleDetails(string officeNo, DateTime checkDate)
{
	var startDate = checkDate.Date;
	var endDate = startDate.AddHours(23).AddMinutes(59).AddSeconds(59);
	var connString = @"Data Source=192.168.206.31;Initial Catalog=oal_dbo;Application Name=posentt;User Id=enttuser;password=P@ssw0rd";
	var conn = new SqlConnection(connString);
	var code = string.Empty;
	var sql = @"SELECT DISTINCT [console_no], [courier_id] FROM [dbo].[console_details] WHERE office_no = '{0}' AND date_field > '{1:yyyy-MM-dd HH:mm:ss}' AND date_field < '{2:yyyy-MM-dd HH:mm:ss}'";
	var query = string.Format(sql, officeNo, startDate, endDate);
	//Console.WriteLine(query);
	var list = new List<Deco>();
	using (var cmd = new SqlCommand(query, conn))
	{
		if (conn.State != ConnectionState.Open)
			conn.Open();
		using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
		{
			while (reader.Read())
			{
				list.Add(new Deco { ConsoleTag = reader.GetString(0), CourierId = reader.GetString(1)});
			}
		}
	}
	return list;
}

private List<Deco> GetToDateConsoleDetails(string officeNo, DateTime checkDate)
{
	var connString = @"Data Source=192.168.206.31;Initial Catalog=oal_dbo;Application Name=posentt;User Id=enttuser;password=P@ssw0rd";
	var conn = new SqlConnection(connString);
	var code = string.Empty;
	var sql = @"SELECT DISTINCT [console_no], [courier_id] FROM [dbo].[console_details] WHERE office_no = '{0}' AND date_field > '{1:yyyy-MM-dd HH:mm:ss}'";
	var query = string.Format(sql, officeNo, checkDate.Date.AddDays(-1));
	//Console.WriteLine(query);
	var list = new List<Deco>();
	using (var cmd = new SqlCommand(query, conn))
	{
		if (conn.State != ConnectionState.Open)
			conn.Open();
		using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
		{
			while (reader.Read())
			{
				list.Add(new Deco { ConsoleTag = reader.GetString(0), CourierId = reader.GetString(1) });
			}
		}
	}
	return list;
}

public class Deco
{

	public string CourierId { get; set; }
	public string LocationId { get; set; }
	public string BeatNo { get; set; }
	public string Date { get; set; }
	public string Time { get; set; }
	public string ConsoleTag { get; set; }
	public string Comment { get; set; }
	public string AllConsignmentnNotes { get; set; }
	public string TotalConsignment { get; set; }
	public string ScannerId { get; set; }
}

public class DeliveryReport
{
	public string BranchCode { get; set; }
	public string BranchName { get; set; }
	public int TotalEs { get; set; }
	public int TotalOal { get; set; }
	public int MissingToDate { get; set; }
}

public class DecoComparer : IEqualityComparer<Deco>
{
	public bool Equals(Deco x, Deco y)
	{
		return x.ConsoleTag == y.ConsoleTag;
	}

	public int GetHashCode(Deco x)
	{
		return x.ConsoleTag.GetHashCode();
	}
}