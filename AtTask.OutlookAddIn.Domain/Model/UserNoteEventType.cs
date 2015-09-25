namespace AtTask.OutlookAddIn.Domain.Model
{
    public enum UserNoteEventType
    {
        /// <summary>
        /// wanted you to know...
        /// </summary>
        M,

        /// <summary>
        /// commented on something you did.
        /// </summary>
        U,

        /// <summary>
        /// commented on a update you're in.
        /// </summary>
        T,

        /// <summary>
        /// commented on your help request.
        /// </summary>
        HRC,

        /// <summary>
        /// commented on your work item.
        /// </summary>
        WIC,

        /// <summary>
        /// replied to your work request.
        /// </summary>
        WRR,

        /// <summary>
        /// approved your work item.
        /// </summary>
        A,

        /// <summary>
        /// rejected your work item.
        /// </summary>
        R,

        /// <summary>
        /// changed the planned completion date.
        /// </summary>
        PDC,

        /// <summary>
        /// proposed a new due date
        /// </summary>
        CDC
    }
}