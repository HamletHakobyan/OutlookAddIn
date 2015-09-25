using System.Collections.Generic;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class OpTask : Work
    {
        public const string ObjCodeString = "OPTASK";

        public string OpTaskType { get; set; }

        //TODO: Hrant: review this member with task member URL
        public string Url { get; set; }

        public int? Severity { get; set; }

        public User Owner { get; set; }

        public Task ResolveTask { get; set; }

        public Project ResolveProject { get; set; }

        public string ResolvingObjCode { get; set; }

        public string ResolvingObjID { get; set; }

        public string OwnerID { get; set; }

        public string QueueTopicID { get; set; }

        public List<Severity> AllSeverities { get; set; }

        public override string GetObjectType()
        {
            //return "issue";
            return "optask";
        }
    }
}