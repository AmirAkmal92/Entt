using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bespoke.Sph.Domain;
using FileHelpers;

namespace Bespoke.PosEntt.ReceiveLocations
{
    public class IposDepositPaymentPort
    {
        public IposDepositPaymentPort(ILogger logger) { this.Logger = logger; }


        public System.Uri Uri { get; set; }
        public ILogger Logger { get; }
        public IDictionary<string, string> Headers { get; } = new Dictionary<string, string>();


        public void AddHeader<T>(string name, T value)
        {
            this.Headers.Add(name, $"{value}");
        }

        public IEnumerable<SnbDepositPayment> Process(IEnumerable<string> lines)
        {
            var fields = new Dictionary<string, int> {
        {"DocumentDate", 8},
        {"PostingDate", 8},
        {"DocumentType", 2},
        {"Currency", 5},
        {"ExchangeRate", 9},
        {"Reference", 16},
        {"DocumentHeaderText", 25},
        {"PostingKey",2 },
        {"AccountNo", 10},
        {"Amount", 15},
        {"CostCenter",10 },
        {"Quantity", 13},
        {"Assignment",2 },
        {"Text",18 },
        {"ReferenceKey", 50},
        {"SequenceNumber", 5}
    };

            var positions = fields.Values.ToArray();
            var names = fields.Keys.ToArray();
            var sequence = 0;
            var engine = new FileHelperEngine<IposDepositPayment>();
            var transactions = new List<IposDepositPayment>();

            var payments = new List<SnbDepositPayment>();

            foreach (var line in lines)
            {
                if (line.StartsWith("EOF")) continue;
                /**/
                for (int i = 0; i < positions.Length; i++)
                {
                    var value = line.Substring(positions.Take(i).Sum(x => x), positions[i]);
                    Debug.Write($"{names[i]} = {value}, ");
                }
                Debug.WriteLine("");
                Debug.WriteLine("--------------------");
                var record = engine.ReadString(line)[0];
                if (record.SequenceNumber == sequence)
                {
                    transactions.Add(record);
                    continue;
                }
                sequence = record.SequenceNumber;
                if (transactions.Count > 0)
                {
                    var snb = SnbDepositPayment.Parse(transactions.ToArray());
                    payments.Add(snb);
                }
                transactions = new List<IposDepositPayment> { record };

            }
            var snb2 = SnbDepositPayment.Parse(transactions.ToArray());
            payments.Add(snb2);


            return payments;

        }
    }
}