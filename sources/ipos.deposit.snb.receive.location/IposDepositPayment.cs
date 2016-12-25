using System;
using FileHelpers;
using Newtonsoft.Json;

namespace Bespoke.PosEntt.ReceiveLocations
{
    [FixedLengthRecord]
    public class IposDepositPayment
    {
        [JsonIgnore]
        [FieldFixedLength(8)]
        public string DocumentDateRaw;
        public DateTime DocumentDate => System.DateTime.ParseExact(DocumentDateRaw, @"ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture);

        [JsonIgnore]
        [FieldFixedLength(8)]
        public string PostingDateRaw;
        public DateTime PostingDate => System.DateTime.ParseExact(PostingDateRaw, @"ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture);

        [JsonIgnore]
        [FieldFixedLength(2)]
        public string DocumentTypeRaw;
        public string DocumentType => DocumentTypeRaw;

        [JsonIgnore]
        [FieldFixedLength(5)]
        public string CurrencyRaw;
        public string Currency => CurrencyRaw;

        [JsonIgnore]
        [FieldFixedLength(9)]
        public string ExchangeRateRaw;
        public decimal ExchangeRate => decimal.Parse(ExchangeRateRaw, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);

        [JsonIgnore]
        [FieldFixedLength(16)]
        public string ReferenceRaw;
        public string Reference => ReferenceRaw;

        [JsonIgnore]
        [FieldFixedLength(25)]
        public string DocumentHeaderTextRaw;
        public string DocumentHeaderText => DocumentHeaderTextRaw;

        [JsonIgnore]
        [FieldFixedLength(2)]
        public string PostingKeyRaw;
        public string PostingKey => PostingKeyRaw;

        [JsonIgnore]
        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both)]
        public string AccountNoRaw;
        public string AccountNo => AccountNoRaw;

        [JsonIgnore]
        [FieldFixedLength(15)]
        [FieldTrim(TrimMode.Both)]
        public string AmountRaw;
        public decimal Amount => decimal.Parse(AmountRaw, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);

        [JsonIgnore]
        [FieldFixedLength(10)]
        public string CostCenterRaw;
        public string CostCenter => CostCenterRaw;

        [JsonIgnore]
        [FieldFixedLength(13)]
        public string QuantityRaw;
        public string Quantity => QuantityRaw;

        [JsonIgnore]
        [FieldFixedLength(2)]
        public string AssignmentRaw;
        public string Assignment => AssignmentRaw;

        [JsonIgnore]
        [FieldFixedLength(18)]
        public string TextRaw;
        public string Text => TextRaw;

        [JsonIgnore]
        [FieldFixedLength(50)]
        public string ReferenceKeyRaw;
        public string ReferenceKey => ReferenceKeyRaw;

        [JsonIgnore]
        [FieldFixedLength(5)]
        [FieldTrim(TrimMode.Both)]
        public string SequenceNumberRaw;
        public int SequenceNumber => int.Parse(SequenceNumberRaw, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);



        [FieldHidden]
        private int m_lineNumber;
        public int GetLineNumber() { return m_lineNumber; }
        internal void SetLineNumber(int line) { m_lineNumber = line; }
    }
}