namespace AtTask.OutlookAddIn.Domain.Model
{
    public class Group : NamedEntityBase
    {
        public string Description { get; set; }

        public override string GetObjectType() { return "group"; }
    }
}