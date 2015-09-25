namespace AtTask.OutlookAddIn.Domain.Model
{
    public class ProjectUser : StreamBase
    {
        public const string ObjCodeString = "PRTU";

        public string CustomerID { get; set; }

        public string ProjectID { get; set; }

        public string UserID { get; set; }

        public override string GetObjectType() { return "prtu"; }
    }
}