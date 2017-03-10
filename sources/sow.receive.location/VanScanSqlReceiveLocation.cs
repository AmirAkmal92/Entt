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

namespace Bespoke.PosEntt.ReceiveLocations
{
    public class VanScanSqlReceiveLocation : IReceiveLocation, IDisposable
    {
        private Timer m_timer;
        private HttpClient m_client;
        private bool m_paused;



        private async void PollVasnTableFired(object stateInfo)
        {
            if (m_paused) return;

            //TODO : get the file name
            var file = "TODO : get the file name" + DateTime.Now;

            var logger = new LocationLogger();
            var port = new RtsVasn(logger) { Uri = new Uri(file) };

            var fileInfo = new FileInfo(file);
            port.AddHeader("CreationTime", $"{fileInfo.CreationTime:s}");
            port.AddHeader("DirectoryName", fileInfo.DirectoryName);
            port.AddHeader("Exists", $"{fileInfo.Exists}");
            port.AddHeader("Length", $"{fileInfo.Length}");
            port.AddHeader("Extension", fileInfo.Extension);
            port.AddHeader("Attributes", $"{fileInfo.Attributes}");
            port.AddHeader("FullName", fileInfo.FullName);
            port.AddHeader("Name", fileInfo.Name);
            port.AddHeader("LastWriteTime", $"{fileInfo.LastWriteTime:s}");
            port.AddHeader("LastAccessTime", $"{fileInfo.LastAccessTime:s}");
            port.AddHeader("IsReadOnly", $"{fileInfo.IsReadOnly}");
            port.AddHeader("Rx:ApplicationName", "PosEntt");
            port.AddHeader("Rx:LocationName", nameof(VanScanSqlReceiveLocation));
            port.AddHeader("Rx:Type", "FolderReceiveLocation");
            port.AddHeader("Rx:MachineName", Environment.GetEnvironmentVariable("COMPUTERNAME"));
            port.AddHeader("Rx:UserName", Environment.GetEnvironmentVariable("USERNAME"));
            

            var number = 0;
            var records = await ReadVasnAsync();

            var engine = new FileHelperEngine<Vasn>();
            foreach (var r in records)
            {
                number++;
                if (null == r) continue; // we got an exception reading the record

                // polly policy goes here
                var retry = ConfigurationManager.GetEnvironmentVariableInt32($"{nameof(RtsVasn)}Retry", 3);
                var interval = ConfigurationManager.GetEnvironmentVariableInt32($"{nameof(RtsVasn)}Internal", 100);

                var pr = await Policy.Handle<Exception>()
                                    .WaitAndRetryAsync(retry, c => TimeSpan.FromMilliseconds(interval * Math.Pow(2, c)))
                                    .ExecuteAndCaptureAsync(() =>
                    {

                        var text = engine.WriteString(new []{r});
                        var request = new StringContent(text);
                        request.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");
                        return m_client.PostAsync("/api/rts/vasn", request);
                    });

                if (null != pr.FinalException)
                {
                    logger.Log(new LogEntry(pr.FinalException) { Message = $"Line {number}" });
                    continue;
                }
                var response = pr.Result;
                Console.Write($"\r{number} : {response.StatusCode}\t");
                logger.Log(new LogEntry { Message = $"Record: {number}({r.ScannerId}) , StatusCode: {(int)response.StatusCode}", Severity = Severity.Debug });

                if (!response.IsSuccessStatusCode)
                {
                    var warn = new LogEntry
                    {
                        Message = $"Non success status code record {number}({r.ScannerId}), StatusCode: {(int)response.StatusCode}",
                        Severity = Severity.Warning,
                        Details = $"{fileInfo.FullName}:{number}({r.ScannerId})"
                    };
                    logger.Log(warn);
                }
                else
                {
                    // TODO : delete the row in vasn
                }
            }

            Console.WriteLine();
            logger.Log(new LogEntry { Message = $"Done processing {file} with {number} record(s)", Severity = Severity.Info });

        }

        private async Task DeleteVasnRowAsync(Vasn item)
        {
            using (var conn = new SqlConnection(ConfigurationManager.GetEnvironmentVariable("OalConnectionString")))
            using (var cmd = new SqlCommand("DELETE FROM dbo.Vasn WHERE Id=@Id", conn))
            {
                cmd.Parameters.Add("@Id", SqlDbType.Int, 4).Value = item.Id;
                await conn.OpenAsync().ConfigureAwait(false);
                
            }
        }
        private async Task<IEnumerable<Vasn>> ReadVasnAsync()
        {
            using (var conn = new SqlConnection(ConfigurationManager.GetEnvironmentVariable("OalConnectionString")))
            {
                await conn.OpenAsync().ConfigureAwait(false);
                return await conn.QueryAsync<Vasn>("SELECT TOP 100 from dbo.Vasn");
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
            var dueTime = ConfigurationManager.GetEnvironmentVariableInt32("SowRtsDueTime", 1000);
            var period = ConfigurationManager.GetEnvironmentVariableInt32("SowRtsPeriod", 5 * 60 * 100);
            m_client = new HttpClient { BaseAddress = new Uri(ConfigurationManager.BaseUrl) };
            m_client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            
            var flag = new AutoResetEvent(false);
            m_timer = new Timer(PollVasnTableFired, flag, dueTime, period);

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
