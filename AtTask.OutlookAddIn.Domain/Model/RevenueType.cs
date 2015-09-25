using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public enum RevenueType
    {
        /// <summary>
        /// Not Billable
        /// </summary>
        NOB,

        /// <summary>
        /// User Hourly
        /// </summary>
        UHR,

        /// <summary>
        /// Role Hourly
        /// </summary>
        RHR,

        /// <summary>
        /// Fixed Hourly
        /// </summary>
        FHR,

        /// <summary>
        /// User Hourly w/Cap
        /// </summary>
        UHC,

        /// <summary>
        /// Role Hourly w/Cap
        /// </summary>
        RHC,

        /// <summary>
        /// User Hourly Plus Fixed
        /// </summary>
        UHF,

        /// <summary>
        /// Role Hourly Plus Fixed
        /// </summary>
        RHF,

        /// <summary>
        /// Fixed Revenue
        /// </summary>
        FRV
    }
}
