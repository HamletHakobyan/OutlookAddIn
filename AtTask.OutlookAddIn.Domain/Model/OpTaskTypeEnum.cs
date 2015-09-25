using System.ComponentModel;
using AtTask.OutlookAddin.Utilities;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public enum OpTaskTypeEnum
    {
        [IssueDescription("Issue")]
        ISU,
        [IssueDescription("Bug Report")]
        BUG,
        [IssueDescription("Request")]
        REQ,
        [IssueDescription("Change Order")]
        CHO
    }
}