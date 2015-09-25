using System.Linq;
using AtTask.OutlookAddIn.Utilities;
using AtTask.OutlookAddIn.Domain.Model;

namespace AtTask.OutlookAddIn.Domain.Extensions
{
    public static class WorkExtensions
    {
        /// <summary>
        /// Returns the Priority object for given work, based on AllPriorities and Priority properites.
        /// </summary>
        /// <param name="work"></param>
        /// <returns></returns>
        public static Priority GetPriority(this Work work)
        {
            if (work.AllPriorities == null || !work.Priority.HasValue)
            {
                return null;
            }

            //get first in any case
            return work.AllPriorities.FirstOrDefault<Priority>(p => p.Value == work.Priority.Value.ToString());
        }

        /// <summary>
        /// Returns the Severity object for given issue, based on AllSeverities and Severity properites.
        /// </summary>
        /// <param name="issue"></param>
        /// <returns></returns>
        public static Severity GetSeverity(this OpTask issue)
        {
            if (issue.AllSeverities == null || !issue.Severity.HasValue)
            {
                return null;
            }

            //get first in any case
            return issue.AllSeverities.FirstOrDefault<Severity>(s => s.Value == issue.Severity.Value.ToString());
        }

        /// <summary>
        /// Returns the WorkStatus object for given work, based on AllStatuses and Status properties.
        /// </summary>
        /// <param name="work"></param>
        /// <returns></returns>
        public static WorkStatus GetStatus(this Work work)
        {
            if (work.AllStatuses == null || work.Status == null)
            {
                return null;
            }

            //get first in any case
            return work.AllStatuses.FirstOrDefault<WorkStatus>(s => s.Value == work.Status);
        }

        /// <summary>
        /// Returns the Assignment object of a user with given id if there is such.
        /// </summary>
        /// <param name="work"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Assignment GetUserAssignment(this Work work, string userId)
        {
            Assignment assignment = work.Assignments != null ? work.Assignments.FirstOrDefault<Assignment>(a => a.AssignedToID == userId) : null;

            return assignment;
        }

        /// <summary>
        /// Returns "assigned to" user's name if any, otherwise returns assigned team's name if any, otherwise null.
        /// </summary>
        /// <param name="work"></param>
        /// <returns></returns>
        public static string GetAssigneeName(this Work work)
        {
            if (work.AssignedTo != null)
            {
                return work.AssignedTo.Name;
            }
            else if (work.TeamAssignment != null && work.TeamAssignment.Team != null)
            {
                return work.TeamAssignment.Team.Name;
            }
            else if (work.Role != null)
            {
                return work.Role.Name;
            }

            return null;
        }

        /// <summary>
        /// Returns the total hours of work. If the ownerId is specified returns the total hours
        /// of that owner.
        /// </summary>
        /// <param name="work"></param>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public static double GetTotalHours(this Work work, string ownerId = null)
        {
            double result;
            if (work.Hours != null)
            {
                if (ownerId == null)
                {
                    result = work.Hours.Sum<Hour>(h => (h.Hours.HasValue) ? h.Hours.Value : 0);
                }
                else
                {
                    result = work.Hours.Sum<Hour>(h => (h.OwnerID == ownerId && h.Hours.HasValue) ? h.Hours.Value : 0);
                }
            }
            else
            {
                result = 0;
            }

            return result;
        }

        /// <summary>
        /// Returns custom data parameter value of work.
        /// </summary>
        /// <param name="work"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static object GetParameterValue(this Work work, Parameter param)
        {
            if (work!=null && work.ParameterValues != null && param != null && param.Name != null)
            {
                string paramValueKey = "DE:" + param.Name;
                if (work.ParameterValues.ContainsKey(paramValueKey))
                {
                    return work.ParameterValues[paramValueKey];
                }
            }

            return null;
        }

        /// <summary>
        /// Sets custom data value for the parameter of work.
        /// Value can be also a list of objects (List&lt;object&gt;).
        /// </summary>
        /// <param name="work"></param>
        /// <param name="param"></param>
        /// <param name="value"></param>
        public static void SetParameterValue(this Work work, Parameter param, object value)
        {
            if (param != null && param.Name != null)
            {
                string paramValueKey = "DE:" + param.Name;
                work.ParameterValues[paramValueKey] = value;
            }
        }

        /// <summary>
        /// Returns whether or not the current work and workToCompare has the same list of documents.
        /// Documents are compared by their IDs.
        /// </summary>
        /// <param name="work"></param>
        /// <param name="workToCompare"></param>
        /// <returns></returns>
        public static bool HasSameDocuments(this Work work, Work workToCompare)
        {
            if (workToCompare == null)
            {
                return false;
            }

            int result;
            if (!CompareUtil.CompareObjects(work.Documents, workToCompare.Documents, out result))
            {
                if (result == 0)
                {
                    //if result was 0, then we shall check Documents contents
                    if (work.Documents.Count != workToCompare.Documents.Count)
                    {
                        return false;
                    }

                    foreach (Document doc in work.Documents)
                    {
                        if (!workToCompare.Documents.Any(d => d.ID == doc.ID))
                        {
                            return false;
                        }
                    }

                    return true;
                }

                //this means that Documents are not equal even as object level
                return false;
            }

            return true;
        }
    }
}
