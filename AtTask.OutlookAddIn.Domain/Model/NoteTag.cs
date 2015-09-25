namespace AtTask.OutlookAddIn.Domain.Model
{
    public class NoteTag : EntityBase
    {
        public string NoteID { get; set; }

        public string ObjID { get; set; }

        public string ObjObjCode { get; set; }

        public override string GetObjectType() { return "notetag"; }
    }
}