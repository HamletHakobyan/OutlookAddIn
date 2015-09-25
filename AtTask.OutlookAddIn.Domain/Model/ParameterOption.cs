namespace AtTask.OutlookAddIn.Domain.Model
{
    public class ParameterOption : EntityBase
    {
        public string Label { get; set; }

        public string Value { get; set; }

        public int? DisplayOrder { get; set; }

        public bool? IsDefault { get; set; }

        public bool? IsHidden { get; set; }

        public override string GetObjectType() { return "parameteroption"; }
    }
}