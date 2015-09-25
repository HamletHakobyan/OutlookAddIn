using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AtTask.OutlookAddIn.StreamApi.Connector.Service;
using AtTask.OutlookAddIn.Domain.Model;

namespace AtTask.OutlookAddIn.StreamApi.Connector.Impl
{
    internal class ApiCommandLoginViaSessionId : ApiCommandLogin
    {
        public string OtherSessionID { get; set; }

        public ApiCommandLoginViaSessionId(IStreamApiConnector streamApiConnector, string username, string otherSessionID, StreamApiHeader streamApiHeader = null)
            : base(streamApiConnector, streamApiHeader)
        {
            this.OtherSessionID = otherSessionID;
            this.Username = username;
            this.ReturnType = typeof(LoginInfo);

            PostParams.Add(new StringPair(ApiConstants.ApiParamUsername, Username));
            PostParams.Add(new StringPair(ApiConstants.ApiParamSessionId, this.OtherSessionID));

            //if (ApiSessionTimeout.HasValue)
            //{
            //    PostParams.Add(new StringPair(ApiConstants.ApiParamTimeToLive, ApiSessionTimeout.Value.ToString()));
            //}
        }

    }
}
