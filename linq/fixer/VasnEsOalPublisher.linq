<Query Kind="Program">
  <Reference>C:\project\work\entt.rts\subscribers.host\Newtonsoft.Json.dll</Reference>
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
	var reports = new List<VasnReport>();

	//var localhostToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyIjoiYWRtaW4iLCJyb2xlcyI6WyJhZG1pbmlzdHJhdG9ycyIsImNhbl9lZGl0X2VudGl0eSIsImNhbl9lZGl0X3dvcmtmbG93IiwiZGV2ZWxvcGVycyJdLCJlbWFpbCI6ImFkbWluQHBvc2VudHQuY29tIiwic3ViIjoiNjM2MzAxMTk0Nzc5MjU5NDcwMGM1Y2Y1MDEiLCJuYmYiOjE1MTAzOTE0NzgsImlhdCI6MTQ5NDQ5Mzg3OCwiZXhwIjoxNTE0Njc4NDAwLCJhdWQiOiJQb3NFbnR0In0.0wfWm2d8Q4CerJqy3YODm5AlblCfwAk2Geo_osQAsPA";
	var productionToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyIjoiYWRtaW4iLCJyb2xlcyI6WyJhZG1pbmlzdHJhdG9ycyIsImNhbl9lZGl0X2VudGl0eSIsImNhbl9lZGl0X3dvcmtmbG93IiwiZGV2ZWxvcGVycyJdLCJlbWFpbCI6ImFkbWluQHBvcy5jb20ubXkiLCJzdWIiOiI2MzYzODY1NDI4NjcxMjkxODFkMTA1Y2VjNSIsIm5iZiI6MTUxODkyNjI4NywiaWF0IjoxNTAzMDI4Njg3LCJleHAiOjE1MDQxMzc2MDAsImF1ZCI6IlBvc0VudHQifQ.0MsE-dh7pAGSAh0Hem_YImYInimvigWz3nJEMDJaG1s";
	//var baseRtsUrl = @"http://localhost:8080/api/rts/";
	var baseRtsUrl = @"http://rx.pos.com.my/api/rts/";
	var token = productionToken;
	Console.WriteLine("RX: {0}\r\n", baseRtsUrl);

	foreach (var branch in branches)
	{
		Console.WriteLine("{0} [{1}]", branch.Key, branch.Value);
		var esVasnList = await GetEsVasn(elasticSearchUrl, indexName, branch.Key, checkDate);
		//esVasnList.Dump();

		var toDateOalDeliveryList = GetToDateDeliveryEventNew(branch.Key, checkDate);
		var diff2 = esVasnList.Except(toDateOalDeliveryList, new VasnComparer());
		//diff2.Dump();

		reports.Add(new VasnReport { BranchCode = branch.Key, BranchName = branch.Value, TotalEs = esVasnList.Count, TotalOal = toDateOalDeliveryList.Count, MissingToDate = diff2.Count() });

		//send data to rx		
		var rtsClient = new HttpClient { BaseAddress = new Uri(baseRtsUrl) };
		rtsClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		rtsClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

		var sb = new StringBuilder();
		foreach (var vasn in diff2)
		{
			var date = DateTime.ParseExact(vasn.Date, "MM/dd/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
			var time = DateTime.ParseExact(vasn.Date, "MM/dd/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

			var fileName = string.Format("sa_vasn_{0:yyyyMMdd}_{1:HHmmss}_{2}.txt", date, time, vasn.ScannerId);
			var data = string.Format("{0}\t{1}\t{2}\t{3:ddMMyyyy}\t{4:HHmmss}\t{5}\t{6}\t{7}\t{8}\r\n",
					vasn.CourierId, vasn.LocationId, vasn.BeatNo, date, time, vasn.ConfirmLocation, vasn.ConsignmentNo, vasn.DestOffice, vasn.Comment);

			//Console.WriteLine(data);
			sb.Append(data);
			await PostVasnData(rtsClient, fileName, data, productionToken);
		}
		var fn = string.Format("sa_vasn_{0}_{1}_{2:yyyyMMdd}.txt", DateTime.Now.ToString("yyyyMMdd_hhmmss"), branch.Key, checkDate);
		File.AppendAllText(@"R:\script sa file\filedeli\Vasn\" + fn, sb.ToString());
	}

	Console.WriteLine("\r\nSummary Reports\r\n============");
	reports.Dump();
}

private async Task PostVasnData(HttpClient client, string filename, string data, string token)
{
	var request = new StringContent(data);
	request.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");
	request.Headers.Add("Name", filename);
	request.Headers.Add("X-Name", filename);
	var response = await client.PostAsync("vasn", request);

	if (response.StatusCode == System.Net.HttpStatusCode.Created)
	{
		var json = response.Content.ReadAsStringAsync().Result;
		var result = JsonConvert.DeserializeObject<PostResult>(json);
		Console.WriteLine(result.message);
	}
}

public class PostResult
{
	public string success { get; set; }
	public string message { get; set; }
	public int rows { get; set; }
	public string[] errors { get; set; }
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

private async Task<List<Vasn>> GetEsVasn(string elasticSearchUrl, string indexName, string officeNo, DateTime date)
{
	var esUrl = String.Concat(elasticSearchUrl, indexName, "/");

	var client = new HttpClient { BaseAddress = new Uri(esUrl) };
	client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

	var contentFormat = "{\"query\":{\"bool\":{\"must\":[{\"range\":{\"Date\":{\"from\":\"[DATE]T00:00:00+08:00\",\"to\":\"[DATE]T23:59:59+08:00\"}}},{\"term\":{\"LocationId\":\"[OFFICENO]\"}}]}},\"from\":0,\"size\":10000,\"sort\":[{\"Date\":{\"order\":\"desc\"}}]}";
	var content = contentFormat.Replace("[OFFICENO]", officeNo).Replace("[DATE]", date.ToString("yyyy-MM-dd"));
	var request = new StringContent(content);
	request.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

	var response = await client.PostAsync("vasn/_search", request);
	var list = new List<Vasn>();

	if (response.StatusCode == System.Net.HttpStatusCode.OK)
	{
		var json = response.Content.ReadAsStringAsync().Result;
		dynamic data = JsonConvert.DeserializeObject(json);
		//Console.WriteLine(data.hits.total);
		var hits = data.hits.hits;

		foreach (var vasn in hits)
		{
			list.Add(new Vasn
			{
				CourierId = vasn["_source"]["CourierId"],
				LocationId = vasn["_source"]["LocationId"],
				BeatNo = vasn["_source"]["BeatNo"],
				Date = vasn["_source"]["Date"],
				Time = vasn["_source"]["Time"],
				ConsignmentNo = vasn["_source"]["ConsignmentNo"],
				ItemType = vasn["_source"]["ItemType"],
				SenderName = vasn["_source"]["SenderName"],
				CodAccount = vasn["_source"]["CodAccount"],
				CodAmount = vasn["_source"]["CodAmount"],
				ScannerId = vasn["_source"]["ScannerId"]
			});
		}
	}
	return list;
}

private List<Vasn> GetToDateDeliveryEventNew(string officeNo, DateTime checkDate)
{
	var connString = @"Data Source=192.168.206.31;Initial Catalog=oal_dbo;Application Name=posentt;User Id=enttuser;password=P@ssw0rd";
	var conn = new SqlConnection(connString);
	var code = string.Empty;
	var sql = @"SELECT DISTINCT [consignment_no], [courier_id] FROM [dbo].[vasn_event_new] WHERE office_no = '{0}' AND date_field > '{1:yyyy-MM-dd HH:mm:ss}'";
	var query = string.Format(sql, officeNo, checkDate.Date);
	//Console.WriteLine(query);
	var list = new List<Vasn>();
	using (var cmd = new SqlCommand(query, conn))
	{
		if (conn.State != ConnectionState.Open)
			conn.Open();
		using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
		{
			while (reader.Read())
			{
				list.Add(new Vasn { ConsignmentNo = reader.GetString(0), CourierId = reader.GetString(1) });
			}
		}
	}
	return list;
}

public class Vasn
{
	public string CourierId { get; set; }
	public string LocationId { get; set; }
	public string BeatNo { get; set; }
	public string Date { get; set; }
	public string Time { get; set; }
	public string ConsignmentNo { get; set; }
	public string ItemType { get; set; }
	public string SenderName { get; set; }
	public string CodAccount { get; set; }
	public string CodAmount { get; set; }
	public string ScannerId { get; set; }
}

public class VasnReport
{
	public string BranchCode { get; set; }
	public string BranchName { get; set; }
	public int TotalEs { get; set; }
	public int TotalOal { get; set; }
	public int MissingToDate { get; set; }
}

public class VasnComparer : IEqualityComparer<Vasn>
{
	public bool Equals(Vasn x, Vasn y)
	{
		return x.ConsignmentNo == y.ConsignmentNo;
	}

	public int GetHashCode(Vasn x)
	{
		return x.ConsignmentNo.GetHashCode();
	}
}

public static class StringHelper
{
	public static string ToNullStringDash(this string val)
	{
		return !string.IsNullOrEmpty(val) ? val : "-";
	}
}