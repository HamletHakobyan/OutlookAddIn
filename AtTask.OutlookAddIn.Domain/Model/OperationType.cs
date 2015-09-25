using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public enum OperationType
    {
        /// <summary>
        /// APPROVAL
        /// </summary>
        APR,

        /// <summary>
        /// UPDATE_CUSTOM_DATA
        /// </summary>
        UDE,

        /// <summary>
        /// STATUS
        /// </summary>
        ST,

        /// <summary>
        /// REQUEST
        /// </summary>
        REQ,

        /// <summary>
        /// APPROVE_REQUEST
        /// </summary>
        APR_REQ,

        /// <summary>
        /// UPDATE_ROLE_GROUP
        /// </summary>
        URG,

        /// <summary>
        /// UPDATE_ACCESSLEVEL
        /// </summary>
        UAL
    }
}
