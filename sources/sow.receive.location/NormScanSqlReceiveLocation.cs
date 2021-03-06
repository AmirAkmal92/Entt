using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Bespoke.Sph.Domain;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.PosEntt.ReceivePorts;
using Dapper;
using FileHelpers;
using Polly;
using System.Linq;

namespace Bespoke.PosEntt.ReceiveLocations
{
    public class NormScanSqlReceiveLocation : IReceiveLocation, IDisposable
    {
        private Timer m_timer;
        private HttpClient m_client;
        private bool m_paused;


        private async void PollNormTableFired(object stateInfo)
        {
            if (m_paused) return;

            Console.WriteLine("now running Norm...");
            var logger = new LocationLogger();
            var number = 0;
            var records = await ReadNormAsync();
            Console.WriteLine("Found: {0} records", records.ToList().Count);
            
            foreach (var r in records)
            {
                number++;
                if (null == r) continue; // we got an exception reading the record

                //sow_Norm_<locationid>_<date>-<time>_<linecount>_<staffid>
                var filename = string.Format("sow_Norm_{0}_{1}_{2}_{3}", r.location_id, r.date_time.ToString("yyyyMMdd-HHmm"),1,r.courier_id);

                // polly policy goes here
                var retry = ConfigurationManager.GetEnvironmentVariableInt32($"{nameof(Norm)}Retry", 3);
                var interval = ConfigurationManager.GetEnvironmentVariableInt32($"{nameof(Norm)}Internal", 100);

                var pr = await Policy.Handle<Exception>()
                                    .WaitAndRetryAsync(retry, c => TimeSpan.FromMilliseconds(interval * Math.Pow(2, c)))
                                    .ExecuteAndCaptureAsync(() =>
                    {

                        var text = r.ToString();
                        var request = new StringContent(text);
                        request.Headers.Add("X-Name", filename);
                        request.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");
                        request.Headers.Add("Source", "sow");
                        return m_client.PostAsync("/api/rts/norm", request);
                    });

                if (null != pr.FinalException)
                {
                    logger.Log(new LogEntry(pr.FinalException) { Message = $"Line {number}" });
                    continue;
                }
                var response = pr.Result;
                Console.WriteLine($"\t{r.console_no} : {response.StatusCode}\t");
                logger.Log(new LogEntry { Message = $"Record: {number}({r.id}) , StatusCode: {(int)response.StatusCode}", Severity = Severity.Debug });

                if (!response.IsSuccessStatusCode)
                {
                    var warn = new LogEntry
                    {
                        Message = $"Non success status code record {number}({r.id}), StatusCode: {(int)response.StatusCode}",
                        Severity = Severity.Warning
                    };
                    logger.Log(warn);
                }
                else
                {
                    await DeleteNormRowAsync(r);
                }
            }

            Console.WriteLine();
            logger.Log(new LogEntry { Message = $"Done processing with {number} record(s)", Severity = Severity.Info });

        }

        public async Task<int> InsertAsync(Norm item)
        {
            using (var conn = new SqlConnection(ConfigurationManager.GetEnvironmentVariable("OalConnectionString")))
            using (var cmd = new SqlCommand(@"INSERT INTO [dbo].[Norm_log] (
                       [version],
                       [beat_no],
                       [comment],
                       [console_no],
                       [console_type_code],
                       [courier_id],
                       [data_entry_beat_no],
                       [data_entry_location_id],
                       [data_entry_staff_id],
                       [date_created],
                       [date_created_ori],
                       [date_generated],
                       [date_time],
                       [filename],
                       [last_updated],
                       [location_id],
                       [next_location],
                       [other_console_type],
                       [routing_code],
                       [status],
                       [all_connotes]
                )
                VALUES(
                       @version,
                       @beat_no,
                       @comment,
                       @console_no,
                       @console_type_code,
                       @courier_id,
                       @data_entry_beat_no,
                       @data_entry_location_id,
                       @data_entry_staff_id,
                       @date_created,
                       @date_created_ori,
                       @date_generated,
                       @date_time,
                       @filename,
                       @last_updated,
                       @location_id,
                       @next_location,
                       @other_console_type,
                       @routing_code,
                       @status,
                       @all_connotes
            )", conn))
            {
                await conn.OpenAsync();
                cmd.Parameters.Add("@beat_no", SqlDbType.VarChar, 3).Value = item.beat_no;
                cmd.Parameters.Add("@comment", SqlDbType.VarChar, 255).Value = item.comment;
                cmd.Parameters.Add("@console_no", SqlDbType.VarChar, 40).Value = item.console_no;
                cmd.Parameters.Add("@console_type_code", SqlDbType.VarChar, 255).Value = item.console_type_code;
                cmd.Parameters.Add("@date_created", SqlDbType.DateTime, 8).Value = DateTime.Now;
                cmd.Parameters.Add("@date_time", SqlDbType.DateTime, 8).Value = item.date_time;
                cmd.Parameters.Add("@date_created_ori", SqlDbType.DateTime, 8).Value = item.date_created;
                cmd.Parameters.Add("@date_generated", SqlDbType.DateTime, 8).Value = DateTime.Now;
                cmd.Parameters.Add("@last_updated", SqlDbType.DateTime).Value = item.last_updated;
                cmd.Parameters.Add("@filename", SqlDbType.VarChar, 255).Value = item.filename;
                cmd.Parameters.Add("@courier_id", SqlDbType.VarChar, 255).Value = item.courier_id;
                cmd.Parameters.Add("@data_entry_beat_no", SqlDbType.VarChar, 3).Value = item.data_entry_beat_no;
                cmd.Parameters.Add("@data_entry_location_id", SqlDbType.VarChar, 4).Value = item.data_entry_location_id;
                cmd.Parameters.Add("@data_entry_staff_id", SqlDbType.VarChar, 255).Value = item.data_entry_staff_id;
                cmd.Parameters.Add("@location_id", SqlDbType.VarChar, 4).Value = item.location_id;
                cmd.Parameters.Add("@next_location", SqlDbType.VarChar, 255).Value = item.next_location;
                cmd.Parameters.Add("@other_console_type", SqlDbType.VarChar, 255).Value = item.other_console_type;
                cmd.Parameters.Add("@routing_code", SqlDbType.VarChar, 255).Value = item.routing_code;
                cmd.Parameters.Add("@all_connotes", SqlDbType.VarChar, 2000).Value = item.all_connotes;
                cmd.Parameters.Add("@status", SqlDbType.VarChar, 255).Value = item.status;
                cmd.Parameters.Add("@version", SqlDbType.Int, 19).Value = item.version;
                return await cmd.ExecuteNonQueryAsync();
            }
        }


        private async Task DeleteNormRowAsync(Norm item)
        {
            using (var conn = new SqlConnection(ConfigurationManager.GetEnvironmentVariable("OalConnectionString")))
            //using (var conn = new SqlConnection(ConfigurationManager.GetEnvironmentVariable("ConnectionString")))
            using (SqlCommand cmd = new SqlCommand("DELETE FROM dbo.Norm WHERE Id=@Id", conn))
            {
                cmd.Parameters.Add("@id", SqlDbType.Int, 4).Value = item.id;
                await conn.OpenAsync().ConfigureAwait(false);

            }
        }
        private async Task<IEnumerable<Norm>> ReadNormAsync()
        {
            using (var conn = new SqlConnection(ConfigurationManager.GetEnvironmentVariable("OalConnectionString")))
            //using (var conn = new SqlConnection(ConfigurationManager.GetEnvironmentVariable("ConnectionString")))

            {
                await conn.OpenAsync().ConfigureAwait(false);
                return await conn.QueryAsync<Norm>("SELECT TOP 5 * FROM [dbo].[Norm] WHERE [status] = '0'");
            }
        }


        public async Task WaitReadyAsync(string fileName)
        {
            while (true)
            {
                try
                {
                    using (Stream stream = File.Open(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                        if (stream != null)
                        {
                            Console.WriteLine($"Output file {fileName} ready.");
                            break;
                        }
                    }
                }
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine($"Output file {fileName} not yet ready ({ex.Message})");
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Output file {fileName} not yet ready ({ex.Message})");
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine($"Output file {fileName} not yet ready ({ex.Message})");
                }
                await Task.Delay(500);
            }
        }
        public bool Start()
        {

            var token = ConfigurationManager.GetEnvironmentVariable("SowRtsJwtToken");
            var dueTime = ConfigurationManager.GetEnvironmentVariableInt32("SowRtsDueTime", 10 * 1000);
            var period = ConfigurationManager.GetEnvironmentVariableInt32("SowRtsPeriod", 5 * 60 * 1000);
            m_client = new HttpClient { BaseAddress = new Uri(ConfigurationManager.BaseUrl) };
            m_client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var flag = new AutoResetEvent(false);
            m_timer = new Timer(PollNormTableFired, flag, dueTime, period);

            return true;


        }

        public bool Stop() { this.Dispose(); return true; }



        public void Dispose()
        {
            m_timer?.Dispose();
            m_timer = null;
            m_client?.Dispose();
            m_client = null;
        }

        public void Pause()
        {
            m_paused = true;
        }

        public void Resume()
        {
            m_paused = false;
        }
    }
}
