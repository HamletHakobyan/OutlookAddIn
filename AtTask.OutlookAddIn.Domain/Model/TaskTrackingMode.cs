using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public enum TaskTrackingMode
    {
        /// <summary>
        /// USER
        /// </summary>
        USER,

        /// <summary>
        /// ASSUME_ONTIME
        /// </summary>
        ONTM,

        /// <summary>
        /// IGNORE_LATE.
        /// </summary>
        IGNR,

        /// <summary>
        /// AUTO_COMPLETE.
        /// </summary>
        AUTO,

        ///<summary>
        /// PREDECESSOR.
        /// </summary>
        PRED
    }
}
