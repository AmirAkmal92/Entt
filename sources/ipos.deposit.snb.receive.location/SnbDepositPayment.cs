using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bespoke.PosEntt.ReceiveLocations
{

    public class SnbDepositPayment
    {
        public static SnbDepositPayment Parse(IposDepositPayment[] lines)
        {
            var credit = lines.Single(x => x.PostingKey == "50");
            var debit = lines.Single(x => x.PostingKey == "40");
            var p = new SnbDepositPayment
            {
                Sequence = credit.SequenceNumber,
                DocumentDate = credit.DocumentDate,
                PostingDate = credit.PostingDate,
                CostCenter = credit.CostCenter
            };
            if (debit.AccountNo == "272120")
            {
                p.PaymentMethod = "Cheque";
                p.CheckNo = debit.Text;
            }

            if (debit.AccountNo == "272110")
            {
                p.PaymentMethod = "Cash";
            }
            p.Amount = credit.Amount;
            p.ReferenceNo = credit.Text;
            p.ReceiptNo = debit.ReferenceKey;
            return p;
        }
        public string ReferenceNo { get; set; }
        public decimal Amount { get; set; }
        public string CheckNo { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime DocumentDate { get; set; }
        public DateTime PostingDate { get; set; }
        public string CostCenter { get; set; }
        public int Sequence { get; set; }
        public string ReceiptNo { get; set; }
    }
}
