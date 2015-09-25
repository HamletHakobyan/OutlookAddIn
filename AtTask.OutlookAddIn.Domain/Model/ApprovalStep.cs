namespace AtTask.OutlookAddIn.Domain.Model
{
    public class ApprovalStep : NamedEntityBase
    {
        public string ApprovalPathID { get; set; }

        public ApprovalType ApprovalType { get; set; }

        public override string GetObjectType() { return "arvstp"; }
    }
}