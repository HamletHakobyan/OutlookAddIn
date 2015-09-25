using System.Collections.Generic;
using AtTask.OutlookAddIn.StreamApi.Connector.Service;
using AtTask.OutlookAddIn.Domain.Model;
using Newtonsoft.Json;

namespace AtTask.OutlookAddIn.StreamApi.Connector.Impl
{
    internal class ApiCommandApiServerInfo : ApiCommand
    {
        public ApiCommandApiServerInfo(IStreamApiConnector streamApiConnector, StreamApiHeader streamApiHeader = null)
            : base(streamApiConnector, streamApiHeader)
        {
            this.ReturnType = typeof(ServerInfo);
        }

        protected override string CommandPath
        {
            get
            {
                return ApiConstants.ApiPathInfo;
            }
        }

        protected override string ApiMethod
        {
            get { return ApiConstants.ApiMethodGet; }
        }

        public ServerInfo Execute()
        {
            ResponseJsonStatusCode resAndCode = MakeRequest();

            JsonEntityRoot<ServerInfo> jsonRoot =
                JsonConvert.DeserializeObject<JsonEntityRoot<ServerInfo>>(resAndCode.Json);
            ServerInfo ret = jsonRoot.Data;
            return ret;
        }
    }
}