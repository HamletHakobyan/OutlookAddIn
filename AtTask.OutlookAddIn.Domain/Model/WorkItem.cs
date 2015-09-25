using Newtonsoft.Json;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class WorkItem : EntityBase
    {
        public int? Priority { get; set; }

        public bool? IsDone { get; set; }

        public bool? IsDead { get; set; }

        public string UserID { get; set; }

        public Task Task { set; get; }

        //TODO: Hrant: Move this property to client object
        [JsonIgnore]
        public int? DisplayPriority { get; set; }

        public override string GetObjectType() { return "workitem"; }
    }
}