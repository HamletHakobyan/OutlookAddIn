using System;
using System.Collections.Generic;
using AtTask.OutlookAddIn.Core.Helpers;
using Newtonsoft.Json;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class Project : NamedEntityBase
    {
        private int? priority;
        private List<Priority> allPriorities;
        private int? priorityFlag;

        public const string ObjCodeString = "PROJ";

        public string GroupID { get; set; }

        public DateTime? ConvertedOpTaskEntryDate { get; set; }
        public string ConvertedOpTaskName { get; set; }
        public string ConvertedOpTaskOriginatorID { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        public List<HourType> HourTypes { get; set; }

        public int? Priority
        {
            get
            {
                return priority;
            }
            set
            {
                priority = value;
                DomainHelper.CalculatePriorityFlag(this.priority, this.allPriorities, out this.priorityFlag);
            }
        }

        /// <summary>
        /// Gets priority flag (0 - none, 1 - high, 2 - urgent).
        /// </summary>
        [JsonIgnore]
        public int? PriorityFlag
        {
            get
            {
                if (!priorityFlag.HasValue)
                {
                    DomainHelper.CalculatePriorityFlag(this.priority, this.allPriorities, out this.priorityFlag);
                }

                return priorityFlag;
            }
        }

        public List<Priority> AllPriorities
        {
            get
            {
                return allPriorities;
            }
            set
            {
                allPriorities = value;
                DomainHelper.CalculatePriorityFlag(this.priority, this.allPriorities, out this.priorityFlag);
            }
        }


        public string Status { get; set; }

        public string Description { get; set; }

        public DateTime? PlannedStartDate { get; set; }

        public DateTime? ActualCompletionDate { get; set; }

        public DateTime? PlannedCompletionDate { get; set; }

        public List<WorkStatus> AllStatuses { get; set; }

        public User Owner { get; set; }

        public string OwnerID { get; set; }

        public User SubmittedBy { get; set; }

        public User EnteredBy { get; set; }

        public bool? Personal { get; set; }

        public string QueueDefID { get; set; }

        public QueueDef QueueDef { get; set; }

        public List<ProjectUser> ProjectUsers { get; set; }

        public Schedule Schedule { get; set; }

        public ProjectScheduleMode? ScheduleMode { get; set; }

        public QueueGroup DefaultQueueGroup { get; set; }

        public override string GetObjectType() { return "project"; }
    }
}