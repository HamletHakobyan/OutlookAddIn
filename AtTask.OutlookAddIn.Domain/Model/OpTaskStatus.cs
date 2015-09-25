namespace AtTask.OutlookAddIn.Domain.Model
{
    public enum OpTaskStatus
    {
        /// <summary>
        /// NEW
        /// </summary>
        NEW,

        /// <summary>
        /// IN_PROGRESS
        /// </summary>
        INP,

        /// <summary>
        /// AWAITING_FEEDBACK
        /// </summary>
        AWF,

        /// <summary>
        /// ON_HOLD
        /// </summary>
        ONH,

        /// <summary>
        /// REOPENED
        /// </summary>
        ROP,

        /// <summary>
        /// CANNOT_DUPLICATE
        /// </summary>
        CND,

        /// <summary>
        /// WONT_RESOLVE
        /// </summary>
        WTR,

        /// <summary>
        /// RESOLVED
        /// </summary>
        RLV,

        /// <summary>
        /// VERIFIED_COMPLETE
        /// </summary>
        VCP,

        /// <summary>
        /// CLOSED
        /// </summary>
        CLS
    }
}