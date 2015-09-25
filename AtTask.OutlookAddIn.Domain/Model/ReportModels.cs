namespace AtTask.OutlookAddIn.Domain.Model
{
    public class ReportTeamWorkCount
    {
        public string TeamID { get; set; }

        public int? Count_ID { get; set; }
    }

    public class ReportOpTaskDocumentCount
    {
        public string OpTaskID { get; set; }

        public int? Count_opTaskID { get; set; }
    }

    public class ReportTaskDocumentCount
    {
        public string TaskID { get; set; }

        public int? Count_taskID { get; set; }
    }

    public class ReportProjectDocumentCount
    {
        public string ProjectID { get; set; }

        public int? Count_projectID { get; set; }
    }
}