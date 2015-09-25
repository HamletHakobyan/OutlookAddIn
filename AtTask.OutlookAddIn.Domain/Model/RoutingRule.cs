namespace AtTask.OutlookAddIn.Domain.Model
{
    public class RoutingRule : NamedEntityBase
    {
        public string DefaultAssignedToID { get; set; }

        public string DefaultProjectID { get; set; }

        public string DefaultRoleID { get; set; }

        public string DefaultTeamID { get; set; }

        public Team DefaultTeam { get; set; }

        public string ProjectID { get; set; }

        public Role DefaultRole { get; set; }

        public User DefaultAssignedTo { get; set; }

        public override string GetObjectType() { return "rrul"; }
    }
}