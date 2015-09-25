using System.Collections.Generic;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class ApprovalPath : EntityBase
    {
        public string ApprovalProcessID { get; set; }

        public string TargetStatus { get; set; }

        public string RejectedStatus { get; set; }

        public bool ShouldCreateIssue { get; set; }

        public List<ApprovalStep> ApprovalSteps { get; set; }

        public override string GetObjectType() { return "arvpth"; }
    }
}