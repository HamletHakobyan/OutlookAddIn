using System.Diagnostics;

namespace AtTask.OutlookAddIn.Domain.Model
{
    [DebuggerDisplay("Label={Label}, Value={Value}")]
    public class CustomEnum : EntityBase
    {
        public string Value { get; set; }

        public string Label { get; set; }

        public string EquatesWith { get; set; }

        public string Color { get; set; }

        public bool? IsPrimary { get; set; }

        public string EnumClass { get; set; }

        public string ValueAsString { get; set; }

        public int? ValueAsInt { get; set; }

        public override string GetObjectType() { return "customenum"; }
    }
}