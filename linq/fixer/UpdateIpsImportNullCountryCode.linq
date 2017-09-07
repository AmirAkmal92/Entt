<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	var date = new DateTime(2017,8,30);
	var list = GetNullCountryCodeList(date);

	var pattern = @"\w{2}\d{9}(?<country>\w{2})";
	foreach (var ips in list)
	{
		var connote = ips.ItemId;
		var match = System.Text.RegularExpressions.Regex.Match(connote, pattern);

		if (match.Success)
		{
			ips.OrigCountryCode = match.Groups["country"].Value;
		}

		if (!string.IsNullOrEmpty(ips.OrigCountryCode))
		{
			await UpdateIpsImport(ips);
			//Console.WriteLine(string.Format("\t{0} - {1}", ips.ItemId, ips.OrigCountryCode));
		}
	}

	Console.WriteLine("\r\nTotal: {0}", list.Count);
	list.Dump();
}

private List<IpsImport> GetNullCountryCodeList(DateTime date)
{
	var connString = @"Data Source=192.168.206.31;Initial Catalog=oal_dbo;Application Name=posentt;User Id=enttuser;password=P@ssw0rd";
	//var connString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=oal;Application Name=entt;Integrated Security=True";
	var conn = new SqlConnection(connString);
	var code = string.Empty;
	var sql = @"SELECT [id],[item_id] FROM [dbo].[ips_import] WHERE [orig_country_cd] IS NULL AND [event_date_local_date_field] > '{0:yyyy-MM-dd}' AND [event_date_local_date_field] < '{1:yyyy-MM-dd}'";
	var query = string.Format(sql, date, date.AddDays(1));
	//Console.WriteLine(query);
	var list = new List<IpsImport>();
	using (var cmd = new SqlCommand(query, conn))
	{
		if (conn.State != ConnectionState.Open)
			conn.Open();
		using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
		{
			while (reader.Read())
			{
				list.Add(new IpsImport { Id = reader.GetString(0), ItemId = reader.GetString(1) });
			}
		}
	}
	return list;
}

public async Task<int> UpdateIpsImport(IpsImport ips)
{
	var connString = @"Data Source=192.168.206.31;Initial Catalog=oal_dbo;Application Name=posentt;User Id=enttuser;password=P@ssw0rd";
	//var connString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=oal;Application Name=entt;Integrated Security=True";
	
	using (var conn = new SqlConnection(connString))
	using (var cmd = new SqlCommand(@"UPDATE [dbo].[ips_import] SET [orig_country_cd] = @orig_country_cd WHERE [id] = @id AND [item_id] = @item_id", conn))
	{
		cmd.Parameters.Add("@orig_country_cd", SqlDbType.VarChar, 5).Value = ips.OrigCountryCode;
		cmd.Parameters.Add("@id", SqlDbType.VarChar, 20).Value = ips.Id;
		cmd.Parameters.Add("@item_id", SqlDbType.VarChar, 50).Value = ips.ItemId;

		await conn.OpenAsync();
		return await cmd.ExecuteNonQueryAsync();
	}
}

public class IpsImport
{
	public string Id { get; set; }
	public string ItemId { get; set; }
	public string OrigCountryCode { get; set; }
}

// Define other methods and classes here