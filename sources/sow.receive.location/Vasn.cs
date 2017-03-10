using System;
using FileHelpers;
using Newtonsoft.Json;
// ReSharper disable InconsistentNaming

namespace Bespoke.PosEntt.ReceivePorts
{
    [DelimitedRecord("	")]
    public class Vasn
    {
        // TODO : the list of properties must follow the Vasn table schema
        public int id { get; set; }
        
        public string courier_id { get; set; }
        
        public string location_id { get; set; }
        
        public string beat_no { get; set; }
        
        
        public DateTime Time { get; set; }
        public string consignment_no { get; set; }
        
        public string ItemType { get; set; }
        public DateTime Date { get; set; }
        public string SenderName { get; set; }
        public string CodAccount { get; set; }
        public string CodAmount { get; set; }


        // the ToString(), kena ikut vasn event untuk RTS api
        public override string ToString()
        {
            return $"{courier_id}\t{location_id}\t{beat_no}\t{Date:yyyyMMdd}\t{Time:HHmmss}\t{consignment_no}\t{ItemType}\t{SenderName}\t" +
                   $"{(string.IsNullOrEmpty(CodAccount) ? "-" : CodAccount)}\t" +
                   $"{(string.IsNullOrEmpty(CodAmount) ? "-" : CodAmount)}";
        }
    }
}
