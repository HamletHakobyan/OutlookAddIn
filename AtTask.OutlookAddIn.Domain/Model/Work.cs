using System;
using System.Collections.Generic;
using AtTask.OutlookAddIn.Core.Helpers;
using Newtonsoft.Json;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class Work : NamedEntityBase
    {
        #region Fields

        private DateTime? commitDate;        
        private DateTime? dueDate;
        private DateTime? plannedCompletionDate;
        private DateTime? actualCompletionDate;
        

        private int? priority;
        private List<Priority> allPriorities;
        private int? priorityFlag;

        #endregion Fields

        #region Properties

        public bool? CanStart { get; set; }

        public DateTime? PlannedStartDate { get; set; }

        public DateTime? ActualStartDate { get; set; }

        public DateTime? ProjectedStartDate { get; set; }

        public DateTime? PlannedCompletionDate
        {
            get
            {
                return plannedCompletionDate;
            }
            set
            {
                plannedCompletionDate = value;
                CalculateDueDate();
            }
        }

        public DateTime? CommitDate
        {
            get
            {
                return commitDate;
            }
            set
            {
                commitDate = value;
                CalculateDueDate();
            }
        }

        public DateTime? EntryDate { get; set; }

        public string ConvertedOpTaskName { get; set; }
        public string ConvertedOpTaskOriginatorID { get; set; }

        public DateTime? ActualCompletionDate
        {
            get
            {
                return actualCompletionDate;
            }
            set
            {
                actualCompletionDate = value;
                CalculateDueDate();
            }
        }

        [JsonIgnore]
        public DateTime? DueDate
        {
            get
            {
                if (!dueDate.HasValue)
                {
                    CalculateDueDate();
                }

                return dueDate;
            }
        }

        [JsonIgnore]
        public bool IsDeleted
        {
            get;
            set;
        }

        public DateTime? LastUpdateDate { get; set; }

        public WorkCondition? Condition { get; set; }

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

        public int? ReferenceNumber { get; set; }

        public string Status { get; set; }

        public WorkItem WorkItem { get; set; }

        public Project Project { get; set; }

        public string ProjectID { get; set; }

        public User EnteredBy { get; set; }

        public User SubmittedBy { get; set; }

        public List<Assignment> Assignments { get; set; }

        public List<WorkStatus> DoneStatuses { get; set; }

        public List<WorkStatus> AllStatuses { get; set; }

        public Note LastNote { get; set; }

        public Note LastConditionNote { get; set; }

        public string Description { get; set; }

        public string TeamID { get; set; }

        public Team Team { get; set; }

        public Assignment TeamAssignment { get; set; }

        public List<Update> Updates { get; set; }

        public string CategoryID { get; set; }

        public Category Category { get; set; }

        public int? NumberOfChildren { get; set; }

        public User AssignedTo { get; set; }

        public string AssignedToID { get; set; }

        public Role Role { get; set; }

        public string RoleID { get; set; }

        public string ApprovalProcessID { get; set; }

        public string EnteredByID { get; set; }

        [JsonConverter(typeof(AtTask.OutlookAddIn.Core.JsonConverters.ParameterValuesConverter))]
        public Dictionary<string, object> ParameterValues { get; set; }

        public List<Hour> Hours { get; set; }

        public int? ActualWorkRequired { get; set; }

        public int? WorkRequired { get; set; }

        public List<Document> Documents { get; set; }

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

        public List<AuditType> AuditTypes { get; set; }

        #endregion Properties

        public override string GetObjectType() { return "work"; }

        /// <summary>
        /// Calculates "due date" for this object from other date fields.
        /// Date field priorities are the following:
        ///     1. actualCompletionDate
        ///     2. commitDate
        ///     3. plannedCompletionDate
        /// </summary>
        /// <param name="work"></param>
        private void CalculateDueDate()
        {
            this.dueDate = ActualCompletionDate;
            if (!this.dueDate.HasValue)
            {
                this.dueDate = CommitDate;
                if (!this.dueDate.HasValue)
                {
                    this.dueDate = PlannedCompletionDate;
                }
            }
        }
    }
}