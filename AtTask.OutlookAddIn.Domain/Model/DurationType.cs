using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public enum DurationType
    {
        /// <summary>
        /// CALCULATED_WORK
        /// </summary>
        W,

        /// <summary>
        /// EFFORT_DRIVEN
        /// </summary>
        D,

        /// <summary>
        /// CALCULATED_ASSIGNMENT
        /// </summary>
        A,

        /// <summary>
        /// SIMPLE
        /// </summary>
        S
    }
}
