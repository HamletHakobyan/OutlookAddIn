using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class EventHandlerObject : EntityBase
    {
        public bool? IsActive { get; set; }

        public const string ObjCodeString = "EVNTH";

        public override string GetObjectType() { return "evnth"; }
    }
}