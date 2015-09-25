using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using AtTask.OutlookAddIn.Utilities;
using AtTask.OutlookAddIn.Core.Helpers;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class HelpRequest : NamedEntityBase
    {
        public override string GetObjectType() { return "helprequest"; }

        #region Fields

        private int? priority;
        private List<Priority> allPriorities;
        private int? priorityFlag;

        #endregion Fields

        #region Properties

        public bool IsConverted { get; set; }

        public Work OriginalWork { get; set; }

        public OpTask Original { get; set; }

        public string Status { get; set; }

        public User SubmittedBy { get; set; }

        public User Owner { get; set; }

        public User AssignedTo { get; set; }

        public Team Team { get; set; }

        public Role Role { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? PlannedCompletionDate { get; set; }

        public DateTime? ActualCompletionDate { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        public string RequestOwner { get; set; }

        public string ProjectName { get; set; }

        public int? NumberOfDocuments { get; set; }

        public List<WorkStatus> AllStatuses { get; set; }


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

        #endregion Properties


        #region Constructors

        public HelpRequest(OpTask opTask)
        {
            if (opTask.ResolveProject != null && opTask.ResolveProject.ConvertedOpTaskOriginatorID != null)
            {
                IsConverted = true; //We can assume this because opTask.ResolveTask != null, that is mean this entity is result of conversion from issue (OpTask)
                Original = opTask;
                EntryDate = opTask.EntryDate;
                SubmittedBy = opTask.SubmittedBy;

                //OriginalWork = opTask;

                ObjCode = opTask.ResolveProject.ObjCode;
                ID = opTask.ResolveProject.ID;
                Name = opTask.ResolveProject.Name;
                Owner = opTask.ResolveProject.Owner;
                Status = opTask.ResolveProject.Status;
                Priority = opTask.ResolveProject.Priority;
                PlannedCompletionDate = opTask.ResolveProject.PlannedCompletionDate;
                LastUpdateDate = DateUtil.Max(opTask.LastUpdateDate.Value, opTask.ResolveProject.LastUpdateDate.Value);

                ProjectName = opTask.ResolveProject.Name;
                AllStatuses = opTask.ResolveProject.AllStatuses;
                AllPriorities = opTask.ResolveProject.AllPriorities;

                //objectMetaData = opTask.ResolveProject.Metadata();
                //permissions = opTask.ResolveProject.Permissions();
            }
            else if (opTask.ResolveTask != null && opTask.ResolveTask.ConvertedOpTaskOriginatorID != null)
            {
                IsConverted = true; //We can assume this because opTask.ResolveTask != null, that is mean this entity is result of conversion from issue (OpTask)
                Original = opTask;
                EntryDate = opTask.EntryDate;
                SubmittedBy = opTask.SubmittedBy;

                OriginalWork = opTask.ResolveTask;

                ObjCode = opTask.ResolveTask.ObjCode;
                ID = opTask.ResolveTask.ID;
                Name = opTask.ResolveTask.Name;
                Team = opTask.ResolveTask.Team;
                Role = opTask.ResolveTask.Role;
                Status = opTask.ResolveTask.Status;
                Owner = opTask.ResolveTask.AssignedTo;
                Priority = opTask.ResolveTask.Priority;
                AssignedTo = opTask.ResolveTask.AssignedTo;
                PlannedCompletionDate = opTask.ResolveTask.PlannedCompletionDate;
                LastUpdateDate = DateUtil.Max(opTask.LastUpdateDate, opTask.ResolveTask.LastUpdateDate);

                if (opTask.ResolveTask.Project != null)
                {
                    ProjectName = opTask.ResolveTask.Project.Name;
                }

                AllStatuses = opTask.ResolveTask.AllStatuses;
                AllPriorities = opTask.ResolveTask.AllPriorities;

                //objectMetaData = opTask.ResolveTask().Metadata();
                //permissions = opTask.ResolveTask().Permissions();

            }
            else
            {
                IsConverted = false;

                OriginalWork = opTask;

                ObjCode = opTask.ObjCode;
                ID = opTask.ID;
                Original = opTask;
                Name = opTask.Name;
                Team = opTask.Team;
                Role = opTask.Role;
                Owner = opTask.AssignedTo;
                Status = opTask.Status;
                SubmittedBy = opTask.Owner;
                Priority = opTask.Priority;
                EntryDate = opTask.EntryDate;
                AssignedTo = opTask.AssignedTo;
                LastUpdateDate = opTask.LastUpdateDate;
                PlannedCompletionDate = opTask.PlannedCompletionDate;

                if (opTask.Project != null)
                {
                    ProjectName = opTask.Project.Name;
                }

                AllStatuses = opTask.AllStatuses;
                AllPriorities = opTask.AllPriorities;

                //objectMetaData = opTask.Metadata();
                //permissions = opTask.Permissions();
            }

        }

        public HelpRequest(Task task)
        {
            ObjCode = task.ObjCode;

            OriginalWork = task;

            OpTask original = new OpTask();
            original.Name = task.ConvertedOpTaskName;

            if (!string.IsNullOrEmpty(original.Name))
            {
                IsConverted = true;
            }

            Original = original;
            ID = task.ID;
            Name = task.Name;
            Team = task.Team;
            Role = task.Role;
            Status = task.Status;
            Priority = task.Priority;
            Owner = task.AssignedTo;
            EntryDate = task.EntryDate;
            AssignedTo = task.AssignedTo;
            SubmittedBy = task.SubmittedBy;
            LastUpdateDate = task.LastUpdateDate;
            ActualCompletionDate = task.ActualCompletionDate;
            PlannedCompletionDate = task.PlannedCompletionDate;

            if (task.Project != null)
            {
                ProjectName = task.Project.Name;
            }

            AllStatuses = task.AllStatuses;
            AllPriorities = task.AllPriorities;

            //objectMetaData = task.Metadata();
            //permissions = task.Permissions();
        }

        public HelpRequest(Project project)
        {
            ObjCode = project.ObjCode;

            OpTask original = new OpTask();
            original.Name = project.ConvertedOpTaskName;
            Original = original;

            IsConverted = true;

            ID = project.ID;
            Name = project.Name;
            Owner = project.Owner;
            Status = project.Status;
            Priority = project.Priority;
            SubmittedBy = project.SubmittedBy;
            LastUpdateDate = project.LastUpdateDate;
            EntryDate = project.ConvertedOpTaskEntryDate;
            ActualCompletionDate = project.ActualCompletionDate;
            PlannedCompletionDate = project.PlannedCompletionDate;

            ProjectName = project.Name;

            AllStatuses = project.AllStatuses;
            AllPriorities = project.AllPriorities;

            //objectMetaData = project.Metadata();
            //permissions = project.Permissions();
        }

        #endregion Constructors



        #region Helper Methods

        public string AssigneeName
        {
            get
            {
                if (this.Owner != null)
                {
                    return this.Owner.Name;
                }
                else if (this.Team != null && this.Team != null)
                {
                    return this.Team.Name;
                }
                else if (this.Role != null)
                {
                    return this.Role.Name;
                }
                else
                {
                    return "Unassigned";
                }                
            }
        }

        public WorkStatus GetStatus()
        {
            if (this.AllStatuses == null || this.Status == null)
            {
                return null;
            }

            //get first in any case
            return this.AllStatuses.FirstOrDefault<WorkStatus>(s => s.Value == this.Status);
        }

        #endregion Helper Methods

    }
}