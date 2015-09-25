namespace AtTask.OutlookAddIn.Domain.Model
{
    public class Company : NamedEntityBase
    {
        public const string ObjCodeString = "CMPY";

        public override string GetObjectType() { return "company"; }
    }
}