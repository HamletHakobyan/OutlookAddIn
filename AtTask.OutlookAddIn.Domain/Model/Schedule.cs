using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class Schedule : EntityBase
    {
        public string TimeZone { get; set; }

        public override string GetObjectType() { return "sched"; }
    }
}
