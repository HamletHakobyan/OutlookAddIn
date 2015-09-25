using System;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class UserNote : EntityBase
    {
        public const string ObjCodeString = "USRNOT";

        public string NoteID { get; set; }

        public string JournalEntryID { get; set; }

        public DateTime? EntryDate { get; set; }

        public JournalEntry JournalEntry { get; set; }

        public Note Note { get; set; }

        public UserNoteEventType EventType { get; set; }

        public override string GetObjectType() { return "notifications"; }
    }
}