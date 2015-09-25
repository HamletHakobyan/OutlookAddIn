using System;
using System.Collections.Generic;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class Note : EntityBase
    {
        public const string ObjCodeString = "NOTE";

        public bool? IsMessage { get; set; }

        public bool IsPrivate { get; set; }

        public bool? IsDeleted { get; set; }

        public string Subject { get; set; }

        public bool? EmailUsers { get; set; }

        public string NoteText { get; set; }

        public string NoteObjCode { get; set; }

        public string OwnerID { get; set; }

        public string ObjID { get; set; }

        public string ParentJournalEntryID { get; set; }

        public string ParentNoteID { get; set; }

        public string TopNoteObjCode { get; set; }

        public string TopObjID { get; set; }

        public string DocumentID { get; set; }

        public DateTime? EntryDate { get; set; }

        public User Owner { get; set; }

        public List<NoteTag> Tags { get; set; }

        public override string GetObjectType() { return "note"; }
    }
}