using System;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class DocumentVersion : EntityBase
    {
        public string DocumentID { get; set; }

        public string Ext { get; set; }

        public string FileName { get; set; }

        public string Version { get; set; }

        public string EnteredByID { get; set; }

        public long? DocSize { get; set; }

        public DateTime? EntryDate { get; set; }

        public User EnteredBy { get; set; }

        public override string GetObjectType() { return "documentversion"; }
    }
}