using Polly;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using Bespoke.Sph.Domain;

namespace Bespoke.PosEntt.Integrations.Transforms
{
    public partial class RtsPickupToOalIpsImport
    {
        partial void BeforeTransform(Bespoke.PosEntt.Pickups.Domain.Pickup item, Bespoke.PosEntt.Adapters.Oal.dbo_ips_import destination)
        {

        }

        partial void AfterTransform(Bespoke.PosEntt.Pickups.Domain.Pickup item, Bespoke.PosEntt.Adapters.Oal.dbo_ips_import destination)
        {
        	var pr = Policy.Handle<SqlException>()
              .WaitAndRetryAsync(3, c => TimeSpan.FromMilliseconds(c * 200))
              .ExecuteAndCaptureAsync(async () => await DoLookupAsync(item, destination));

            pr.Wait();
            if (null != pr.Result.FinalException)
                throw new Exception("Pickup Ips import result null", pr.Result.FinalException);
        }

        private async Task<bool> DoLookupAsync(Bespoke.PosEntt.Pickups.Domain.Pickup item, Bespoke.PosEntt.Adapters.Oal.dbo_ips_import destination)
        {

            Func<string, string> SetOriginCountryCode = (origin) =>
            {
                if (!string.IsNullOrWhiteSpace(origin))
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
            const string queryString = @"SELECT [shipper_address_country] FROM [dbo].[consignment_initial]  WHERE [number] = @consignmentNo";
            using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                using (var command = new System.Data.SqlClient.SqlCommand(queryString, connection))
                {
                    command.Parameters.Add("@consignmentNo", SqlDbType.VarChar, 255).Value = item.ConsignmentNo.ToDbNull();
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            destination.orig_country_cd = SetOriginCountryCode(reader["shipper_address_country"].ReadNullableString());
                        }
                    }
                }

            }

            if (null == destination.item_weight_double) destination.item_weight_double = 0d;
            return true;

        }

    }
}
