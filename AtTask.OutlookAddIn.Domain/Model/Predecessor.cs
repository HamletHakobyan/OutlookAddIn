using Newtonsoft.Json;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class Predecessor : EntityBase
    {
        public override string GetObjectType() { return "pred"; }

        public string PredecessorID { get; set; }

        public bool? IsCP { get; set; }

        public bool? isEnforced { get; set; }

        public bool? IsActive { get; set; }

        public PredecessorType PredecessorType { get; set; }

        [JsonProperty(PropertyName = "predecessor")]
        public Task PredecessorTask { get; set; }
    }
}