namespace AtTask.OutlookAddIn.Domain.Model
{
    public enum ApprovalType
    {
        /// <summary>
        /// None
        /// </summary>
        NO,

        /// <summary>
        /// Role Based
        /// </summary>
        RB,

        /// <summary>
        /// Team Based
        /// </summary>
        TB,

        /// <summary>
        /// At Least One user
        /// </summary>
        ON,

        /// <summary>
        /// All Users
        /// </summary>
        AL
    }
}