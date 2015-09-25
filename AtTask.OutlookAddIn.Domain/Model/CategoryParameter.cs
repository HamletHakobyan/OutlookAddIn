using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class CategoryParameter : StreamBase
    {
        public CategoryParameter()
        {
            SecurityLevel = SecurityLevelType.LE;
            ViewSecurityLevel = SecurityLevelType.V;
        }
        public int? DisplayOrder { get; set; }

        public string ParameterID { get; set; }

        public string ParameterGroupID { get; set; }

        public bool? RowShared { get; set; }

        public Parameter Parameter { get; set; }

        public ParameterGroup ParameterGroup { get; set; }

        public bool? IsRequired { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SecurityLevelType SecurityLevel { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SecurityLevelType ViewSecurityLevel { get; set; }

        public CategoryParameterExpression CategoryParameterExpression { get; set; }

        public override string GetObjectType()
        {
            return "categoryparameter";
        }
    }
}