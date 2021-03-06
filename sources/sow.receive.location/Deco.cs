using System;
using FileHelpers;
using Newtonsoft.Json;
// ReSharper disable InconsistentNaming

namespace Bespoke.PosEntt.ReceivePorts
{
    [DelimitedRecord("	")]
    public class Deco
    {
        // TODO : the list of properties must follow the Vasn table schema
        public int id { get; set; }

        public int version { get; set; }

        public string all_connotes { get; set; }

        public string beat_no { get; set; }
        
        public string comment { get; set; }

        public string console_no { get; set; }

        public string courier_id { get; set; }

        public string data_entry_beat_no { get; set; }

        public string data_entry_location_id { get; set; }

        public string data_entry_staff_id { get; set; }

        public DateTime date_created { get; set; }

        public DateTime date_time { get; set; }

        public DateTime last_updated { get; set; }

        public string location_id { get; set; }
       
        public string status { get; set; }
        
        public string filename { get; set; }


        // the ToString(), kena ikut vasn event untuk RTS api
        public override string ToString()
        {
            //return $"{id}\t{version}\t{beat_no}\t{date_time:yyyyMMdd HHMMSS}\t{beat_no}\t{courier_id}\t"+
            //       $"{consignment_no}\t{van_item_type_code}\t{van_sender_name}\t" +
            //       $"{(string.IsNullOrEmpty(CodAccount) ? "-" : CodAccount)}\t" +
            //       $"{(string.IsNullOrEmpty(CodAmount) ? "-" : CodAmount)}";-	-

            return $"{courier_id}\t{location_id}\t{beat_no}\t{date_time:ddMMyyyy}\t{date_time:HHmmss}\t{console_no}\t{comment}\t{all_connotes}\t{"-"}";


        }
    }
}
