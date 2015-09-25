using System;

namespace AtTask.OutlookAddIn.Domain.Model
{
    [Serializable()]
    public class Currency
    {
        public bool? UseNegativeSign { get; set; }

        public int? FractionDigits { get; set; }

        public string Symbol { get; set; }

        public string ID { get; set; }

        public string GroupingSeparator { get; set; }

        public string DecimalSeparator { get; set; }
    }
}