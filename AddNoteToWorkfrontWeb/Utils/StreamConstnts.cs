namespace AddNoteToWorkfrontWeb.Utils
{
    public class StreamConstants
    {
        public const int WorkPriorityNone = 0;
        public const int WorkPriorityLow = 1;
        public const int WorkPriorityMedium = 2;
        public const int WorkPriorityHigh = 3;
        public const int WorkPriorityUrgent = 4;

        public const string WorkingOnSortCommitDateAsc = "dueDate";
        public const string WorkingOnSortProjectNameAsc = "project.name";
        public const string WorkingOnSortWorkItemPriorityAsc = "workItem.priority";

        public const string HelpRequestsSortAssignedTo = "assignedTo:name";
        public const string HelpRequestsSortSubmittedOn = "entryDate";
        public const string HelpRequestsSortRecentlyUpdated = "lastUpdateDate";
        public const string HelpRequestsSortName = "name";
        public const string HelpRequestsSortPriority = "priority";
        public const string HelpRequestsSortQueue = "project:name";
        public const string HelpRequestsSortStatus = "status";

        public const string NamedQueryWorkMyWork = "myWork";
        public const string NamedQueryHelpDeskQueues = "helpDeskQueues";
        public const string NamedQueryRecentHelpDeskQueues = "recentHelpDeskQueues";
        public const string NamedQueryWorkWorkRequests = "workRequests";
        public const string NamedQueryWorkMyAccomplishments = "myAccomplishments";
        public const string NamedQueryQueueTopicGroups = "queueTopicGroups";


        public const string NamedQueryWorkMyOpenRequests = "myOpenRequests";
        public const string NamedQueryWorkMyCompletedRequests = "myCompletedRequests";
        public const string NamedQueryWorkMyAwaitingFeedbackRequests = "myAwaitingFeedbackRequests";

        public const string NamedQueryUserNoteMyNotifications = "myNotificationsQuickList";
        public const string NamedQueryUserNoteMyUnreadNotifications = "myUnreadNotifications";

        public const string NamedQueryTaskStatuses = "taskStatuses";
        public const string NamedQueryOpTaskStatuses = "opTaskStatuses";
        public const string NamedQueryOpTaskPriorities = "opTaskPriorities";
        public const string NamedQueryOpTaskSeverities = "opTaskSeverities";
        public const string NamedQueryOpTaskTypes = "opTaskTypes";

        public const string NamedQueryDefaultSchedule = "defaultSchedule";

        public const string ApiActionAcceptWork = "acceptWork";
        public const string ApiActionReplyToAssignment = "replyToAssignment";
        public const string ApiActionMarkDone = "markDone";
        public const string ApiActionMarkNotDone = "markNotDone";
        public const string ApiActionAssign = "assign";
        public const string ApiActionUnassign = "unassign";
        public const string InternalApiActionResetPassword = "resetPassword";

        public const string ApiActionGetDefaultTaskPriority = "getDefaultTaskPriorityEnum";
        public const string ApiActionGetDefaultOpTaskPriority = "getDefaultOpTaskPriorityEnum";

        public const string ApiActionGetEarliestWorkTimeOfDay = "getEarliestWorkTimeOfDay";
        public const string ApiActionGetNextStartDate = "getNextStartDate";

        public const string ApiActionConvertToTask = "convertToTask";
        public const string ApiActionConvertToProject = "convertToProject";

        public const string UserPrefWorkingOnSorting = "list-myWork-workingOn-sort1";
        public const string UserPrefHelpRequestsSorting = "list-helpRequests-mine-sort1";
        public const string UserPrefPercentCompleteVisible = "updatestatus.percentcomplete.visible";

        public const string CustomerPrefHoursPerDay = "project.mgmt:default.timeline.hoursperday";
        public const string CustomerPrefDaysPerWeek = "project.mgmt:default.timeline.daysperweek";
        public const string CustomerPrefWeeksPerMonth = "project.mgmt:default.timeline.weekspermonth";
        public const string CustomerPrefOpTaskAuditEntryTypes = "project.mgmt:default.optask.auditentrytypes";
        public const string CustomerPrefHelpDeskPeerToPeer = "helpdesk:helpdesk.peertopeer";
        public const string CustomerPrefFutureDatesLogEnabled = "timesheet:default.timesheet.enablefuturedateslog";

        public const string CustomerPrefTaskResourceScope = "project.mgmt:default.task.resourcescope";
        public const string CustomerPrefTaskTrackingMode = "project.mgmt:default.task.trackingmode";
        public const string CustomerPrefTaskDurationType = "project.mgmt:default.task.durationtype";
        public const string CustomerPrefTaskRevenueType = "project.mgmt:default.task.revenuetype";
        public const string CustomerPrefTaskCostType = "project.mgmt:default.task.costtype";
        public const string CustomerPrefTaskAuditEntryTypes = "project.mgmt:default.task.auditentrytypes";
        public const string CustomerPrefTaskStartConstraint = "project.mgmt:default.task.startconstraint";

        public const string ParamID = "ID";
        public const string ParamSessionID = "sessionID";
        public const string ParamTaskID = "taskID";
        public const string ParamOpTaskID = "opTaskID";
        public const string ParamProjectID = "projectID";
        public const string ParamName = "name";
        public const string ParamType = "type";
        public const string ParamLastName = "lastName";
        public const string ParamValue = "value";
        public const string ParamSubObjId = "subObjID";
        public const string ParamIsActive = "isActive";
        public const string ParamEmailAddr = "emailAddr";

        public const string ParamRefNumber = "referenceNumber";
        public const string ParamRefNumberSolr = "referenceNumber_s";
        public const string NameFullSearch = "name_t";



        public const string ParamSubObjCode = "subObjCode";
        public const string ParamSubObjCodeValue = "ASSGN";

        public const string ParamChangeType = "changeType";
        public const string ParamChangeTypeValue = "A";

        public const string ParamPriority = "priority";

        public const string ParamOldPassword = "oldPassword";
        public const string ParamNewPassword = "newPassword";

        public const string NoteSubjectProposed = "Proposed";
        public const string NoteSubjectUpdate = "Update";
        public const string NoteSubjectComment = "Comment";

        public const string ParamObjId = "objID";
        public const string ParamObjCode = "objCode";
        public const string ParamObjObjCode = "objObjCode";
        public const string ParamDocObjCode = "docObjCode";
        public const string ParamLastModDate_Sort = "lastModDate_Sort";
        public const string ParamIsDir = "isDir";

        public const string ParamNoteText = "noteText";
        public const string ParamCommitDate = "commitDate";
        public const string ParamStatus = "status";
        public const string ParamStatus_Mod = "status_Mod";
        public const string ParamAssignmentID = "assignmentID";
        public const string ParamUserID = "userID";

        public const string ParamAssignedToID = "assignedToID";
        public const string ParamIsComplete = "isComplete";
        public const string ParamTeamID = "teamID";

        public const string ParamOwnerID = "ownerID";
        public const string ParamEnteredByID = "enteredByID";
        public const string ParamActualCompletionDate = "actualCompletionDate";
        public const string ParamProjectStatusEquatesWith = "project:statusEquatesWith";
        public const string ParamPersonal = "personal";
        public const string ParamIsHelpDesk = "isHelpDesk";
        public const string ParamConvertedOpTaskOriginatorID = "convertedOpTaskOriginatorID";
        public const string ParamConvertedOpTaskName = "convertedOpTaskName";

        public const string ParamLastViewedDate = "lastViewedDate";
        public const string ParamLastUpdateDate = "lastUpdateDate";

        public const string ParamDate = "date";
        public const string ParamScope = "scope";

        public const string ParamQueueDefId = "queueDefID";

        public const string ParamNoteObjCode = "noteObjCode";

        public const string Sort = "_Sort";
        public const string SortOrderDesc = "desc";
        public const string SortOrderAsc = "asc";

        public const string GroupBy = "_GroupBy";

        public const string AggFunc = "_AggFunc";
        public const string AggFuncCount = "count";
        public const string AggFuncDcount = "dcount";

        public const string CriteriaLimit = "$$LIMIT";
        public const string CriteriaRelevance = "$$relevance";
        public const string ServerNow = "$$NOW";
        public const string UserId = "$$USER.ID";

        public const string Mod = "_Mod";
        public const string ModIn = "in";
        public const string ModNotIn = "notin";
        public const string ModNotEqual = "ne";
        public const string ModLessThanEqual = "lte";
        public const string ModGreatThanEqual = "gte";
        public const string ModNotBlank = "notblank";
        public const string ModIsNull = "isnull";
        public const string ModNotNull = "notnull";
        public const string ModCILike = "cilike";

        public const string OrPattern = "OR:{0}:{1}";

        public const string Percent = "%";

        public const string CrashReportTitlePattern = "{0}: {2} - {1}";
        public const string CrashInfo = "Outlook-Info-Crash";
        public const string CrashProduction = "Outlook-Production-Crash";
        public const string CrashDevelopment = "Outlook-Development-Crash";

        public static readonly string[] ApiFieldsUserInfo =
        {
            "firstName", "lastName", "username", "teams", "title", "companyID", "licenseType", "emailAddr", "isAdmin",
            "defaultHourType", "defaultHourType:isActive",
            "defaultHourType:scope", "hourTypes:isActive", "hourTypes:customerID", "hourTypes:*",
            "customer:securityModelType"
        };

        public static readonly string[] ApiFieldsWorkMyWork =
        {
            "ID", "name", "description", "condition", "priority", "commitDate", "plannedCompletionDate",
            "plannedStartDate", "plannedDurationMinutes", "entryDate", "status", "enteredByID", "permissions",
            "projectID", "project:name", "project:personal", "workItem:priority", "workItem:isDead", "workItem:isDone",
            "workItem:task:percentComplete", "workItem:task:status",
            "lastUpdateDate", "assignments:assignedToID", "assignments:status", "lastNote:noteText",
            "actualWorkRequired", "assignments:roleID",
            "allStatuses:value", "allStatuses:equatesWith", "allStatuses:label", "allStatuses:isPrimary",
            "allStatuses:label", "allPriorities:value", "allPriorities:label", "numberOfChildren",
            "doneStatuses:value", "doneStatuses:equatesWith", "doneStatuses:label", "project:permissions",
            "lastConditionNote:noteText", "lastConditionNote:entryDate", "lastConditionNote:owner:name",
            "hours:hours", "hours:hourTypeID", "hours:hourTypeID", "hours:hourType:isActive", "hours:hourType:name",
            "hours:hourType:scope", "project:hourTypes:*"
        };

        public static readonly string[] ApiFieldsWorkMyWorkAndRequests =
        {
            "ID", "name", "description", "plannedStartDate", "dueDate", "actualCompletionDate","commitDate", "plannedCompletionDate"
        };

        public static readonly string[] ApiFieldsWorkWorkRequests =
        {
            "ID", "name", "description", "condition", "priority", "commitDate", "plannedStartDate", "entryDate",
            "plannedCompletionDate", "status", "canStart", "permissions",
            "projectID", "project:name", "project:personal", "project:permissions", "assignments:assignedToID",
            "enteredBy:name",
            "assignments:roleID", "assignments:feedbackStatus", "allStatuses:value", "allStatuses:equatesWith",
            "allStatuses:label", "allStatuses:label", "allPriorities:value", "allPriorities:label",
            "doneStatuses:value", "doneStatuses:equatesWith", "doneStatuses:label", "numberOfChildren",
            "projectedStartDate", "lastNote:noteText", "lastNote:entryDate", "lastNote:owner:name"
        };

        public static readonly string[] ApiFieldsWorkTeamRequests =
        {
            "ID", "name", "description", "condition", "priority", "commitDate", "plannedStartDate", "entryDate",
            "plannedCompletionDate", "status", "permissions",
            "projectID", "project:name", "project:personal", "project:permissions", "teamAssignment:assignedToID",
            "teamID", "teamAssignment:feedbackStatus", "enteredBy:name", "numberOfChildren",
            "allStatuses:value", "allStatuses:equatesWith", "allStatuses:label",
            "allStatuses:label", "allPriorities:value", "allPriorities:label",
            "doneStatuses:value", "doneStatuses:equatesWith", "doneStatuses:label",
            "lastNote:noteText", "lastNote:entryDate", "lastNote:owner:name"
        };

        public static readonly string[] ApiFieldsOpTaskHelpRequest =
        {
            "ID", "name", "condition", "priority", "status",

            "commitDate", "plannedStartDate", "entryDate", "plannedCompletionDate", "actualCompletionDate",
            "lastUpdateDate",

            "roleID", "role:name", "teamID", "team:name", "enteredBy:name",

            "lastNote:noteText", "lastNote:entryDate", "lastNote:ownerID", "lastNote:owner:name",

            "allPriorities:value", "allPriorities:label",

            "allStatuses:value", "allStatuses:equatesWith", "allStatuses:label", "allStatuses:isPrimary",

            "project:name", "project:personal", "project:permissions", "submittedBy:name", "owner:name",

            "assignedToID", "assignedTo:name", "assignments:roleID", "assignments:status",
            "assignments:assignedTo:name", "assignments:assignedToID", "assignments:feedbackStatus",

            "teamAssignment:teamID", "teamAssignment:team:name", "teamAssignment:feedbackStatus",
            "teamAssignment:status",

            "resolvingObjCode", "resolvingObjID",

            "resolveProject:convertedOpTaskName", "resolveProject:convertedOpTaskOriginatorID",
            "resolveProject:name", "resolveProject:status", "resolveProject:priority",
            "resolveProject:owner:name", "resolveProject:plannedCompletionDate", "resolveProject:lastUpdateDate",
            "resolveProject:allPriorities:value", "resolveProject:allPriorities:label",
            "resolveProject:allStatuses:value", "resolveProject:allStatuses:equatesWith",
            "resolveProject:allStatuses:label", "resolveProject:allStatuses:isPrimary",

            "resolveTask:convertedOpTaskName", "resolveTask:convertedOpTaskOriginatorID",
            "resolveTask:name", "resolveTask:status", "resolveTask:priority", "resolveTask:assignedTo:name",
            "resolveTask:plannedCompletionDate", "resolveTask:lastUpdateDate", "resolveTask:team:name",
            "resolveTask:role:name",
            "resolveTask:allPriorities:value", "resolveTask:allPriorities:label",
            "resolveTask:allStatuses:value", "resolveTask:allStatuses:equatesWith", "resolveTask:allStatuses:label",
            "resolveTask:allStatuses:isPrimary",
        };


        public static readonly string[] ApiFieldsTaskHelpRequest =
        {
            "ID", "name", "priority", "status",

            "entryDate", "plannedCompletionDate", "actualCompletionDate", "lastUpdateDate",

            "roleID", "role:name", "teamID", "team:name", "submittedBy:name", "assignments:assignedToID",
            "assignedTo:name",

            "allPriorities:value", "allPriorities:label",

            "allStatuses:value", "allStatuses:equatesWith", "allStatuses:label", "allStatuses:isPrimary",

            "convertedOpTaskName", "convertedOpTaskOriginatorID", "project:name", "project:permissions"
        };

        public static readonly string[] ApiFieldsProjectHelpRequest =
        {
            "ID", "name", "priority", "status",

            "entryDate", "plannedCompletionDate", "actualCompletionDate", "lastUpdateDate", "permissions",

            "allPriorities:value", "allPriorities:label",

            "allStatuses:value", "allStatuses:equatesWith", "allStatuses:label", "allStatuses:isPrimary",

            "owner:name", "submittedBy:name", "convertedOpTaskName",
        };


        public static readonly string[] ApiFieldsTask =
        {
            "ID", "name", "condition", "priority", "commitDate", "projectID", "personal", "percentComplete",
            "description", "plannedStartDate", "entryDate", "status", "actualWorkRequired", "role:name",
            "project:name", "project:personal", "project:permissions",
            "workItem:priority", "workItem:isDone", "plannedCompletionDate", "actualCompletionDate",
            "assignments:assignedToID", "assignments:status", "assignments:isPrimary", "assignments:assignedTo:name",
            "assignments:assignedTo:title", "assignments:role:name", "assignments:roleID", "teamAssignment:*",
            "lastNote:noteText", "teamID", "team:*",

            "allStatuses:isPrimary", "allStatuses:value", "allStatuses:equatesWith", "allStatuses:label",

            "doneStatuses:value", "doneStatuses:equatesWith", "doneStatuses:label", "permissions", "URL",
            "lastConditionNote:noteText", "lastConditionNote:entryDate", "lastConditionNote:owner:name",
            "allPriorities:value", "allPriorities:label", "enteredBy:name", "lastUpdateDate", "referenceNumber",
            "hours:hours", "hours:hourTypeID", "hours:hourType:isActive", "hours:hourType:name", "hours:hourType:scope",
            "hours:ownerID", "numberOfChildren",
            "documents:ID", "project:hourTypes:*"
        };

        public static readonly string[] ApiFieldsOpTask =
        {
            "ID", "name", "condition", "priority", "severity", "commitDate", "projectID", "owner:name", "enteredByID",
            "description", "plannedStartDate", "entryDate", "status", "actualWorkRequired", "role:name",
            "project:name", "project:personal", "project:permissions", "workItem:priority", "workItem:isDone",
            "plannedCompletionDate", "actualCompletionDate",
            "assignments:assignedToID", "assignments:status", "assignments:isPrimary", "assignments:assignedTo:name",
            "assignments:assignedTo:title", "assignments:role:name", "assignments:roleID",
            "lastNote:noteText", "teamID", "team:*", "allStatuses:value", "allStatuses:equatesWith", "allStatuses:label",
            "teamAssignment:*",
            "doneStatuses:value", "doneStatuses:equatesWith", "doneStatuses:label", "permissions", "url",
            "lastConditionNote:noteText", "lastConditionNote:entryDate", "lastConditionNote:owner:name",
            "allPriorities:value", "allPriorities:label", "enteredBy:name", "lastUpdateDate", "referenceNumber",
            "hours:hours", "hours:hourTypeID", "hours:hourType:isActive", "hours:hourType:name", "hours:hourType:scope",
            "hours:ownerID",
            "documents:ID", "resolvingObjCode", "resolvingObjID", "project:hourTypes:*"
        };

        public static readonly string[] ApiFieldsLastNote =
        {
            "noteText", "owner:name", "entryDate", "ownerID", "isPrivate"
        };

        public static readonly string[] ApiFieldsComment =
        {
            "noteText", "owner:name", "entryDate"
        };

        public static readonly string[] ApiFieldsUpdateStatus =
        {
            "ID", "name", "condition", "priority", "commitDate", "description", "plannedStartDate", "entryDate",
            "status", "project:name", "project:personal", "workItem:priority", "plannedCompletionDate",
            "assignments:assignedToID", "assignments:status", "lastNote:noteText", "lastNote:owner:name",
            "assignments:roleID",
            "allStatuses:value", "allStatuses:equatesWith", "allStatuses:label",
            "doneStatuses:value", "doneStatuses:equatesWith", "doneStatuses:label",
            "lastConditionNote:noteText", "lastConditionNote:entryDate", "lastConditionNote:owner:name",
            "lastNote:noteText", "lastNote:entryDate", "lastNote:owner:name", "numberOfChildren"
        };

        public static readonly string[] ApiFieldsUpdateStatusUpdates =
        {
            "ID", "name", "condition", "priority", "commitDate", "description", "plannedStartDate", "entryDate",
            "status", "project:name", "project:personal", "workItem:priority",
            "plannedCompletionDate", "projectedStartDate", "plannedStartDate",
            "assignments:assignedToID", "assignments:status", "lastNote:noteText", "lastNote:owner:name",
            "assignments:roleID",
            "allStatuses:value", "allStatuses:equatesWith", "allStatuses:label", "actualWorkRequired",
            "doneStatuses:value", "doneStatuses:equatesWith", "doneStatuses:label",
            "updates:*", "updates:nestedUpdates:*", "updates:replies:*",
            "lastConditionNote:noteText", "lastConditionNote:entryDate", "lastConditionNote:owner:name",
            "numberOfChildren"
        };

        public static readonly string[] ApiFieldsId = { "ID" };

        public static readonly string[] ApiFieldsAll = { "*" };

        public static readonly string[] ApiFieldsWorkDocuments =
        {
            "description", "lastModDate", "downloadURL", "currentVersion:enteredBy", "currentVersion:entryDate",
            "currentVersion:docSize", "currentVersion:ext"
        };

        public static readonly string[] ApiFieldsWorkDocumentNotes =
        {
            "noteText", "owner", "ownerID", "entryDate", "documentID"
        };

        public static readonly string[] ApiFieldsWorkUpdates =
        {
            "updates:*", "updates:nestedUpdates:*", "updates:replies:*", "updates:replies:tags:objID",
            "updates:replies:tags:objObjCode", "updates:replies:owner:name",
            "updates:updateNote:tags:objID", "updates:updateNote:tags:objObjCode", "updates:updateNote:isPrivate",
            "updates:updateNote:isDeleted", "updates:updateNote:ownerID", "numberOfChildren"
        };

        public static readonly string[] ApiFieldsWorkHours =
        {
            "actualWorkRequired", "hours:entryDate", "hours:hours", "hours:hourTypeID", "hours:hourType:isActive",
            "hours:hourType:name", "hours:hourType:scope", "hours:ownerID", "hours:approvedByID", "hours:taskID",
            "hours:opTaskID", "hours:projectID", "hours:status",
            "project:hourTypes:*"
        };

        public static readonly string[] ApiFieldsWorkLoggedHour =
        {
            "actualWorkRequired", "updates:*", "updates:nestedUpdates:*", "updates:replies:*",
            "updates:replies:tags:objID", "updates:replies:tags:objObjCode", "updates:replies:owner:name",
            "updates:updateNote:tags:objID", "updates:updateNote:tags:objObjCode", "updates:updateNote:isPrivate",
            "hours:entryDate", "hours:hours", "hours:hourTypeID", "hours:hourType:isActive", "hours:hourType:name",
            "hours:hourType:scope", "hours:ownerID", "hours:approvedByID", "hours:taskID", "hours:opTaskID", "hours:projectID",
            "hours:status", "numberOfChildren", "project:hourTypes:*"
        };

        public static readonly string[] ApiFieldsWorkDetailsFullBase =
        {
            "ID", "name", "description", "priority", "workRequired", "actualWorkRequired", "plannedStartDate",
            "actualStartDate", "projectedStartDate",
            "condition", "commitDate", "lastNote:noteText", "lastNote:entryDate", "lastNote:owner:name", "status",
            "project:personal",
            "lastConditionNote:noteText", "lastConditionNote:entryDate", "lastConditionNote:owner:name",
            "plannedCompletionDate", "actualCompletionDate", "permissions", "status", "workItem:isDead",
            "workItem:isDone", "workItem:priority",
            "assignments:assignedToID", "assignments:status", "assignments:isPrimary", "assignments:assignedTo:name",
            "assignments:assignedTo:title", "assignments:role:name", "assignments:roleID",
            "doneStatuses:value", "doneStatuses:equatesWith", "doneStatuses:label", "role:name",
            "allStatuses:value", "allStatuses:label", "allPriorities:value", "allPriorities:label", "hours:hours",
            "hours:hourTypeID", "hours:hourType:isActive", "hours:hourType:name", "hours:hourType:scope",
            "hours:ownerID",
            "category:categoryParameters:displayOrder", "category:categoryParameters:parameterGroup",
            "category:categoryParameters:rowShared", "category:categoryParameters:viewSecurityLevel",
            "category:categoryParameters:parameter:dataType", "category:categoryParameters:parameter:displayType",
            "category:categoryParameters:parameter:parameterOptions:label",
            "category:categoryParameters:parameter:parameterOptions:value",
            "category:categoryParameters:parameter:parameterOptions:displayOrder",
            "parameterValues", "project:hourTypes:scope", "project:permissions"
        };

        public static readonly string[] ApiFieldsTaskDetailsPart =
        {
            "allPriorities:value", "allPriorities:label", "enteredBy:name", "enteredBy:companyID", "lastUpdateDate",
            "referenceNumber", "entryDate",
            "assignments:assignedToID", "assignments:status", "assignments:isPrimary", "assignments:assignedTo:name",
            "assignments:assignedTo:title", "assignments:role:name",
            "lastNote:noteText", "teamID", "team:*", "allStatuses:value", "allStatuses:equatesWith", "allStatuses:label",
            "allStatuses:isPrimary", "teamAssignment:*",
            "doneStatuses:value", "doneStatuses:equatesWith", "doneStatuses:label", "permissions",
            "actualCompletionDate", "status",
            "lastConditionNote:noteText", "lastConditionNote:entryDate", "lastConditionNote:owner:name", "role:name",
            "percentComplete",

            "project:owner:name", "parent:name", "URL", "durationUnit", "plannedDurationMinutes",
            "actualDurationMinutes", "numberOfChildren"
        };

        public static readonly string[] ApiFieldsOpTaskDetailsPart =
        {
            "assignments:assignedToID", "assignments:status", "assignments:isPrimary", "assignments:assignedTo:name",
            "assignments:assignedTo:title", "assignments:role:name", "assignments:roleID",
            "lastNote:noteText", "teamID", "team:*", "allStatuses:value", "allStatuses:equatesWith", "allStatuses:label",
            "teamAssignment:*",
            "doneStatuses:value", "doneStatuses:equatesWith", "doneStatuses:label", "permissions",
            "actualCompletionDate", "status",
            "lastConditionNote:noteText", "lastConditionNote:entryDate", "lastConditionNote:owner:name", "owner:name",
            "resolvingObjCode", "resolvingObjID",

            "url", "severity", "owner:name", "allSeverities:value", "allSeverities:label", "entryDate", "role:name",
            "allPriorities:value", "allPriorities:label", "enteredBy:name", "enteredBy:companyID", "lastUpdateDate",
            "referenceNumber"
        };

        public static readonly string[] ApiFieldsWorkDocumentsFromWork =
        {
            "documents:description", "documents:lastModDate", "documents:currentVersion:enteredBy",
            "documents:currentVersion:docSize",
            "documents:currentVersion:ext", "documents:notes:noteText", "documents:notes:owner",
            "documents:notes:entryDate"
        };

        public static readonly string[] ApiFieldsDocumentsBySearch =
        {
            "description", "lastModDate", "downloadURL", "currentVersion:enteredBy", "currentVersion:entryDate",
            "currentVersion:docSize", "currentVersion:ext",
            "notes:noteText", "notes:owner", "notes:ownerID", "notes:entryDate"
        };

        public static readonly string[] ApiFieldsAssignmentWorkItem = { "task:workItem", "opTask:workItem" };

        public static readonly string[] ApiFieldsHelpDeskQueueDef =
        {
            "ID", "permissions",
            "queueDef:ID", "queueDef:projectID", "queueDef:addOpTaskStyle", "queueDef:defaultCategoryID",
            "queueDef:allowedOpTaskTypes", "queueDef:requestorCoreAction",
            "queueDef:defaultRoute:ID", "queueDef:defaultRoute:name", "queueDef:defaultRoute:objCode",
            "queueDef:defaultRoute:defaultAssignedTo",
            "queueDef:defaultRoute:defaultRole",
            "queueDef:defaultRoute:defaultTeam",
            "queueDef:defaultDurationMinutes", "queueDef:defaultDurationUnit",
            "queueDef:visibleOpTaskFields", "queueDef:allowedLegacyQueueTopicIDs",
            "queueDef:queueTopics:parentTopicGroupID", "queueDef:queueTopics:description",
            "queueDef:defaultTopicGroupID",
            "queueDef:queueTopics:allowedOpTaskTypes", "queueDef:queueTopics:defaultCategoryID",
            "queueDef:queueTopics:parentTopicID",
            "queueDef:queueTopics:defaultRoute:ID", "queueDef:queueTopics:defaultRoute:name",
            "queueDef:queueTopics:defaultRoute:objCode",
            "queueDef:queueTopics:defaultRoute:defaultRole",
            "queueDef:queueTopics:defaultRoute:defaultTeam",
            "queueDef:queueTopics:defaultRoute:defaultAssignedTo",
            "queueDef:queueTopics:defaultApprovalProcessID",
            "queueDef:queueTopics:defaultDurationMinutes", "queueDef:queueTopics:defaultDurationUnit"
        };

        public static readonly string[] ApiFieldsHelpDeskQueues =
        {
            "ID", "name", "description", "queueDefID"
        };

        public static readonly string[] ApiFieldsHelpDeskQueuesId =
        {
            "ID"
        };

        public static readonly string[] ApiFieldsQueueGroups =
        {
            "ID", "name", "description", "parentID"
        };

        public static readonly string[] ApiFieldsUpdatableEntity =
        {
            "ID", "name"
        };

        public static readonly string[] ApiFieldsProjects =
        {
            "ID", "name", "queueDef:ID", "queueDef:allowedOpTaskTypes", "schedule:timeZone", "scheduleMode",
            "plannedStartDate", "plannedCompletionDate"
        };

        public static readonly string[] ApiFieldsRoutingRule =
        {
            "ID", "name", "customerID", "customer", "defaultAssignedToID",
            "defaultAssignedTo", "defaultAssignedTo:ID", "defaultAssignedTo:name",
            "defaultRole", "defaultRole:ID", "defaultRole:name",
            "defaultRoleID", "defaultRole", "defaultTeamID", "defaultTeam"
        };

        public static readonly string[] ApiFieldsCategory =
        {
            "groupID", "categoryParameters:displayOrder", "categoryParameters:rowShared",
            "categoryParameters:isRequired", "categoryParameters:securityLevel",
            "categoryParameters:parameterGroup:description",
            "categoryParameters:parameter:dataType", "categoryParameters:parameter:displayType",
            "categoryParameters:parameter:displaySize", "categoryParameters:parameter:description",
            "categoryParameters:parameter:parameterOptions:label", "categoryParameters:parameter:parameterOptions:value",
            "categoryParameters:parameter:parameterOptions:displayOrder",
            "categoryParameters:parameter:parameterOptions:isDefault",
            "categoryParameters:parameter:parameterOptions:isHidden",
            "categoryCascadeRules:nextParameterID", "categoryCascadeRules:nextParameterGroupID",
            "categoryCascadeRules:otherwiseParameterID",
            "categoryCascadeRules:ruleType", "categoryCascadeRules:toEndOfForm",
            "categoryCascadeRules:categoryCascadeRuleMatches:parameterID",
            "categoryCascadeRules:categoryCascadeRuleMatches:value",
            "categoryCascadeRules:categoryCascadeRuleMatches:matchType"
        };

        public static readonly string[] ApiFieldsQueueTopic =
        {
            "name", "defaultCategoryID", "allowedOpTaskTypes",
            "defaultCategory:categoryParameters:displayOrder", "defaultCategory:categoryParameters:parameterGroup",
            "defaultCategory:categoryParameters:rowShared", "defaultCategory:categoryParameters:isRequired",
            "defaultCategory:categoryParameters:parameter:dataType",
            "defaultCategory:categoryParameters:parameter:displayType",
            "defaultCategory:categoryParameters:parameter:displaySize",
            "defaultCategory:categoryParameters:parameter:parameterOptions:label",
            "defaultCategory:categoryParameters:parameter:parameterOptions:value",
            "defaultCategory:categoryParameters:parameter:parameterOptions:displayOrder",
            "defaultCategory:categoryParameters:parameter:parameterOptions:isDefault",
            "defaultCategory:categoryParameters:parameter:parameterOptions:isHidden"
        };

        public static readonly string[] ApiFieldsQueueDef =
        {
            "defaultCategoryID",
            "defaultCategory:categoryParameters:displayOrder", "defaultCategory:categoryParameters:parameterGroup",
            "defaultCategory:categoryParameters:rowShared", "defaultCategory:categoryParameters:isRequired",
            "defaultCategory:categoryParameters:parameter:dataType",
            "defaultCategory:categoryParameters:parameter:displayType",
            "defaultCategory:categoryParameters:parameter:displaySize",
            "defaultCategory:categoryParameters:parameter:parameterOptions:label",
            "defaultCategory:categoryParameters:parameter:parameterOptions:value",
            "defaultCategory:categoryParameters:parameter:parameterOptions:displayOrder",
            "defaultCategory:categoryParameters:parameter:parameterOptions:isDefault",
            "defaultCategory:categoryParameters:parameter:parameterOptions:isHidden"
        };

        public static readonly string[] ApiFieldsCustomEnum =
        {
            "label", "value", "valueAsInt", "ID", "isPrimary"
        };

        public static readonly string[] ApiFieldsOpTaskTypes =
        {
            "label", "value"
        };

        public static readonly string[] ApiFieldsSchedule =
        {
            "timeZone"
        };
        public static readonly string[] ApiFieldsRecent =
        {
            "ID", "name", "objID", "objObjCode", "lastViewedDate"
        };
    }
}