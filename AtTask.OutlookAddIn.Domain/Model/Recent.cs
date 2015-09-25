using System;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class Recent : NamedEntityBase
    {
        public const string ObjCodeString = "RECENT";

        public override string GetObjectType()
        {
            return "recent";
        }

        public string ObjObjCode { get; set; }

        public DateTime LastViewedDate { get; set; }

        public string ObjID { get; set; }
    }
}