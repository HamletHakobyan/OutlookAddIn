using System;
using System.Collections.Generic;
using System.ComponentModel;
using AtTask.OutlookAddin.Utilities;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public enum OpTaskField
    {
        [IssueProperty("Name", typeof(string))]
        [IssueDescription("Subject")]
        [P2PDescription("Subject")]
        NAME = 1,
        [IssueProperty("Description", typeof(string))]
        [IssueDescription("Description")]
        [P2PDescription("Description")]
        DESC,
        [IssueProperty("Url", typeof(string))]
        [IssueDescription("URL")]
        URL,
        [IssueProperty("Priority", typeof(int?))]
        [IssueDescription("Priority")]
        PRIORITY,
        [IssueProperty("Severity", typeof(int?))]
        [IssueDescription("Severity")]
        SEVERITY,
        [IssueProperty("OwnerID",typeof(string))]
        [IssueDescription("Primary Contact")]
        OWNER,
        [IssueProperty("AssignedToID", typeof(string))]
        [IssueDescription("Assigned To")]
        [P2PDescription("Assigned To")]
        ASSIGN,
        [IssueProperty("RoleID", typeof(string))]
        [IssueDescription("Job Role")]
        ROLE,
        [IssueProperty("TeamID", typeof(string))]
        [IssueDescription("Team")]
        TEAM,
        [IssueProperty("WorkRequired", typeof(int?))]
        [IssueDescription("Planned Hours")]
        WORKREQ,
        [IssueProperty("PlannedStartDate", typeof(DateTime?))]
        [IssueDescription("Planned Start Date")]
        STARTDATE,
        [IssueProperty("PlannedCompletionDate", typeof(DateTime?))]
        [IssueDescription("Planned Completion Date")]
        [P2PDescription("Need it done by")]
        ENDDATE,
        [IssueProperty("Status", typeof(string))]
        [IssueDescription("Status")]
        STATUS,
        [IssueProperty("Documents", typeof(List<Document>))]
        [IssueDescription("Documents")]
        [P2PDescription("Documents")]
        DOCATTACH,
    }
}