using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AtTask.OutlookAddIn.Domain.Model;

namespace AtTask.OutlookAddIn.Core.Helpers
{
    public static class DomainHelper
    {
        public static void CalculatePriorityFlag(int? priority, List<Priority> allPriorities, out int? priorityFlag)
        {
            if (!priority.HasValue || allPriorities == null || allPriorities.Count == 0)
            {
                priorityFlag = null;
                return;
            }

            int urgentValue = int.MinValue;
            int highValue = int.MinValue;
            foreach (Priority tmpPriority in allPriorities)
            {
                int tmpValue;
                if (int.TryParse(tmpPriority.Value, out tmpValue))
                {
                    if (tmpValue > urgentValue)
                    {
                        highValue = urgentValue;
                        urgentValue = tmpValue;
                    }
                    else if (tmpValue > highValue)
                    {
                        highValue = tmpValue;
                    }
                }
                else
                {
                    tmpValue = int.MinValue;
                }
            }

            if (priority.Value == urgentValue)
            {
                priorityFlag = 2;
            }
            else if (priority.Value == highValue)
            {
                priorityFlag = 1;
            }
            else
            {
                priorityFlag = 0;
            }
        }
    }
}
