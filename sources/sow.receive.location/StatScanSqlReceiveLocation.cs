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
    public class StatScanSqlReceiveLocation : IReceiveLocation, IDisposable
    {
        private Timer m_timer;
        private HttpClient m_client;
        private bool m_paused;


        private async void PollStatTableFired(object stateInfo)
        {
            if (m_paused) return;

            Console.WriteLine("now running Stat...");
            var logger = new LocationLogger();
            var number = 0;
            var records = await ReadStatAsync();
            Console.WriteLine("Found: {0} records", records.ToList().Count);
            
            foreach (var r in records)
            {
                number++;
                if (null == r) continue; // we got an exception reading the record

                //sow_Stat_<locationid>_<date>-<time>_<linecount>_<staffid>
                var filename = string.Format("sow_Stat_{0}_{1}_{2}_{3}", r.location_id, r.date_time.ToString("yyyyMMdd-HHmm"),1,r.courier_id);

                // polly policy goes here
                var retry = ConfigurationManager.GetEnvironmentVariableInt32($"{nameof(Stat)}Retry", 3);
                var interval = ConfigurationManager.GetEnvironmentVariableInt32($"{nameof(Stat)}Internal", 100);

                var pr = await Policy.Handle<Exception>()
                                    .WaitAndRetryAsync(retry, c => TimeSpan.FromMilliseconds(interval * Math.Pow(2, c)))
                                    .ExecuteAndCaptureAsync(() =>
                    {

                        var text = r.ToString();
                        var request = new StringContent(text);
                        request.Headers.Add("X-Name", filename);
                        request.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");
                        request.Headers.Add("Source", "sow");
                        return m_client.PostAsync("/api/rts/Stat", request);
                    });

                if (null != pr.FinalException)
                {
                    logger.Log(new LogEntry(pr.FinalException) { Message = $"Line {number}" });
                    continue;
                }
                var response = pr.Result;
                Console.WriteLine($"\t{r.consignment_no} : {response.StatusCode}\t");
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
                    await DeleteStatRowAsync(r);
                }
            }

            Console.WriteLine();
            logger.Log(new LogEntry { Message = $"Done processing with {number} record(s)", Severity = Severity.Info });

        }

        public async Task<int> InsertAsync(Stat item)
        {
            using (var conn = new SqlConnection(ConfigurationManager.GetEnvironmentVariable("OalConnectionString")))
            using (var cmd = new SqlCommand(@"INSERT INTO [dbo].[Stat_log] (
                      [version],
                      [beat_no],
                      [comment],
                      [consignment_no],
                      [courier_id],
                      [data_entry_beat_no],
                      [data_entry_location_id],
                      [data_entry_staff_id],
                      [date_created],
                      [date_generated],
                      [date_time],
                      [filename],
                      [last_updated],
                      [location_id],
                      [status_code],
                      [date_created_ori],
                      [status]
                                )
                VALUES(
                      @version,
                      @beat_no,
                      @comment,
                      @consignment_no,
                      @courier_id,
                      @data_entry_beat_no,
                      @data_entry_location_id,
                      @data_entry_staff_id,
                      @date_created,
                      @date_generated,
                      @date_time,
                      @filename,
                      @last_updated,
                      @location_id,
                      @status_code,
                      @date_created_ori,
                      @status,
            )", conn))
            {
                await conn.OpenAsync();
                cmd.Parameters.Add("@beat_no", SqlDbType.VarChar, 3).Value = item.beat_no;
                cmd.Parameters.Add("@courier_id", SqlDbType.VarChar, 255).Value = item.courier_id;
                cmd.Parameters.Add("@comment", SqlDbType.VarChar, 250).Value = item.comment;
                cmd.Parameters.Add("@consignment_no", SqlDbType.VarChar, 40).Value = item.consignment_no;
                cmd.Parameters.Add("data_entry_location_id", SqlDbType.VarChar, 4).Value = item.data_entry_location_id;
                cmd.Parameters.Add("@data_entry_staff_id", SqlDbType.VarChar, 255).Value = item.data_entry_staff_id;
                cmd.Parameters.Add("@last_updated", SqlDbType.DateTime, 8).Value = item.last_updated;
                cmd.Parameters.Add("@filename", SqlDbType.VarChar, 255).Value = item.filename;
                cmd.Parameters.Add("@location_id", SqlDbType.VarChar, 4).Value = item.location_id;
                cmd.Parameters.Add("@status", SqlDbType.VarChar, 255).Value = item.status;
                cmd.Parameters.Add("@data_entry_beat_no", SqlDbType.VarChar, 3).Value = item.data_entry_beat_no;
                cmd.Parameters.Add("@status_code", SqlDbType.VarChar, 2000).Value = item.status_code;
                cmd.Parameters.Add("@version", SqlDbType.Int, 19).Value = item.version;
                cmd.Parameters.Add("@date_time", SqlDbType.DateTime, 8).Value = item.date_time;
                cmd.Parameters.Add("date_created", SqlDbType.DateTime, 8).Value = DateTime.Now;
                cmd.Parameters.Add("@date_created_ori", SqlDbType.DateTime, 8).Value = item.date_created;
                cmd.Parameters.Add("@date_generated", SqlDbType.DateTime, 8).Value = DateTime.Now;
                cmd.Parameters.Add("@last_updated", SqlDbType.DateTime, 8).Value = item.last_updated;
                return await cmd.ExecuteNonQueryAsync();
            }
        }


        private async Task DeleteStatRowAsync(Stat item)
        {
            using (var conn = new SqlConnection(ConfigurationManager.GetEnvironmentVariable("OalConnectionString")))
            //using (var conn = new SqlConnection(ConfigurationManager.GetEnvironmentVariable("ConnectionString")))
            using (SqlCommand cmd = new SqlCommand("DELETE FROM dbo.Stat WHERE Id=@Id", conn))
            {
                cmd.Parameters.Add("@id", SqlDbType.Int, 4).Value = item.id;
                await conn.OpenAsync().ConfigureAwait(false);

            }
        }
        private async Task<IEnumerable<Stat>> ReadStatAsync()
        {
            using (var conn = new SqlConnection(ConfigurationManager.GetEnvironmentVariable("OalConnectionString")))
            //using (var conn = new SqlConnection(ConfigurationManager.GetEnvironmentVariable("ConnectionString")))

            {
                await conn.OpenAsync().ConfigureAwait(false);
                return await conn.QueryAsync<Stat>("SELECT TOP 5 * FROM [dbo].[Stat] WHERE [status] = '0'");
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
            m_timer = new Timer(PollStatTableFired, flag, dueTime, period);

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
