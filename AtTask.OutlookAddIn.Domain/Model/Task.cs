using System;
using System.Collections.Generic;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class Task : Work
    {
        public const string ObjCodeString = "TASK";

        public bool? Personal { get; set; }

        public int? PlannedDurationMinutes { get; set; }

        public double? PercentComplete { get; set; }

        public TaskConstraint? TaskConstraint { get; set; }

        public int? ActualDurationMinutes { get; set; }

        public Task Parent { get; set; }

        public DurationUnit? DurationUnit { get; set; }

        public DateTime? ConstraintDate { get; set; }

        public string DurationExpression { get; set; }

        public string URL { get; set; }

        public List<Predecessor> Predecessors { get; set; }

        public TaskResourceScope? ResourceScope { get; set; }

        public TaskTrackingMode? TrackingMode { get; set; }

        public DurationType? DurationType { get; set; }

        public RevenueType? RevenueType { get; set; }

        public CostType? CostType { get; set; }

        public override string GetObjectType() { return "task"; }
    }
}