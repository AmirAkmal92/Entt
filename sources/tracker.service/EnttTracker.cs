using Bespoke.Sph.Domain;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Bespoke.Entt.Tracker.Service
{
    public class EnttTracker : ITrackerService
    {
        public async Task<int> AddStatusAsync(string hash, string consignmentNo, DateTime? datetime, string eventName)
        {
            using (var conn = new SqlConnection(ConfigurationManager.GetEnvironmentVariable("TrackerConnectionString")))
            using (var cmd = new SqlCommand("[Entt].[usp_add_event_status]", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@connote", SqlDbType.NVarChar, 20).Value = consignmentNo.Trim();
                cmd.Parameters.Add("@eventName", SqlDbType.NVarChar, 30).Value = eventName;
                cmd.Parameters.Add("@hash", SqlDbType.NVarChar, 50).Value = hash;
                cmd.Parameters.Add("@dateTime", SqlDbType.DateTime).Value = datetime;

                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<bool> GetStatusAsync(string hash, string consignmentNo, DateTime? datetime, string eventName)
        {
            //var connectionString = "Data Source=localhost;Initial Catalog=EnttTracker;Integrated Security=true";
            //var queryString = "SELECT [Hash] FROM [dbo].[EventTracking] WHERE [ConsignmentNo] = @connote AND [EventName] = @eventName AND [DateTime] = @dateTime";

            using (var conn = new SqlConnection(ConfigurationManager.GetEnvironmentVariable("TrackerConnectionString")))
            using (var cmd = new SqlCommand("[Entt].[usp_get_event_status]", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@connote", SqlDbType.NVarChar, 20).Value = consignmentNo.Trim();
                cmd.Parameters.Add("@eventName", SqlDbType.NVarChar, 30).Value = eventName;
                cmd.Parameters.Add("@dateTime", SqlDbType.DateTime).Value = datetime;

                await conn.OpenAsync();
                var result = await cmd.ExecuteScalarAsync();

                if (null == result) return false;
                return string.Equals(hash, result.ToString());
            }
        }
    }
}
