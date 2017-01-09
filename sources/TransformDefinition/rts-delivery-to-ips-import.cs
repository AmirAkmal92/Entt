using Polly;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using Bespoke.Sph.Domain;


namespace Bespoke.PosEntt.Integrations.Transforms
{
    public partial class RtsDeliveryToIpsImport
    {


        partial void BeforeTransform(Bespoke.PosEntt.Deliveries.Domain.Delivery item, Bespoke.PosEntt.Adapters.Oal.dbo_ips_import destination)
        {

        }

        partial void AfterTransform(Bespoke.PosEntt.Deliveries.Domain.Delivery item, Bespoke.PosEntt.Adapters.Oal.dbo_ips_import destination)
        {
        	var pr = Policy.Handle<SqlException>()
        	  .WaitAndRetryAsync(5, c =>TimeSpan.FromMilliseconds(c * 500))
        	  .ExecuteAndCaptureAsync(async () => await DoLookupAsync(item, destination));
        
        	pr.Wait();
        	if (null != pr.Result.FinalException)
        		throw new Exception("What the hell happened here", pr.Result.FinalException);
        }
        
        // Define other methods and classes here
        private async Task<bool> DoLookupAsync(Bespoke.PosEntt.Deliveries.Domain.Delivery item, Bespoke.PosEntt.Adapters.Oal.dbo_ips_import destination)
        {
        
        	Func<string, string> SetContent = (content) =>
        		  {
        			  return content.Equals("01") ? "M" : "D";
        		  };
        
        
        	Func<string, string> SetOriginCountryCode = (origin) =>
        	{
        		if (!origin.Equals("-"))
        		{
        			return origin;
        		}
        		else
        		{
        			var pattern = @"\w{2}\d{9}(?<country>\w{2})";
        			var match = System.Text.RegularExpressions.Regex.Match(item.ConsignmentNo, pattern);
        			return match.Success ? match.Groups["country"].Value : "-";
        		}
        	};
        	var config = ConfigurationManager.ConnectionStrings["oal"].ConnectionString;
        	var connectionString = @config;
        	const string queryString = @"SELECT [item_category], [shipper_address_country], [weight_double] FROM [dbo].[consignment_initial]  WHERE [number] = @consignmentNo";
        	using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
        	{
        		using (var command = new System.Data.SqlClient.SqlCommand(queryString, connection))
        		{
        			command.Parameters.Add("@consignmentNo", SqlDbType.VarChar, 255).Value =  item.ConsignmentNo.ToDbNull();
        			await connection.OpenAsync();
        			using (var reader = await command.ExecuteReaderAsync())
        			{
        				while (await reader.ReadAsync())
        				{
        					destination.content = SetContent((string)reader["item_category"]);
        					destination.orig_country_cd = SetOriginCountryCode((string)reader["shipper_address_country"]);
        					destination.item_weight_double = (double)reader["weight_double"];
        
        				}
        
        			}
        		}
        
        	}
        
        	return true;
        
        
        }
    

    }

}
