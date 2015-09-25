using System.Collections.Generic;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class QueueTopic : NamedEntityBase
    {
        public QueueTopic ParentTopic { get; set; }

        public Category DefaultCategory { get; set; }

        public List<OpTaskTypeEnum> AllowedOpTaskTypes { get; set; }

        public string DefaultApprovalProcessID { get; set; }

        public RoutingRule DefaultRoute { get; set; }

        public int? DefaultDurationMinutes { get; set; }

        public DurationUnit? DefaultDurationUnit { get; set; }

        public QueueDef QueueDef { get; set; }

        public string QueueID { get; set; }

        public string DefaultCategoryID { get; set; }

        public string DefaultRouteID { get; set; }

        public string ParentTopicID { get; set; }

        public string Description { get; set; }

        public string ParentTopicGroupID { get; set; }

        public override string GetObjectType() { return "quet"; }
    }
}