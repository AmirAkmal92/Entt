using Bespoke.PosEntt.Adapters.Oal;
using Bespoke.Sph.Domain;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Bespoke.PosEntt.CustomActions
{
    public static class RtsPickupBabiesExtension
    {
        public static string HashEvent(this DomainObject domain)
        {
            switch (domain)
            {
                case  dbo_sop_event_new sop:
                    return sop.HashSopEventNew();
                case dbo_sip_event_new sip:
                    return sip.HashSipEventNew();
                case dbo_wwp_event_new_log wwp:
                    return wwp.HashWwpEventNewLog();
            }

            return "";
        }

        public static string GetConsignmentNo(this DomainObject domain)
        {
            switch (domain)
            {
                case dbo_sop_event_new sop:
                    return sop.GetSopConsignmentNo();
                case dbo_sip_event_new sip:
                    return sip.GetSipConsignmentNo();
                case dbo_wwp_event_new_log wwp:
                    return wwp.GetWwpConsignmentNo();
            }

            return "";
        }

        public static DateTime? GetDateTime(this DomainObject domain)
        {
            switch (domain)
            {
                case dbo_sop_event_new sop:
                    return sop.GetSopDateTime();
                case dbo_sip_event_new sip:
                    return sip.GetSipDateTime();
                case dbo_wwp_event_new_log wwp:
                    return wwp.GetWwpDateTime();
            }

            return null;
        }

        private static string HashSopEventNew(this dbo_sop_event_new sop)
        {
            var key = $"sop.{sop.consignment_no}.{sop.date_field.Value:s}";
            return GetHashKey(key);
        }
        private static string GetSopConsignmentNo(this dbo_sop_event_new sop)
        {
            return sop.consignment_no;
        }

        private static DateTime? GetSopDateTime(this dbo_sop_event_new sop)
        {
            return sop.date_field;
        }

        private static string HashSipEventNew(this dbo_sip_event_new sip)
        {
            var key = $"sip.{sip.consignment_no}.{sip.date_field.Value:s}";
            return GetHashKey(key);
        }

        private static string GetSipConsignmentNo(this dbo_sip_event_new sip)
        {
            return sip.consignment_no;
        }
        
        private static DateTime? GetSipDateTime(this dbo_sip_event_new sip)
        {
            return sip.date_field;
        }        
        
        private static string HashWwpEventNewLog(this dbo_wwp_event_new_log wwp)
        {
            var key = $"wwp.{wwp.consignment_note_number}.{wwp.date_field.Value:s}";
            return GetHashKey(key);
        }

        private static string GetWwpConsignmentNo(this dbo_wwp_event_new_log wwp)
        {
            return wwp.consignment_note_number;
        }

        private static DateTime? GetWwpDateTime(this dbo_wwp_event_new_log wwp)
        {
            return wwp.date_field;
        }

        private static string GetHashKey(string input)
        {
            if (string.IsNullOrEmpty(input)) return null;
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                return string.Join("", hash.Select(x => x.ToString("x2")));
            }
        }
    }
}
