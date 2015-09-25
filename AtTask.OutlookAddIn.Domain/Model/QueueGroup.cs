using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class QueueGroup : NamedEntityBase
    {
        public string Description { get; set; }
        public List<QueueTopic> QueueTopics { get; set; }
        public List<QueueGroup> QueueTopicGroups { get; set; }

        public string ParentID { get; set; }
        public override string GetObjectType()
        {
            return "QUETGP";
        }
    }
}
