using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public enum ActionType
    {
        ADD_TASKS,
        ADD_ISSUES,
        ADD_DOCUMENTS,
        ADD_HOURS,
        ADD_EXPENSES,
        EDIT,
        LIMITED_EDIT,
        EDIT_CUSTOMDATA,
        EDIT_APPROVALPROCESS,
        EDIT_ASSIGNMENTS,
        EDIT_FINANCE,
        VIEW,
        DELETE,
        CHANGE_STATUS,        
        VIEW_FINANCE,        
        SHARE,
        SHARE_SYSTEMWIDE
    }
}
