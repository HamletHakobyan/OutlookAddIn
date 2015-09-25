namespace AtTask.OutlookAddIn.Domain.Model
{
    public class ParameterGroup : NamedEntityBase
    {
        public string Description { get; set; }
        public override string GetObjectType() { return "parametergroup"; }
    }
}