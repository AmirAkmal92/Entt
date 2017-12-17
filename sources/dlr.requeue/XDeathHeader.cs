using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Bespoke.PosEntt.DlqRequeue
{
    public class XDeathHeader 
    {
        protected bool Equals(XDeathHeader other)
        {
            return string.Equals(Reason, other.Reason)
                && string.Equals(Queue, other.Queue)
                && string.Equals(Exchange, other.Exchange)
                && RoutingKeys.Length == other.RoutingKeys.Length;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                // ReSharper disable NonReadonlyFieldInGetHashCode
                int hashCode = (Reason != null ? Reason.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Queue != null ? Queue.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Exchange != null ? Exchange.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (RoutingKeys != null ? RoutingKeys.GetHashCode() : 0);
                return hashCode;
                // ReSharper restore NonReadonlyFieldInGetHashCode
            }
        }

        static string GetStringValue(object vals)
        {
            if (vals is bool)
                return $"{vals}";

            try
            {
                if (null != vals)
                    return Encoding.UTF8.GetString((byte[])vals);
                return null;
            }
            catch (Exception e)
            {
                return "Exception: " + e.Message;
            }

        }

        public XDeathHeader(IDictionary<string, object> entries)
        {
            if (null == entries) return;

            var temp = entries.Keys
                .Where(key => key != "x-death")
                .ToDictionary(key => key, key => GetStringValue(entries[key]));
            this.OtherHeaders = JsonConvert.SerializeObject(temp, Formatting.Indented);
            if (entries.ContainsKey("log"))
                this.Log = GetStringValue(entries["log"]);

            if (!entries.ContainsKey("x-death")) return;
            if (!(entries["x-death"] is IList<object> vals)) return;
            if (vals.Count == 0) return;
            if (!(vals[0] is IDictionary<string, object> hash)) return;

            this.Reason = hash.GetStringValue("reason");
            this.Queue = hash.GetStringValue("queue");
            this.Exchange = hash.GetStringValue("exchange");
            this.Time = hash.GetDateTimeValue("time");

            if (!(hash["routing-keys"] is IList<object> rks)) return;
            Debug.WriteLine(rks);

            this.RoutingKeys = rks.GetStringValues();
        }


        public string Log { get; set; }
        public string OtherHeaders { get; set; }
        public string[] RoutingKeys { get; set; }
        public string RoutingValuesKeys => null == this.RoutingKeys ? string.Empty : string.Join(",", this.RoutingKeys);
        public string Exchange { get; set; }
        public DateTime? Time { get; set; }
        public string Queue { get; set; }
        public string Reason { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((XDeathHeader)obj);
        }
    }
}
