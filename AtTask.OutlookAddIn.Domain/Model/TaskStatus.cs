namespace AtTask.OutlookAddIn.Domain.Model
{
    public enum TaskStatus
    {
        /// <summary>
        /// NEW
        /// </summary>
        NEW,

        /// <summary>
        /// INPROGRESS
        /// </summary>
        INP,

        /// <summary>
        /// COMPLETE
        /// </summary>
        CPL,

        /// <summary>
        /// COMPLETE_PENDING_APPROVAL
        /// </summary>
        CPA,

        /// <summary>
        /// COMPLETE_PENDING_ISSUES
        /// </summary>
        CPI
    }
}