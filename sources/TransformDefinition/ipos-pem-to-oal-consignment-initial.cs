using System;
using System.Threading.Tasks;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using Bespoke.Sph.Domain;
using Polly;

namespace Bespoke.PosEntt.Integrations.Transforms
{
    public partial class IposPemToOalConsignmentInitial
    {


        partial void BeforeTransform(Bespoke.PosEntt.IposPems.Domain.IposPem item, Bespoke.PosEntt.Adapters.Oal.UspConsigmentInitialRtsRequest destination)
        {

        }

        partial void AfterTransform(Bespoke.PosEntt.IposPems.Domain.IposPem item, Bespoke.PosEntt.Adapters.Oal.UspConsigmentInitialRtsRequest destination)
        {
            if (null != item.BabyConnoteNo)
            {
                var pr = Policy.Handle<SqlException>()
                  .WaitAndRetryAsync(3, c => TimeSpan.FromMilliseconds(c * 200))
                  .ExecuteAndCaptureAsync(async () => await DoLookupAsync(item, destination));
    
                pr.Wait();
                if (null != pr.Result.FinalException)
                    throw new Exception("What the fish happened here", pr.Result.FinalException);
            }
        }
        
        private async Task<bool> DoLookupAsync(Bespoke.PosEntt.IposPems.Domain.IposPem item, Bespoke.PosEntt.Adapters.Oal.UspConsigmentInitialRtsRequest destination)
        {

            var connectionString = ConfigurationManager.ConnectionStrings["oal"].ConnectionString;
            const string queryString = @"SELECT [id] FROM [dbo].[consignment_initial]  WHERE [number] = @babyConsignmentNo";
            using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                using (var command = new System.Data.SqlClient.SqlCommand(queryString, connection))
                {
                    command.Parameters.Add("@babyConsignmentNo", SqlDbType.VarChar, 255).Value = item.BabyConnoteNo.ToDbNull();
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            destination.baby_item = reader["id"].ReadNullableString();
                            destination.parent = reader["id"].ReadNullableString();
                        }
                    }
                }
            }

            return true;

        }

    }
}
