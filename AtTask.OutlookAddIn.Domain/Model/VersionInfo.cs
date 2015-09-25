using System;
using System.Collections.Generic;

namespace AtTask.OutlookAddIn.Domain.Model
{
    [Serializable()]
    public class VersionInfo
    {
        public string CurrentAPI { get; set; }

        public string BuildNumber { get; set; }

        public DateTime LastUpdated { get; set; }

        public string Release { get; set; }

        public string Version { get; set; }

        public Dictionary<string, string> ApiVersions { get; set; }
    }
}