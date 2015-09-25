using System;
using System.Text;
using AtTask.OutlookAddIn.StreamApi.Connector.Service;
using AtTask.OutlookAddIn.Domain.Model;
using Newtonsoft.Json;

namespace AtTask.OutlookAddIn.StreamApi.Connector.Impl
{
    internal class ApiCommandMetadata<T> : ApiCommand where T : StreamBase
    {
        public ApiCommandMetadata(IStreamApiConnector streamApiConnector, StreamApiHeader streamApiHeader = null)
            : base(streamApiConnector, streamApiHeader)
        {
            this.ReturnType = typeof(DomainObjectInfo);
        }

        protected override string CommandPath
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(((T)Activator.CreateInstance(typeof(T))).GetObjectType()).Append("/");
                sb.Append(ApiConstants.ApiActionMetadata);
                return sb.ToString();
            }
        }

        protected override string ApiMethod
        {
            get { return ApiConstants.ApiMethodGet; }
        }

        public DomainObjectInfo Execute()// throws StreamAPIErrorException, Exception
        {
            ResponseJsonStatusCode resAndCode = MakeRequest();
            JsonEntityRoot<DomainObjectInfo> jsonRoot = JsonConvert.DeserializeObject<JsonEntityRoot<DomainObjectInfo>>(resAndCode.Json, internetTimeConverter);
            DomainObjectInfo ret = jsonRoot.Data;
            return ret;
        }
    }
}