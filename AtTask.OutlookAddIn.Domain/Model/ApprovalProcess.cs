using System.Collections.Generic;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class ApprovalProcess : NamedEntityBase
    {
        public List<string> ApprovalStatuses { get; set; }

        public ApprovalObjCode ApprovalObjCode { get; set; }

        public List<ApprovalPath> ApprovalPaths { get; set; }

        public override string GetObjectType() { return "arvprc"; }
    }
}