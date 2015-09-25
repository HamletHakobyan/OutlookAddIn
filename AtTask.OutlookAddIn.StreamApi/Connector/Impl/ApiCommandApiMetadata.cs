using System.Collections.Generic;
using AtTask.OutlookAddIn.StreamApi.Connector.Service;
using AtTask.OutlookAddIn.Domain.Model;
using Newtonsoft.Json;

namespace AtTask.OutlookAddIn.StreamApi.Connector.Impl
{
    internal class ApiCommandApiMetadata : ApiCommand
    {
        public ApiCommandApiMetadata(IStreamApiConnector streamApiConnector, StreamApiHeader streamApiHeader = null)
            : base(streamApiConnector, streamApiHeader)
        {
            this.ReturnType = typeof(Dictionary<string, Dictionary<string, ApiObjectInfo>>);
        }

        protected override string CommandPath
        {
            get
            {
                return ApiConstants.ApiPathMetadata;
            }
        }

        protected override string ApiMethod
        {
            get { return ApiConstants.ApiMethodGet; }
        }

        public Dictionary<string, Dictionary<string, ApiObjectInfo>> Execute()
        {
            ResponseJsonStatusCode resAndCode = MakeRequest();

            JsonEntityRoot<Dictionary<string, Dictionary<string, ApiObjectInfo>>> jsonRoot =
                JsonConvert.DeserializeObject<JsonEntityRoot<Dictionary<string, Dictionary<string, ApiObjectInfo>>>>(resAndCode.Json, internetTimeConverter);
            Dictionary<string, Dictionary<string, ApiObjectInfo>> ret = jsonRoot.Data;
            return ret;
        }
    }
}