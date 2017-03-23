using System;
using FileHelpers;
using Newtonsoft.Json;
// ReSharper disable InconsistentNaming

namespace Bespoke.PosEntt.ReceivePorts
{
    [DelimitedRecord("	")]
    public class Deli
    {
        // TODO : the list of properties must follow the Vasn table schema
        public string id { get; set; }

        public int version { get; set; }
        
        public string alternative_address { get; set; }
        
        public string authorized_name { get; set; }
        
        public string bank_code { get; set; }

        public string beat_no { get; set; }

        public string cheque_no { get; set; }

        public string comment { get; set; }

        public string courier_id { get; set; }

        public string consignment_no { get; set; }

        public string data_entry_beat_no { get; set; }

        public string data_entry_location_id { get; set; }

        public string data_entry_staff_id { get; set; }

        public string location_id { get; set; }

        public DateTime date_created { get; set; }

        public DateTime last_updated { get; set; }

        public DateTime date_time { get; set; }

        public byte drop_code { get; set; }

        public string lokasi_drop { get; set; }

        public byte damage_code { get; set; }

        public string mode_of_payment { get; set; }

        public string payment_type { get; set; }

        public string recepient_ic { get; set; }

        public string recepient_location { get; set; }

        public int reason_code_id { get; set; }

        public string recipient_name { get; set; }

        public string status { get; set; }

        public double total_payment { get; set; }

        public string filename { get; set; }


        // the ToString(), kena ikut vasn event untuk RTS api
        public override string ToString()
        {
            //return $"{id}\t{version}\t{beat_no}\t{date_time:yyyyMMdd HHMMSS}\t{beat_no}\t{courier_id}\t"+
            //       $"{consignment_no}\t{van_item_type_code}\t{van_sender_name}\t" +
            //       $"{(string.IsNullOrEmpty(CodAccount) ? "-" : CodAccount)}\t" +
            //       $"{(string.IsNullOrEmpty(CodAmount) ? "-" : CodAmount)}";-	-

            return $"{courier_id}\t{location_id}\t{beat_no}\t{date_time:ddMMyyyy}\t{date_time:HHmmss}\t{consignment_no}\t{reason_code_id}\t{recipient_name}\t"
                + $"{recepient_ic}\t{recepient_location}\t{damage_code}\t{authorized_name}\t{comment}\t{alternative_address}\t{payment_type}\t{mode_of_payment}\t"
                + $"{total_payment}\t{cheque_no}\t{bank_code}\t{drop_code}\t{lokasi_drop}\t{0}";
        }
    }
}
