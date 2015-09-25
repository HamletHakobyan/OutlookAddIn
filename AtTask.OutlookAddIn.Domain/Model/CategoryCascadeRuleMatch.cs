namespace AtTask.OutlookAddIn.Domain.Model
{
    public class CategoryCascadeRuleMatch : EntityBase
    {
        public string ParameterID { get; set; }
        public string Value { get; set; }
        public MatchType MatchType { get; set; }
        public Parameter Parameter { get; set; }
        public override string GetObjectType()
        {
            return "categorycascaderulematch";
        }
    }
}