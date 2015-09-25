using System.Collections.Generic;
using Newtonsoft.Json;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class User : NamedEntityBase
    {
        public const string ObjCodeString = "USER";

        public string Username { get; set; }

        public string Password { get; set; }

        public string HomeGroupID { get; set; }

        public string AccessLevelID { get; set; }

        [JsonProperty("EmailAddr")]
        public string EmailAddress { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool? IsActive { get; set; }

        public string Title { get; set; }

        public string RoleID { get; set; }

        public double? CostPerHour { get; set; }

        public string CompanyID { get; set; }

        public Customer Customer { get; set; }

        public HourType DefaultHourType { get; set; }

        public string HomeTeamID { get; set; }

        public List<Team> Teams { get; set; }

        public List<HourType> HourTypes { get; set; }

		public List<WorkStatus> AllTaskStatuses { get; set; }

        public LicenseType? LicenseType { get; set; }

        public bool? IsAdmin { get; set; }

        public override string GetObjectType() { return "user"; }

        //[JsonIgnore]
        public List<HourType> AllHourTypes { get; set; }

        public List<OpTaskType> OpTaskTypes { get; set; }

        [JsonIgnore]
        public bool IsRequestor
        {
            get { return this.LicenseType == Model.LicenseType.R; }
        }

        [JsonIgnore]
        public bool IsReviewer
        {
            get { return this.LicenseType == Model.LicenseType.C; }
        }

        [JsonIgnore]
        public bool IsFull
        {
            get { return this.LicenseType == Model.LicenseType.F; }
        }
    }
}