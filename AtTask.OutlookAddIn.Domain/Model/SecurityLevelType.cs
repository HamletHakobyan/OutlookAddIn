using System.ComponentModel;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public enum SecurityLevelType
    {
        [Description("Change Status")] CS,
        [Description("Edit")] E,
        [Description("Edit")] ELU,
        [Description("Edit Custom Form")] UDE,
        [Description("View")] V,
        [Description("Limited Edit")] LE,
        [Description("enum.parametersecuritylevelenum.admin")] A
    }
}