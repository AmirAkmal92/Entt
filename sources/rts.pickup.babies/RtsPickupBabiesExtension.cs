﻿using Bespoke.PosEntt.Adapters.Oal;
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
                case dbo_delivery_console_event_new deco:
                    return deco.HashDecoEventNew();
                case dbo_normal_console_event_new norm:
                    return norm.HashNormEventNew();
                case dbo_console_details console:
                    return console.HashConsoleDetails();
                case dbo_delivery_event_new deli:
                    return deli.HashDeliEventNew();
                case dbo_status_code_event_new stat:
                    return stat.HashStatusCodeEventNew();
                case dbo_ips_import ips:
                    return ips.HashIpsImport();
                case dbo_hip_event_new hip:
                    return hip.HashHipEventNew();
                case dbo_hop_event_new hop:
                    return hop.HashHopEventNew();
                case  dbo_sop_event_new sop:
                    return sop.HashSopEventNew();
                case dbo_sip_event_new sip:
                    return sip.HashSipEventNew();
                case dbo_wwp_event_new_log wwp:
                    return wwp.HashWwpEventNewLog();
                case dbo_vasn_event_new vasn:
                    return vasn.HashVasnEventNew();
                case dbo_comment_event_new comm:
                    return comm.HashCommentEventNew();
                case dbo_missort_event_new miss:
                    return miss.HashMissortEventNew();
                case dbo_event_pending_console pending:
                    return pending.HashPendingConsoleEvent();
                case dbo_event_exception exception:
                    return exception.HashEventException();
                case dbo_console_duplicate_error duplicate:
                    return duplicate.HashConsoleDuplicateError();
            }

            return "";
        }

        public static string GetConsignmentNo(this DomainObject domain)
        {
            switch (domain)
            {
                case dbo_delivery_console_event_new deco:
                    return deco.GetDecoConsignmentNo();
                case dbo_normal_console_event_new norm:
                    return norm.GetNormConsignmentNo();
                case dbo_console_details console:
                    return console.GetConsoleDetailsConsignmentNo();
                case dbo_delivery_event_new deli:
                    return deli.GetDeliConsignmentNo();
                case dbo_status_code_event_new stat:
                    return stat.GetStatusCodeConsignmentNo();
                case dbo_ips_import ips:
                    return ips.GetIpsImportConsignmentNo();
                case dbo_hip_event_new hip:
                    return hip.GetHipConsignmentNo();
                case dbo_hop_event_new hop:
                    return hop.GetHopConsignmentNo();
                case dbo_sop_event_new sop:
                    return sop.GetSopConsignmentNo();
                case dbo_sip_event_new sip:
                    return sip.GetSipConsignmentNo();
                case dbo_wwp_event_new_log wwp:
                    return wwp.GetWwpConsignmentNo();
                case dbo_vasn_event_new vasn:
                    return vasn.GetVasnConsignmentNo();
                case dbo_comment_event_new comm:
                    return comm.GetCommentConsignmentNo();
                case dbo_missort_event_new miss:
                    return miss.GetMissortConsignmentNo();
                case dbo_event_pending_console pending:
                    return pending.GetPendingConsoleConsignmentNo();
                case dbo_event_exception exception:
                    return exception.GetEventExceptionConsignmentNo();
                case dbo_console_duplicate_error duplicate:
                    return duplicate.GetConsoleDuplicateErrorConsignmentNo();
            }

            return "";
        }

        public static DateTime? GetDateTime(this DomainObject domain)
        {
            switch (domain)
            {
                case dbo_delivery_console_event_new deco:
                    return deco.GetDecoDateTime();
                case dbo_normal_console_event_new norm:
                    return norm.GetNormDateTime();
                case dbo_console_details console:
                    return console.GetConsoleDetailsDateTime();
                case dbo_delivery_event_new deli:
                    return deli.GetDeliDateTime();
                case dbo_status_code_event_new stat:
                    return stat.GetStatusCodeDateTime();
                case dbo_ips_import ips:
                    return ips.GetIpsImportDateTime();
                case dbo_hip_event_new hip:
                    return hip.GetHipDateTime();
                case dbo_hop_event_new hop:
                    return hop.GetHopDateTime();
                case dbo_sop_event_new sop:
                    return sop.GetSopDateTime();
                case dbo_sip_event_new sip:
                    return sip.GetSipDateTime();
                case dbo_wwp_event_new_log wwp:
                    return wwp.GetWwpDateTime();
                case dbo_vasn_event_new vasn:
                    return vasn.GetVasnDateTime();
                case dbo_comment_event_new comm:
                    return comm.GetCommentDateTime();
                case dbo_missort_event_new miss:
                    return miss.GetMissortDateTime();
                case dbo_event_pending_console pending:
                    return pending.GetPendingConsoleDateTime();
                case dbo_event_exception exception:
                    return exception.GetEventExceptionDateTime();
                case dbo_console_duplicate_error duplicate:
                    return duplicate.GetConsoleDuplicateErrorDateTime();
            }

            return null;
        }

        private static string HashDecoEventNew(this dbo_delivery_console_event_new deco)
        {
            var key = $"deco.{deco.GetType().Name}.{deco.consignment_no}.{deco.date_field.Value:s}";
            return GetHashKey(key);
        }
        private static string GetDecoConsignmentNo(this dbo_delivery_console_event_new deco)
        {
            return deco.consignment_no;
        }

        private static DateTime? GetDecoDateTime(this dbo_delivery_console_event_new deco)
        {
            return deco.date_field;
        }

        private static string HashNormEventNew(this dbo_normal_console_event_new norm)
        {
            var key = $"norm.{norm.GetType().Name}.{norm.consignment_no}.{norm.date_field.Value:s}";
            return GetHashKey(key);
        }
        private static string GetNormConsignmentNo(this dbo_normal_console_event_new norm)
        {
            return norm.consignment_no;
        }

        private static DateTime? GetNormDateTime(this dbo_normal_console_event_new norm)
        {
            return norm.date_field;
        }

        private static string HashConsoleDetails(this dbo_console_details console)
        {
            var key = $"console.{console.GetType().Name}.{console.console_no}.{console.date_field.Value:s}";
            return GetHashKey(key);
        }
        private static string GetConsoleDetailsConsignmentNo(this dbo_console_details console)
        {
            return console.console_no;
        }

        private static DateTime? GetConsoleDetailsDateTime(this dbo_console_details console)
        {
            return console.date_field;
        }

        private static string HashDeliEventNew(this dbo_delivery_event_new deli)
        {
            var key = $"deli.{deli.GetType().Name}.{deli.consignment_no}.{deli.date_field.Value:s}";
            return GetHashKey(key);
        }
        private static string GetDeliConsignmentNo(this dbo_delivery_event_new deli)
        {
            return deli.consignment_no;
        }

        private static DateTime? GetDeliDateTime(this dbo_delivery_event_new deli)
        {
            return deli.date_field;
        }

        private static string HashStatusCodeEventNew(this dbo_status_code_event_new stat)
        {
            var key = $"stat.{stat.GetType().Name}.{stat.consignment_no}.{stat.date_field.Value:s}";
            return GetHashKey(key);
        }
        private static string GetStatusCodeConsignmentNo(this dbo_status_code_event_new stat)
        {
            return stat.consignment_no;
        }

        private static DateTime? GetStatusCodeDateTime(this dbo_status_code_event_new stat)
        {
            return stat.date_field;
        }

        private static string HashIpsImport(this dbo_ips_import ips)
        {
            var key = $"ips.{ips.GetType().Name}.{ips.item_id}.{ips.event_date_local_date_field.Value:s}";
            return GetHashKey(key);
        }
        private static string GetIpsImportConsignmentNo(this dbo_ips_import ips)
        {
            return ips.item_id;
        }

        private static DateTime? GetIpsImportDateTime(this dbo_ips_import ips)
        {
            return ips.event_date_local_date_field;
        }

        private static string HashHipEventNew(this dbo_hip_event_new hip)
        {
            var key = $"hip.{hip.GetType().Name}.{hip.consignment_no}.{hip.date_field.Value:s}";
            return GetHashKey(key);
        }
        private static string GetHipConsignmentNo(this dbo_hip_event_new hip)
        {
            return hip.consignment_no;
        }

        private static DateTime? GetHipDateTime(this dbo_hip_event_new hip)
        {
            return hip.date_field;
        }

        private static string HashHopEventNew(this dbo_hop_event_new hop)
        {
            var key = $"hop.{hop.GetType().Name}.{hop.consignment_no}.{hop.date_field.Value:s}";
            return GetHashKey(key);
        }
        private static string GetHopConsignmentNo(this dbo_hop_event_new hop)
        {
            return hop.consignment_no;
        }

        private static DateTime? GetHopDateTime(this dbo_hop_event_new hop)
        {
            return hop.date_field;
        }

        private static string HashSopEventNew(this dbo_sop_event_new sop)
        {
            var key = $"sop.{sop.GetType().Name}.{sop.consignment_no}.{sop.date_field.Value:s}";
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
            var key = $"sip.{sip.GetType().Name}.{sip.consignment_no}.{sip.date_field.Value:s}";
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
            var key = $"wwp.{wwp.GetType().Name}.{wwp.consignment_note_number}.{wwp.date_field.Value:s}";
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

        private static string HashVasnEventNew(this dbo_vasn_event_new vasn)
        {
            var key = $"vasn.{vasn.GetType().Name}.{vasn.consignment_no}.{vasn.date_field.Value:s}";
            return GetHashKey(key);
        }
        private static string GetVasnConsignmentNo(this dbo_vasn_event_new vasn)
        {
            return vasn.consignment_no;
        }

        private static DateTime? GetVasnDateTime(this dbo_vasn_event_new vasn)
        {
            return vasn.date_field;
        }

        private static string HashCommentEventNew(this dbo_comment_event_new comm)
        {
            var key = $"comm.{comm.GetType().Name}.{comm.consignment_no}.{comm.date_field.Value:s}";
            return GetHashKey(key);
        }
        private static string GetCommentConsignmentNo(this dbo_comment_event_new comm)
        {
            return comm.consignment_no;
        }

        private static DateTime? GetCommentDateTime(this dbo_comment_event_new comm)
        {
            return comm.date_field;
        }

        private static string HashMissortEventNew(this dbo_missort_event_new miss)
        {
            var key = $"miss.{miss.GetType().Name}.{miss.consignment_no}.{miss.date_field.Value:s}";
            return GetHashKey(key);
        }
        private static string GetMissortConsignmentNo(this dbo_missort_event_new miss)
        {
            return miss.consignment_no;
        }

        private static DateTime? GetMissortDateTime(this dbo_missort_event_new miss)
        {
            return miss.date_field;
        }

        private static string HashPendingConsoleEvent(this dbo_event_pending_console pending)
        {
            var key = $"pending.{pending.GetType().Name}.{pending.console_no}.{pending.date_field.Value:s}.{pending.event_class}";
            return GetHashKey(key);
        }
        private static string GetPendingConsoleConsignmentNo(this dbo_event_pending_console pending)
        {
            return pending.console_no;
        }

        private static DateTime? GetPendingConsoleDateTime(this dbo_event_pending_console pending)
        {
            return pending.date_field;
        }

        private static string HashEventException(this dbo_event_exception exception)
        {
            var key = $"exception.{exception.GetType().Name}.{exception.consignment_no}.{exception.date_field.Value:s}";
            return GetHashKey(key);
        }
        private static string GetEventExceptionConsignmentNo(this dbo_event_exception exception)
        {
            return exception.consignment_no;
        }

        private static DateTime? GetEventExceptionDateTime(this dbo_event_exception exception)
        {
            return exception.date_field;
        }

        private static string HashConsoleDuplicateError(this dbo_console_duplicate_error duplicate)
        {
            var key = $"duplicate.{duplicate.GetType().Name}.{duplicate.console_no}.{duplicate.date_field.Value:s}";
            return GetHashKey(key);
        }
        private static string GetConsoleDuplicateErrorConsignmentNo(this dbo_console_duplicate_error duplicate)
        {
            return duplicate.console_no;
        }

        private static DateTime? GetConsoleDuplicateErrorDateTime(this dbo_console_duplicate_error duplicate)
        {
            return duplicate.date_field;
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
