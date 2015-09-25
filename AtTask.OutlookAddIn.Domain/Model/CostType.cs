using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public enum CostType
    {
        /// <summary>
        /// No Cost
        /// </summary>
        NOC,

        /// <summary>
        /// Fixed Hourly
        /// </summary>
        FHR,

        /// <summary>
        /// User Hourly
        /// </summary>
        UHR,

        /// <summary>
        /// Role Hourly
        /// </summary>
        RHC
    }
}
