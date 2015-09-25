using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class UserAgent : ICloneable
    {
        public const string Prefix = "AtTask-Outlook365";

        public const string PrefixDaemon = "AtTask-Outlook365-Daemon";

        public string AddInPrefix { get; set; }

        public string AddInVersion { get; set; }

        public string OutlookVersion { get; set; }

        public string OSVersion { get; set; }

        public string Is64BitProcess { get; set; }

        public string Is64BitOperatingSystem { get; set; }

        public string NETFramework { get; set; }

        public string CurrentCulture { get; set; }

        public string OutlookCulture { get; set; }

        public UserAgent()
        {
            AddInPrefix = UserAgent.Prefix;
        }

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            if (string.IsNullOrEmpty(AddInPrefix))
            {
                AddInPrefix = UserAgent.Prefix;
            }

            result.Append(AddInPrefix).Append(" v").Append(this.AddInVersion).Append(" (");

            //append OS information
            result.Append(this.OSVersion);
            result.Append(this.Is64BitOperatingSystem);
            result.Append(" ").Append(this.CurrentCulture);

            //append .NET framework information
            result.Append("; .NET ").Append(this.NETFramework);

            //append Outlook information
            result.Append("; Outlook ").Append(this.OutlookVersion);
            result.Append(this.Is64BitProcess);

            if(this.OutlookCulture != null)
            {
                result.Append(" ").Append(this.OutlookCulture);
            }
            
            result.Append(")");
            return result.ToString();            
        }
    }
}
