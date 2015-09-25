using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class CategoryCascadeRule : EntityBase
    {
        public Parameter NextParameter { get; set; }
        public string NextParameterID { get; set; }
        public string NextParameterGroupID { get; set; }
        public ParameterGroup NextParameterGroup { get; set; }
        public string OtherwiseParameterID { get; set; }
        public Parameter OtherwiseParameter { get; set; }
        public RuleType RuleType { get; set; }
        public bool ToEndOfForm { get; set; }

        [JsonProperty(PropertyName = "categoryCascadeRuleMatches")]
        public IEnumerable<CategoryCascadeRuleMatch> CascadeRuleMatches { get; set; }

        public override string GetObjectType()
        {
            return "categorycascaderule";
        }
    }
}