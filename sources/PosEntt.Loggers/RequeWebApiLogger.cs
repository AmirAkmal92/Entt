using Bespoke.Sph.Domain;

namespace Bespoke.PosEntt.LoggersRequeWebApiLogger
{

    public class RequeWebApiLogger : ILogger
    {

        
        public Task LogAsync(LogEntry entry)
        {
        }

        public void Log(LogEntry entry)
        {
            /*  Message = $"{(int)response.StatusCode} GET {m_client.BaseAddress}/{url}",
                    Severity = Severity.Warning,
                    Log = EventLog.Subscribers,
                    Source = "GetApiBranch",
                    Details = text + "\r\n" + JsonConvert.SerializeObject(request)
                     */
            var sources = new [] {"GetApiBranch", "PssApi"};
            if(!sources.Contains(entry.Source))return;
            if(EventLog.Subscribers != entry.Log)return;
            //TODO : extrace the body
            var details = entry.Details;

            // reque



        }

    }
}