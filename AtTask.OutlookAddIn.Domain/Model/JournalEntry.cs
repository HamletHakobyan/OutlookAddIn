using System;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class JournalEntry : EntityBase
    {
        public const string ObjCodeString = "JRNLE";

        public DateTime? EntryDate { get; set; }

        public override string GetObjectType()
        {
            //return "journalentry";
            return "jrnle";
        }
    }
}