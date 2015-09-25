using System.Collections.Generic;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class Customer : NamedEntityBase
    {
        public string AdminAcctName { get; set; }

        public string AdminAcctPassword { get; set; }

        public string AccountRepID { get; set; }

        public string ResellerID { get; set; }

        public string TimeZone { get; set; }

        public string Locale { get; set; }

        public string Currency { get; set; }

        public string Status { get; set; }

        public bool? IsDisabled { get; set; }

        public List<Role> Roles { get; set; }

        public bool? NeedLicenseAgreement { get; set; }

        public SecurityModelTypes? SecurityModelType { get; set; }

        public string OnDemandOptions { get; set; }

        public override string GetObjectType() { return "customer"; }
    }
}