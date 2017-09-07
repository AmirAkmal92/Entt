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
	var checkDate = new DateTime(2017, 8, 15);
	Console.WriteLine("{0:dd-MM-yyyy}\r\n========", checkDate);
	Console.WriteLine("ES: {0} ", elasticSearchUrl);
	var branches = GetAllBranch();
	var reports = new List<DeliveryReport>();

	//var localhostToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyIjoiYWRtaW4iLCJyb2xlcyI6WyJhZG1pbmlzdHJhdG9ycyIsImNhbl9lZGl0X2VudGl0eSIsImNhbl9lZGl0X3dvcmtmbG93IiwiZGV2ZWxvcGVycyJdLCJlbWFpbCI6ImFkbWluQHBvc2VudHQuY29tIiwic3ViIjoiNjM2MzAxMTk0Nzc5MjU5NDcwMGM1Y2Y1MDEiLCJuYmYiOjE1MTAzOTE0NzgsImlhdCI6MTQ5NDQ5Mzg3OCwiZXhwIjoxNTE0Njc4NDAwLCJhdWQiOiJQb3NFbnR0In0.0wfWm2d8Q4CerJqy3YODm5AlblCfwAk2Geo_osQAsPA";
	var productionToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyIjoiYWRtaW4iLCJyb2xlcyI6WyJhZG1pbmlzdHJhdG9ycyIsImNhbl9lZGl0X2VudGl0eSIsImNhbl9lZGl0X3dvcmtmbG93IiwiZGV2ZWxvcGVycyJdLCJlbWFpbCI6ImFkbWluQHBvcy5jb20ubXkiLCJzdWIiOiI2MzYzODY1NDI4NjcxMjkxODFkMTA1Y2VjNSIsIm5iZiI6MTUxODkyNjI4NywiaWF0IjoxNTAzMDI4Njg3LCJleHAiOjE1MDQxMzc2MDAsImF1ZCI6IlBvc0VudHQifQ.0MsE-dh7pAGSAh0Hem_YImYInimvigWz3nJEMDJaG1s";
	//var baseRtsUrl = @"http://localhost:8080/api/rts/";
	var baseRtsUrl = @"http://rx.pos.com.my/api/rts/";
	var token = productionToken;
	Console.WriteLine("RX: {0}\r\n", baseRtsUrl);
	
	foreach (var branch in branches)
	{
		Console.WriteLine("{0} [{1}]", branch.Key, branch.Value);		
		var esDeliveryList = await GetEsDelivery(elasticSearchUrl, indexName, branch.Key, checkDate);
		//esDeliveryList.Dump();
		var oalDeliveryList = GetDeliveryEventNew(branch.Key, checkDate);
		//oalDeliveryList.Dump();

		//var diff = esDeliveryList.Except(oalDeliveryList, new DeliveryComparer());
		//diff.Dump();

		var toDateOalDeliveryList = GetToDateDeliveryEventNew(branch.Key, checkDate);
		var diff2 = esDeliveryList.Except(toDateOalDeliveryList, new DeliveryComparer());
		//diff2.Dump();

		reports.Add(new DeliveryReport { BranchCode = branch.Key, BranchName = branch.Value, TotalEs = esDeliveryList.Count, TotalOal = oalDeliveryList.Count, MissingToDate = diff2.Count()});

		//send data to rx		
		var rtsClient = new HttpClient { BaseAddress = new Uri(baseRtsUrl) };
		rtsClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		rtsClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		
		var sb = new StringBuilder();
		foreach (var deli in diff2)
		{
			var date = DateTime.Parse(deli.Date);
			var time = DateTime.Parse(deli.Time);
			
			var fileName = string.Format("sa_deli_{0:yyyyMMdd}_{1:HHmmss}_{2}.txt", date, time, deli.ScannerId);
			var data = string.Format("{0}\t{1}\t{2}\t{3:ddMMyyyy}\t{4:HHmmss}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}\t{15}\t{16}\t{17}\t{18}\t{19}\t{20}\t{21}\r\n",
					deli.CourierId, deli.LocationId, deli.BeatNo, date, time, deli.ConsignmentNo, deli.ReasonCode, deli.RecipientName.ToNullStringDash(), deli.RecipientIc.ToNullStringDash(), deli.RecipientLocation.ToNullStringDash(), deli.DamageCode.ToNullStringDash(), deli.AuthorizedName.ToNullStringDash(), deli.Comment.ToNullStringDash(), deli.AlternativeAddress.ToNullStringDash(), deli.PaymentType.ToNullStringDash(), deli.PaymentMode.ToNullStringDash(), deli.TotalPayment.ToNullStringDash(), deli.ChequeNo.ToNullStringDash(), deli.BankCode.ToNullStringDash(), deli.DropCode.ToNullStringDash(), deli.LokasiDrop.ToNullStringDash(), deli.CodAccount);
			
			//Console.WriteLine(data);
			sb.Append(data);
			await PostDeliData(rtsClient, fileName, data, productionToken);
		}
		var fn = string.Format("sa_deli_{0}_{1}_{2:yyyyMMdd}.txt", DateTime.Now.ToString("yyyyMMdd_hhmmss"), branch.Key, checkDate);
		File.AppendAllText(@"C:\Users\azrolmn\Desktop\" + fn, sb.ToString());
	}
	
	Console.WriteLine("\r\nSummary Reports\r\n============");
	reports.Dump();
}

private Dictionary<string, string> GetAllBranch()
{
	var list = new Dictionary<string, string>();
//		list.Add("0100", "Pos Laju Kangar");
//		list.Add("0500", "Pos Laju Alor Setar");
//		list.Add("0607", "Pos Laju Jitra");
//		list.Add("0700", "Pos Laju Langkawi");
//		list.Add("0805", "Pos Laju Sg. Petani");
//		list.Add("0901", "Pos Laju Kulim");
//		list.Add("1000", "Pos Laju P.Pinang");
//		list.Add("1200", "Pos Laju Butterworth");
//		list.Add("1321", "Pos Laju Kepala Batas");
//		list.Add("1400", "Pos Laju Bukit Mertajam");
//		list.Add("1500", "Pos Laju Kota Bharu");
//		list.Add("1701", "Pos Laju Pasir Mas");
//		list.Add("1830", "Pos Laju Gua Musang");
//		list.Add("1851", "Pos Laju Machang");
//		list.Add("2000", "Pos Laju K. Terengganu");
//		list.Add("2200", "Pos Laju Jerteh");
//		list.Add("2301", "Pos Laju Dungun");
//		list.Add("2400", "Pos Laju Chukai");
//		list.Add("2510", "Pos Laju Kuantan");
//		list.Add("2662", "Pos Laju Pekan");
//		list.Add("2671", "Pos Laju Muadzam");
//		list.Add("2804", "Pos Laju Temerloh");
//		list.Add("2870", "Pos Laju Bentong");
//		list.Add("3000", "Pos Laju Ipoh");
//		list.Add("3201", "Pos Laju Sitiawan");
//		list.Add("3300", "Pos Laju Kuala Kangsar");
//		list.Add("3400", "Pos Laju Taiping");
//		list.Add("3423", "Pos Laju Parit Buntar");
//		list.Add("3501", "Pos Laju Tapah");
//		list.Add("3591", "Pos Laju Tanjung Malim");
//		list.Add("3600", "Pos Laju Teluk Intan");
//		list.Add("4000", "Pos Laju Shah Alam");
//		list.Add("4100", "Pos Laju Klang");
//		list.Add("4272", "Pos Laju Banting");
//		list.Add("4300", "Pos Laju Bangi");
//		list.Add("4332", "Pos Laju Balakong");
//		list.Add("4399", "Pos Laju Putrajaya");
//		list.Add("4501", "Pos Laju Kuala Selangor");
//		list.Add("4609", "Pos Laju Petaling Jaya");
//    	list.Add("4709", "Pos Laju Kota Damansara");
//		list.Add("4715", "Pos Laju Puchong");
//		list.Add("4802", "Pos Laju Rawang");
//		list.Add("5009", "Pos Laju Brickfields");
//		list.Add("5222", "Pos Laju Kepong");
//		list.Add("5613", "Pos Laju Cheras");
//		list.Add("6101", "Pos Laju Transit Office (KLIA Hub)");
//		list.Add("6815", "Pos Laju Batu Caves");
//		list.Add("7005", "Pos Laju Seremban");
//		list.Add("7102", "Pos Laju Port Dickson");
//		list.Add("7185", "Pos Laju Nilai");
//		list.Add("7214", "Pos Laju Bahau");
//		list.Add("7500", "Pos Laju Melaka");
//		list.Add("7801", "Pos Laju Alor Gajah");
//		list.Add("8135", "Pos Laju Johor Bharu");
//		list.Add("8151", "Pos Laju Pekan Nanas");
//		list.Add("8170", "Pos Laju Pasir Gudang");
//		list.Add("8192", "Pos Laju Kota Tinggi");
//		list.Add("8302", "Pos Laju Batu Pahat");
//		list.Add("8400", "Pos Laju Muar");
//		list.Add("8500", "Pos Laju Segamat");
//		list.Add("8600", "Pos Laju Kluang");
//		list.Add("8609", "Pos Laju Ayer Hitam");
//		list.Add("8681", "Pos Laju Mersing");
//		list.Add("8700", "Pos Laju Labuan");
//		list.Add("8800", "Pos Laju Kota Kinabalu");
//		list.Add("8901", "Pos Laju Keningau");
//		list.Add("9000", "Pos Laju Sandakan");
//		list.Add("9100", "Pos Laju Tawau");
//		list.Add("9110", "Pos Laju Lahad Datu");
//		list.Add("9345", "Pos Laju Kuching");
//		list.Add("9500", "Pos Laju Sri Aman");
//		list.Add("9601", "Pos Laju Sibu");
//		list.Add("9610", "Pos Laju Sarikei");
//		list.Add("9700", "Pos Laju Bintulu");
//		list.Add("9800", "Pos Laju Miri");
//		list.Add("9870", "Pos Laju Limbang");
//	return list;
}

private async Task<List<Delivery>> GetEsDelivery(string elasticSearchUrl, string indexName, string officeNo, DateTime date)
{
	//var esUrl = @"http://10.1.16.110:9200/posentt_rts/";
	var esUrl = String.Concat(elasticSearchUrl, indexName, "/");
	
	var client = new HttpClient { BaseAddress = new Uri(esUrl) };
	client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

	//var content = "{\"_source\":[\"ConsignmentNo\",\"CourierId\",\"Date\",\"Time\"],\"query\":{\"bool\":{\"must\":[{\"range\":{\"CreatedDate\":{\"from\":\"2017-08-15T00:00:00+08:00\",\"to\":\"2017-08-15T23:59:59+08:00\"}}},{\"term\":{\"LocationId\":\"1400\"}}]}},\"from\":0,\"size\":10000,\"sort\":[{\"CreatedDate\":{\"order\":\"desc\"}}]}";
	//var contentFormat = "{\"_source\":[\"ConsignmentNo\",\"CourierId\",\"Date\",\"Time\"],\"query\":{\"bool\":{\"must\":[{\"range\":{\"CreatedDate\":{\"from\":\"[DATE]T00:00:00+08:00\",\"to\":\"[DATE]T23:59:59+08:00\"}}},{\"term\":{\"LocationId\":\"[OFFICENO]\"}}]}},\"from\":0,\"size\":10000,\"sort\":[{\"CreatedDate\":{\"order\":\"desc\"}}]}";
	var contentFormat = "{\"query\":{\"bool\":{\"must\":[{\"range\":{\"Date\":{\"from\":\"[DATE]T00:00:00+08:00\",\"to\":\"[DATE]T23:59:59+08:00\"}}},{\"term\":{\"LocationId\":\"[OFFICENO]\"}}]}},\"from\":0,\"size\":10000,\"sort\":[{\"Date\":{\"order\":\"desc\"}}]}";
	var content = contentFormat.Replace("[OFFICENO]",officeNo).Replace("[DATE]", date.ToString("yyyy-MM-dd"));	
    var request = new StringContent(content);
	request.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

	var response = await client.PostAsync("delivery/_search", request);
	var list = new List<Delivery>();
	
	if (response.StatusCode == System.Net.HttpStatusCode.OK)
	{
		var json = response.Content.ReadAsStringAsync().Result;
		dynamic data = JsonConvert.DeserializeObject(json);
		//Console.WriteLine(data.hits.total);
		var hits = data.hits.hits;

		foreach (var deli in hits)
		{
			list.Add(new Delivery
			{
				CourierId = deli["_source"]["CourierId"],
				LocationId = deli["_source"]["LocationId"],
				BeatNo = deli["_source"]["BeatNo"],
				ConsignmentNo = deli["_source"]["ConsignmentNo"],
				Date = deli["_source"]["Date"],
				Time = deli["_source"]["Time"],
				ReasonCode = deli["_source"]["ReasonCode"],
				RecipientName = deli["_source"]["RecipientName"],
				RecipientIc = deli["_source"]["RecipientIc"],
				RecipientLocation = deli["_source"]["RecipientLocation"],
				DamageCode = deli["_source"]["DamageCode"],
				AuthorizedName = deli["_source"]["AuthorizedName"],
				Comment = deli["_source"]["Comment"],
				AlternativeAddress = deli["_source"]["AlternativeAddress"],
				PaymentType = deli["_source"]["PaymentType"],
				PaymentMode = deli["_source"]["PaymentMode"],
				TotalPayment = deli["_source"]["TotalPayment"],
				ChequeNo = deli["_source"]["ChequeNo"],
				BankCode = deli["_source"]["BankCode"],
				DropCode = deli["_source"]["DropCode"],
				LokasiDrop = deli["_source"]["LokasiDrop"],
				CodAccount = deli["_source"]["CodAccount"],
				ScannerId = deli["_source"]["ScannerId"]
			});
		}
	}
	return list;
}

private List<Delivery> GetDeliveryEventNew(string officeNo, DateTime checkDate)
{
	var startDate = checkDate.Date;
	var endDate = startDate.AddHours(23).AddMinutes(59).AddSeconds(59);
	var connString = @"Data Source=192.168.206.31;Initial Catalog=oal_dbo;Application Name=posentt;User Id=enttuser;password=P@ssw0rd";
	var conn = new SqlConnection(connString);
	var code = string.Empty;
	var sql = @"SELECT DISTINCT [consignment_no], [courier_id] FROM [dbo].[delivery_event_new] WHERE office_no = '{0}' AND date_field > '{1:yyyy-MM-dd HH:mm:ss}' AND date_field < '{2:yyyy-MM-dd HH:mm:ss}'";
	var query = string.Format(sql, officeNo, startDate, endDate);
	//Console.WriteLine(query);
	var list = new List<Delivery>();
	using (var cmd = new SqlCommand(query, conn))
	{
		if (conn.State != ConnectionState.Open)
			conn.Open();
		using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
		{
			while (reader.Read())
			{
				list.Add(new Delivery { ConsignmentNo = reader.GetString(0), CourierId = reader.GetString(1)});
			}
		}
	}
	return list;
}

private List<Delivery> GetToDateDeliveryEventNew(string officeNo, DateTime checkDate)
{
	var connString = @"Data Source=192.168.206.31;Initial Catalog=oal_dbo;Application Name=posentt;User Id=enttuser;password=P@ssw0rd";
	var conn = new SqlConnection(connString);
	var code = string.Empty;
	var sql = @"SELECT DISTINCT [consignment_no], [courier_id] FROM [dbo].[delivery_event_new] WHERE office_no = '{0}' AND date_field > '{1:yyyy-MM-dd HH:mm:ss}'";
	var query = string.Format(sql, officeNo, checkDate.Date);
	//Console.WriteLine(query);
	var list = new List<Delivery>();
	using (var cmd = new SqlCommand(query, conn))
	{
		if (conn.State != ConnectionState.Open)
			conn.Open();
		using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
		{
			while (reader.Read())
			{
				list.Add(new Delivery { ConsignmentNo = reader.GetString(0), CourierId = reader.GetString(1) });
			}
		}
	}
	return list;
}

private async Task PostDeliData(HttpClient client, string filename, string data, string token)
{
	var request = new StringContent(data);
	request.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");
	request.Headers.Add("Name", filename);
	request.Headers.Add("X-Name", filename);
	var response = await client.PostAsync("deli", request);

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

public class Delivery
{
	public string ConsignmentNo { get; set; }
	public string Date { get; set; }
	public string Time { get; set; }
	public string ScannerId { get; set; }
	public string CourierId { get; set; }
	public string LocationId { get; set; }
	public string BeatNo { get; set; }
	public string ReasonCode { get; set; }
	public string RecipientName { get; set; }
	public string RecipientIc { get; set; }
	public string RecipientLocation { get; set; }
	public string DamageCode { get; set; }
	public string AuthorizedName { get; set; }
	public string Comment { get; set; }
	public string AlternativeAddress { get; set; }
	public string PaymentType { get; set; }
	public string PaymentMode { get; set; }
	public string TotalPayment { get; set; }
	public string ChequeNo { get; set; }
	public string BankCode { get; set; }
	public string DropCode { get; set; }
	public string LokasiDrop { get; set; }
	public string CodAccount { get; set; }
}

public class DeliveryReport
{
	public string BranchCode { get; set; }
	public string BranchName { get; set; }
	public int TotalEs { get; set; }
	public int TotalOal { get; set; }
	public int MissingToDate { get; set; }
}

public class DeliveryComparer : IEqualityComparer<Delivery>
{
	public bool Equals(Delivery x, Delivery y)
	{
		return x.ConsignmentNo == y.ConsignmentNo;
	}

	public int GetHashCode(Delivery x)
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