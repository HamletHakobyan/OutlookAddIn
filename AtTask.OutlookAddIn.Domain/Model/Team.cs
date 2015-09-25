using System.Collections.Generic;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class Team : NamedEntityBase
    {
        public List<TeamMember> TeamMembers { get; set; }

        public string[] TaskStatuses { get; set; }

        public string[] OpTaskBugReportStatuses { get; set; }

        public string[] OpTaskChangeOrderStatuses { get; set; }

        public string[] OpTaskIssueStatuses { get; set; }

        public string[] OpTaskRequestStatuses { get; set; }

        public override string GetObjectType() { return "team"; }
    }
}