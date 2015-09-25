namespace AtTask.OutlookAddIn.Domain.Model
{
    public class Assignment : EntityBase
    {
        public string AssignedToID { get; set; }

        public AssignmentStatus? Status { get; set; }

        public AssignmentFeedbackStatus? FeedbackStatus { get; set; }

        public string TeamID { get; set; }

        public Team Team { get; set; }

        public string RoleID { get; set; }

        public Role Role { get; set; }

        public User AssignedTo { get; set; }

        public bool? IsPrimary { get; set; }

        public double? AssignmentPercent { get; set; }

        public string TaskID { get; set; }

        public string OpTaskID { get; set; }

        public Task Task { get; set; }

        public OpTask OpTask { get; set; }

        public override string GetObjectType() { return "assignment"; }
    }
}