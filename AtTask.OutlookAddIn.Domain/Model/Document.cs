using System;
using System.Collections.Generic;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class Document : NamedEntityBase
    {
        public const string ObjCodeString = "DOCU";

        public string Handle { get; set; }

        public string DocObjCode { get; set; }

        public string ObjID { get; set; }

        public string Description { get; set; }

        public DateTime LastModDate { get; set; }

        public string DownloadURL { get; set; }

        public DocumentVersion CurrentVersion { get; set; }

        public override string GetObjectType() { return "document"; }
    }
}