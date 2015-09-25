using System.Collections.Generic;
using Newtonsoft.Json;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class QueueDef : EntityBase
    {
        public QueueDef()
        {
            RequestorCoreAction = ActionType.LIMITED_EDIT;
        }

        public int IsPublic { get; set; }

        public string ProjectID { get; set; }

        public bool? HasQueueTopics { get; set; }

        public string DefaultCategoryID { get; set; }

        public string DefaultRouteID { get; set; }

        public RoutingRule DefaultRoute { get; set; }

        public int? DefaultDurationMinutes { get; set; }

        public AddOpTaskStyle? AddOpTaskStyle { get; set; }

        public DurationUnit? DefaultDurationUnit { get; set; }

        public List<OpTaskField> VisibleOpTaskFields { get; set; }

        public List<OpTaskTypeEnum> AllowedOpTaskTypes { get; set; }

        public List<QueueTopic> QueueTopics { get; set; }

        public Category DefaultCategory { get; set; }

        public List<string> AllowedLegacyQueueTopicIDs { get; set; }

        public string DefaultTopicGroupID { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ActionType RequestorCoreAction { get; set; }

        public override string GetObjectType() { return "qued"; }
    }
}