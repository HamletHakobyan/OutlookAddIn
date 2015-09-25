using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class Permissions
    {
        public List<ActionType?> Actions { get; set; }

        public List<OperationType?> Operations { get; set; }
    }
}
