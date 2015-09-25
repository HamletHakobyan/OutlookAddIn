using AtTask.OutlookAddIn.StreamApi.Connector.Service;
using AtTask.OutlookAddIn.Domain.Model;
using Newtonsoft.Json;

namespace AtTask.OutlookAddIn.StreamApi.Connector.Impl
{
    internal class ApiCommandLogout : ApiCommand
    {
        public ApiCommandLogout(IStreamApiConnector streamApiConnector, StreamApiHeader streamApiHeader = null)
            : base(streamApiConnector, streamApiHeader)
        {
            ReturnType = typeof(SuccessResult);
        }

        protected override string CommandPath
        {
            get { return ApiConstants.ApiPathLogout; }
        }

        protected override string ApiMethod
        {
            get { return ApiConstants.ApiMethodGet; }
        }

        public void Execute()
        {
            ResponseJsonStatusCode resAndCode = MakeRequest();
            JsonEntityRoot<SuccessResult> jsonRoot = JsonConvert.DeserializeObject<JsonEntityRoot<SuccessResult>>(resAndCode.Json);
        }
    }
}