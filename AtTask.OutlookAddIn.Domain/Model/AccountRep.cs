namespace AtTask.OutlookAddIn.Domain.Model
{
    public class AccountRep : EntityBase
    {
        public string AdminLevel { get; set; }

        public string ResellerID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public override string GetObjectType() { return "accountrep"; }
    }
}