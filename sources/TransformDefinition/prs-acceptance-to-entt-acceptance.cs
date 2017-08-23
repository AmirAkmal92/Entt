using System;
using System.Threading.Tasks;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using Bespoke.Sph.Domain;
using Polly;

namespace Bespoke.PosEntt.Integrations.Transforms
{
    public partial class PrsAcceptanceToEnttAcceptance
    {


        partial void BeforeTransform(Bespoke.PosEntt.AcceptanceDatas.Domain.AcceptanceData item, Bespoke.PosEntt.EnttAcceptances.Domain.EnttAcceptance destination)
        {

        }

        partial void AfterTransform(Bespoke.PosEntt.AcceptanceDatas.Domain.AcceptanceData item, Bespoke.PosEntt.EnttAcceptances.Domain.EnttAcceptance destination)
        {
            var pr = Policy.Handle<SqlException>()
                  .WaitAndRetryAsync(3, c => TimeSpan.FromMilliseconds(c * 200))
                  .ExecuteAndCaptureAsync(async () => await DoLookupAsync(item, destination));
    
            pr.Wait();
            if (null != pr.Result.FinalException)
                throw new Exception("Lookup BROM Branch - What the fish happened here?", pr.Result.FinalException);
        
        }
        
        private async Task<bool> DoLookupAsync(Bespoke.PosEntt.AcceptanceDatas.Domain.AcceptanceData item, Bespoke.PosEntt.EnttAcceptances.Domain.EnttAcceptance destination)
        {

            var connectionString = ConfigurationManager.ConnectionStrings["brom"].ConnectionString;
            const string queryString = @"SELECT [BranchCode],[BranchName] FROM [dbo].[BROMBranchProfile] WHERE BranchCostCenter = @branchCode";
            using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                using (var command = new System.Data.SqlClient.SqlCommand(queryString, connection))
                {
                    command.Parameters.Add("@branchCode", SqlDbType.VarChar, 255).Value = item.BranchCode.ToDbNull();
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            destination.LocationId = reader["BranchCode"].ReadNullableString();
                            destination.LocationName = reader["BranchName"].ReadNullableString();
                        }
                    }
                }
            }

            return true;

        }

    }
}
