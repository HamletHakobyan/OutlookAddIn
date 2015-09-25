using System;

namespace AtTask.OutlookAddIn.Domain.Model
{
    [Serializable()]
    public class Session : EntityBase
    {
        public string UserID { get; set; }

        public string SessionID { get; set; }

        public string Locale { get; set; }

        public string TimeZone { get; set; }

        public string TimeZoneName { get; set; }

        public string Iso3Country { get; set; }

        public string Iso3Language { get; set; }

        public VersionInfo VersionInformation { get; set; }

        public Currency Currency { get; set; }

        public override string GetObjectType() { return "session"; }
    }
}