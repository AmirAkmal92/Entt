using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using Topshelf.Logging;

namespace Bespoke.PosEntt.ReceiveLocations
{
    public class LocationLogger : ILogger
    {



        public Task LogAsync(LogEntry entry)
        {
            this.Log(entry);
            return Task.FromResult(0);
        }
        public void Log(LogEntry entry)
        {
            var logger = HostLogger.Get<IposDepositFileDrop>();
            switch (entry.Severity)
            {
                case Severity.Critical: logger.Fatal(entry.Message); break;
                case Severity.Error: logger.Error(entry.Details, entry.Exception); break;
                case Severity.Warning: logger.Warn(entry.Message); break;
                case Severity.Log:
                case Severity.Info: logger.Info(entry.Message); break;
                case Severity.Verbose: logger.Debug(entry.Message); break;
                case Severity.Debug: logger.Debug(entry.Message); break;
            }

        }



    }
}
