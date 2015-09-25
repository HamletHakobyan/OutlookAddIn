namespace AtTask.OutlookAddIn.Domain.Model
{
    public class StepApprover : NamedEntityBase
    {
        public string ApproverStepID { get; set; }

        public string roleID { get; set; }

        public string teamID { get; set; }

        public string userID { get; set; }

        public override string GetObjectType() { return "spapvr"; }
    }
}