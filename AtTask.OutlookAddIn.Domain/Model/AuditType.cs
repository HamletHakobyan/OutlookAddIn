namespace AtTask.OutlookAddIn.Domain.Model
{
    public enum AuditType
    {
        /// <summary>
        /// Status Change
        /// </summary>
        ST,

        /// <summary>
        /// Attachment Action
        /// </summary>
        AA,

        /// <summary>
        /// Scope Change
        /// </summary>
        SC,

        /// <summary>
        /// Note
        /// </summary>
        NO,

        /// <summary>
        /// General Edit
        /// </summary>
        GE,

        /// <summary>
        /// Combined Entry
        /// </summary>
        CM
    }
}