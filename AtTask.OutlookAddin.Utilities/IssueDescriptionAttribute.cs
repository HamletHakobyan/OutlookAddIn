using System.ComponentModel;

namespace AtTask.OutlookAddin.Utilities
{
    public class IssueDescriptionAttribute : DescriptionAttribute
    {
        public IssueDescriptionAttribute(string description)
            : base(description)
        {
        }
    }
}