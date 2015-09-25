using System;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class LicenseOrder : EntityBase
    {
        public bool? IsEnterprise { get; set; }

        public bool? IsAPIEnabled { get; set; }

        public bool? IsSOAPEnabled { get; set; }

        public int? FullUsers { get; set; }

        public int? LimitedUsers { get; set; }

        public int? RequestorUsers { get; set; }

        public string CustomerID { get; set; }

        public DateTime? ExpDate { get; set; }

        public override string GetObjectType() { return "licenseorder"; }
    }
}