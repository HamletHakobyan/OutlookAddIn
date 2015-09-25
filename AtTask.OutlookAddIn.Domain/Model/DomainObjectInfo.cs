using System.Collections.Generic;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class DomainObjectInfo : ApiObjectInfo
    {
        public string[] Flags { get; set; }

        public Dictionary<string, DomainObjectFieldInfo> Fields { get; set; }

        public Dictionary<string, DomainObjectReferenceInfo> References { get; set; }

        public Dictionary<string, DomainObjectReferenceInfo> Collections { get; set; }

        public Dictionary<string, DomainObjectReferenceInfo> Search { get; set; }

        public Dictionary<string, DomainObjectActionInfo> Actions { get; set; }

        public Dictionary<string, string> Queries { get; set; }

        public string[] Operations { get; set; }
    }
}