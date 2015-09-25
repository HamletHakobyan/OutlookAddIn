using System.Collections.Generic;
using Newtonsoft.Json;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class Category : NamedEntityBase
    {
        public List<CategoryParameter> CategoryParameters { get; set; }

        public string GroupID { get; set; }

        public CategoryType? CatObjCode { get; set; }

        [JsonProperty(PropertyName = "categoryCascadeRules")]
        public IEnumerable<CategoryCascadeRule> CascadeRules { get; set; }

        public override string GetObjectType() { return "category"; }
    }
}