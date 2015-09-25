using System.Collections.Generic;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public static class OpTaskTypes
    {
        public static readonly Dictionary<OpTaskTypeEnum, string> OpTaskTypesMap = new Dictionary<OpTaskTypeEnum, string>()
        {
            {OpTaskTypeEnum.ISU, "Issue"},
            {OpTaskTypeEnum.REQ, "Request"},
            {OpTaskTypeEnum.BUG, "Bug Report"},
            {OpTaskTypeEnum.CHO, "Change Order"}
        };
    }
}