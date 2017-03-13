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


        }



    }
}
