<Query Kind="Program" />

void Main()
{
	var date = new DateTime(2017,10,30);
	//var list = GetAcceptanceForUnknown(date);
	var list2 = GetAcceptanceForUnknownFromOal(date);
	
//	foreach (var acceptance in list)
//	{
//		var unknown = new Unknown { ConsignmentNo = acceptance.ConsignmentNo, PupDateTime = acceptance.DateTime, Status = "0", LocationId = acceptance.LocationId};
//		var ok = UpdateUnknown(unknown);
//		Console.WriteLine("\t{0}-{1}", unknown.ConsignmentNo, ok ? "Updated" : "Not Updated");		
//	}

	foreach (var oal in list2)
	{
		DateTime newdate = new DateTime (oal.dt_received_at_oal_date_field.Year,oal.dt_received_at_oal_date_field.Month,oal.dt_received_at_oal_date_field.Day,oal.dt_received_at_oal_date_field.Hour,oal.dt_received_at_oal_date_field.Minute,oal.dt_received_at_oal_date_field.Second);
		var unknown = new Unknown { ConsignmentNo = oal.consignment_no, PupDateTime = newdate , Status = "0", LocationId = oal.location_id };
		var ok = UpdateUnknown(unknown);
		Console.WriteLine("\t{0}-{1}", unknown.ConsignmentNo, ok ? "Updated" : "Not Updated");
	}
	
	//list.Dump();
	list2.Dump();
}

//private List<Acceptance> GetAcceptanceForUnknown(DateTime checkDate)
//{
//	var connString = @"Data Source=10.1.16.124;Initial Catalog=Entt;Application Name=entt;User Id=sa;password=P@ssw0rd";
//	var conn = new SqlConnection(connString);
////	var sql = "SELECT [ConsignmentNo],[DateTime] FROM [Entt].[Acceptance] WHERE ConsignmentNo IN (" +
////				"SELECT [ConsignmentNo] FROM[Entt].[Unknown] WHERE[CreatedDate] >= DATEADD(d, 0, DATEDIFF(d, 0, ('{checkdate}'))) " +
////  				"AND [CreatedDate] < DATEADD(d, 1, DATEDIFF(d, 0, ('{checkdate}'))) AND[Status] = '1' )";
//	var sql = "SELECT [ConsignmentNo],[DateTime],[LocationId] FROM [Entt].[Acceptance] WHERE ConsignmentNo IN (" +
//				"SELECT [ConsignmentNo] FROM[Entt].[Unknown] WHERE [Date] = '{0:ddMMyyyy}' AND [Status] = '1' )";
//	var query = string.Format(sql, checkDate);
//	//Console.WriteLine(query);
//	var list = new List<Acceptance>();
//	using (var cmd = new SqlCommand(query, conn))
//	{
//		if (conn.State != ConnectionState.Open)
//			conn.Open();
//		using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
//		{
//			while (reader.Read())
//			{
//				list.Add(new Acceptance { ConsignmentNo = reader.GetString(0), DateTime = reader.GetDateTime(1), LocationId = reader.GetValue(2).ToString() });
//			}
//		}
//	}
//	return list;
//}

private List<Oal> GetAcceptanceForUnknownFromOal(DateTime checkDate)
{
	var connString = @"Data Source=10.1.16.124;Initial Catalog=Entt;Application Name=entt;User Id=sa;password=P@ssw0rd";
	var conn = new SqlConnection(connString);
	//	var sql = "SELECT [ConsignmentNo],[DateTime] FROM [Entt].[Acceptance] WHERE ConsignmentNo IN (" +
	//				"SELECT [ConsignmentNo] FROM[Entt].[Unknown] WHERE[CreatedDate] >= DATEADD(d, 0, DATEDIFF(d, 0, ('{checkdate}'))) " +
	//  				"AND [CreatedDate] < DATEADD(d, 1, DATEDIFF(d, 0, ('{checkdate}'))) AND[Status] = '1' )";
	var sql = "SELECT [number],[location_id],[dt_received_at_oal_date_field] FROM [192.168.206.31].[oal_dbo].[dbo].[consignment_initial] WHERE number IN (" +
				"SELECT [ConsignmentNo] FROM[Entt].[Unknown] WHERE [Date] = '{0:ddMMyyyy}' AND [Status] = '1' )";
	var query = string.Format(sql, checkDate);
	//Console.WriteLine(query);
	var list2 = new List<Oal>();
	using (var cmd = new SqlCommand(query, conn))
	{
		if (conn.State != ConnectionState.Open)
			conn.Open();
		using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
		{
			while (reader.Read())
			{
				list2.Add(new Oal { consignment_no = reader.GetString(0), location_id = reader.GetValue(1).ToString(), dt_received_at_oal_date_field = reader.GetDateTime(2), });
			}
		}
	}
	return list2;
}

private bool UpdateUnknown(Unknown unknown)
{
	var connString = @"Data Source=10.1.16.124;Initial Catalog=Entt;Application Name=entt;User Id=sa;password=P@ssw0rd";
	var conn = new SqlConnection(connString);
	var code = string.Empty;
	//var query = $"UPDATE [Entt].[Unknown] SET [Status] = '{unknown.Status}', [PupDateTime] = '{unknown.PupDateTime}', [ChangedDate] = GETDATE() WHERE [ConsignmentNo] = '{unknown.ConsignmentNo}' AND [LocationId] = '{unknown.LocationId}'";
	var sql = "UPDATE [Entt].[Unknown] SET [Status] = '{0}', [PupDateTime] = '{1}', [ChangedDate] = GETDATE() WHERE [ConsignmentNo] = '{2}'";
	var query = string.Format(sql, unknown.Status, unknown.PupDateTime, unknown.ConsignmentNo);
	//Console.WriteLine(query);
	using (var connection = new SqlConnection(connString))
	{
		var command = new SqlCommand(query, connection);
		command.Connection.Open();
		command.ExecuteNonQuery();
		return true;
	}

	return false;
}

public class Acceptance
{
	public DateTime DateTime { get; set; }
	public string ConsignmentNo { get; set; }
	public string LocationId { get; set; }
}

public class Oal
{
	public string consignment_no { get; set; }
	public DateTime dt_received_at_oal_date_field { get; set;}
	public string location_id { get; set; }
}

public class Unknown
{
	public DateTime PupDateTime { get; set; }
	public string Status { get; set; }
	public string ConsignmentNo { get; set; }
	public string LocationId { get; set; }
	public string Id { get; set; }
}