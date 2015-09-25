namespace AtTask.OutlookAddIn.Domain.Model
{
    public enum TaskConstraint
    {
        /// <summary>
        /// Fixed Dates
        /// </summary>
        FIXT,

        /// <summary>
        /// Must Start On
        /// </summary>
        MSO,

        /// <summary>
        /// Must Finish On
        /// </summary>
        MFO,

        /// <summary>
        /// As Soon As Possible
        /// </summary>
        ASAP,

        /// <summary>
        /// As Late As Possible
        /// </summary>
        ALAP,

        /// <summary>
        /// Earliest Available Time
        /// </summary>
        EAT,

        /// <summary>
        /// Latest Available Time
        /// </summary>
        LAT,

        /// <summary>
        /// Start No Later Than
        /// </summary>
        SNLT,

        /// <summary>
        /// Start No Earlier Than
        /// </summary>
        SNET,

        /// <summary>
        /// Finish No Later Than
        /// </summary>
        FNLT,

        /// <summary>
        /// Finish No Earlier Than
        /// </summary>
        FNET
    }
}