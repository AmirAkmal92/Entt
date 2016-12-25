using System;
using Bespoke.Sph.Domain;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;

namespace Bespoke.PosEntt.ReceiveLocations
{
    public class IposDepositFileDrop : IReceiveLocation, IDisposable
    {
        private FileSystemWatcher m_watcher;
        private HttpClient m_client;
        private bool m_paused;



        private async void FswChanged(object sender, FileSystemEventArgs e)
        {
            if (m_paused) return;

            var file = e.FullPath;
            var wip = file + ".wip";
            await WaitReadyAsync(file);

            var logger = new LocationLogger();
            var port = new IposDepositPaymentPort(logger) { Uri = new Uri(file) };

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
            port.AddHeader("Rx:LocationName", nameof(IposDepositFileDrop));
            port.AddHeader("Rx:Type", "FolderReceiveLocation");
            port.AddHeader("Rx:MachineName", Environment.GetEnvironmentVariable("COMPUTERNAME"));
            port.AddHeader("Rx:UserName", Environment.GetEnvironmentVariable("USERNAME"));



            File.Move(file, wip);
            await WaitReadyAsync(wip);
            await Task.Delay(100);
            
            var number = 0;
            var lines = File.ReadLines(wip);
            var records = port.Process(lines);
            // now post it to the server
            Console.WriteLine($"Reading file {file}");
            foreach (var r in records)
            {
                number++;
                if (null == r) continue; // we got an exception reading the record

                // polly policy goes here
                var retry = ConfigurationManager.GetEnvironmentVariableInt32("IposDepositFileDrop", 3);
                var interval = ConfigurationManager.GetEnvironmentVariableInt32("IposDepositFileDrop", 100);

                var pr = await Policy.Handle<Exception>()
                                    .WaitAndRetryAsync(retry, c => TimeSpan.FromMilliseconds(interval * Math.Pow(2, c)))
                                    .ExecuteAndCaptureAsync(() =>
                    {
                        var request = new StringContent(r.ToJson());
                        request.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                        return m_client.PostAsync("/api/ipos-deposit-payments/", request);
                    });

                if (null != pr.FinalException)
                {
                    logger.Log(new LogEntry(pr.FinalException) { Message = $"Line {number}" });
                    continue;
                }
                var response = pr.Result;
                Console.Write($"\r{number} : {response.StatusCode}\t");
                logger.Log(new LogEntry { Message = $"Record: {number}({r.Sequence}) , StatusCode: {(int)response.StatusCode}", Severity = Severity.Debug });

                if (!response.IsSuccessStatusCode)
                {
                    var warn = new LogEntry
                    {
                        Message = $"Non success status code record {number}({r.Sequence}), StatusCode: {(int)response.StatusCode}",
                        Severity = Severity.Warning,
                        Details = $"{fileInfo.FullName}:{number}({r.Sequence})"
                    };
                    logger.Log(warn);     
                }
            }


            Console.WriteLine();
            logger.Log(new LogEntry { Message = $"Done processing {file} with {number} record(s)", Severity = Severity.Info });

            var archivedFolder = ConfigurationManager.GetEnvironmentVariable(nameof(IposDepositFileDrop) + "ArchiveLocation");
            if (!string.IsNullOrWhiteSpace(archivedFolder) && Directory.Exists(archivedFolder))
                File.Move(wip, Path.Combine(archivedFolder, Path.GetFileName(file)));
            else
                File.Delete(wip);

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

            var token = ConfigurationManager.GetEnvironmentVariable("IposDepositFileDrop_JwtToken") ?? @"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyIjoiYWRtaW4iLCJyb2xlcyI6WyJhZG1pbmlzdHJhdG9ycyIsImRldmVsb3BlcnMiXSwiZW1haWwiOjE0ODMxNDI0MDAsInN1YiI6ImQ2MTc0NmUwLTdhZmMtNDhjYS04OTVmLWQzZDAzOWQ4ZDI4MSIsIm5iZiI6MTQ5MDUwMzE1MiwiaWF0IjoxNDc0ODY0NzUyLCJleHAiOjE0ODMxNDI0MDAsImF1ZCI6IkRldlYxIn0.50WRticwNzLXL3sHdJsBsdhulOQqhPJlYATxm-amFfw";
            m_client = new HttpClient { BaseAddress = new Uri(ConfigurationManager.BaseUrl) };
            m_client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var path = ConfigurationManager.GetEnvironmentVariable("IposDepositFileDrop_Path") ?? @"D:\temp\ipos-deposit-drop";
            m_watcher = new FileSystemWatcher(path, @"ipos_dep_*.txt") { EnableRaisingEvents = true };
            m_watcher.Created += FswChanged;

            return true;


        }

        public bool Stop() { this.Dispose(); return true; }



        public void Dispose()
        {
            m_watcher?.Dispose();
            m_watcher = null;
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
