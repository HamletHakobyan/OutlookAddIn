using System;
using System.Collections.Generic;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class Update : EntityBase
    {
        public string Message { get; set; }

        public string UpdateObjCode { get; set; }

        public string UpdateObjID { get; set; }

        public string EnteredByID { get; set; }

        public string EnteredByName { get; set; }

        public DateTime? EntryDate { get; set; }

        public string IconPath { get; set; }

        public string RefName { get; set; }

        public string RefObjCode { get; set; }

        public string RefObjID { get; set; }

        public string TopName { get; set; }

        public string TopObjCode { get; set; }

        public string TopObjID { get; set; }

        public string UpdateType { get; set; }

        public Note UpdateNote { get; set; }

        public List<MessageArg> MessageArgs { get; set; }

        public List<Update> NestedUpdates { get; set; }

        public List<Note> Replies { get; set; }

        public override string GetObjectType() { return "updates"; }
    }
}