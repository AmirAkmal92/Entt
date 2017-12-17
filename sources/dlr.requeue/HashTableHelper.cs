using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;

namespace Bespoke.PosEntt.DlqRequeue
{
    public static class HashTableHelper
    {

        public static string GetStringValue(this IDictionary<string, object> hash, string key)
        {
            var vals = hash[key];
            if (null != vals)
                return Encoding.UTF8.GetString((byte[])vals);
            return null;

        }
        public static DateTime? GetDateTimeValue(this IDictionary<string, object> hash, string key)
        {
            var vals = hash[key];
            var time = (AmqpTimestamp)vals;
            return time.UnixTime.UnixTimeStampToDateTime();
        }
        public static DateTime UnixTimeStampToDateTime(this long unixTimeStamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static string[] GetStringValues(this IList<object> list)
        {
            var ret = from object v in list
                select Encoding.UTF8.GetString((byte[])v);
            return ret.ToArray();
        }

    }
}