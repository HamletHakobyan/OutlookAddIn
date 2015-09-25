namespace AtTask.OutlookAddIn.Domain.Model
{
    public class TeamMember : StreamBase
    {
        public string TeamID { get; set; }

        public string UserID { get; set; }

        public override string GetObjectType() { return "teammember"; }
    }
}