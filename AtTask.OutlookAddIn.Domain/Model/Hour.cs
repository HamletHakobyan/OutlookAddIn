using System;
using AtTask.OutlookAddIn.Core.JsonConverters;
using Newtonsoft.Json;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class Hour : EntityBase
    {
        [JsonConverter(typeof(IsoDateConverter))]
        public DateTime? EntryDate { get; set; }

        public double? Hours { get; set; }

        public string OwnerID { get; set; }

        public string ApprovedByID { get; set; }

        public string TaskID { get; set; }

        public string OpTaskID { get; set; }

        public string ProjectID { get; set; }

        public string HourTypeID { get; set; }

        public HourStatus? Status { get; set; }

        public HourType HourType { get; set; }

        public override string GetObjectType() { return "hour"; }
    }
}