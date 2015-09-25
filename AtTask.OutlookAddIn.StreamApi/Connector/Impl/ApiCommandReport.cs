using System;
using System.Collections.Generic;
using System.Text;
using AtTask.OutlookAddIn.StreamApi.Connector.Service;
using AtTask.OutlookAddIn.Domain.Model;
using Newtonsoft.Json;

namespace AtTask.OutlookAddIn.StreamApi.Connector.Impl
{
    internal class ApiCommandReport<T, U> : ApiCommand where T : StreamBase
    {
        private List<StringPair> criteria;

        public List<StringPair> Criteria
        {
            get { return criteria; }
            set
            {
                this.criteria = value;
            }
        }

        public ApiCommandReport(IStreamApiConnector streamApiConnector, List<StringPair> criteria, StreamApiHeader streamApiHeader = null)
            : base(streamApiConnector, streamApiHeader)
        {
            this.Criteria = criteria;
            this.ReturnType = typeof(U);

            if (Criteria != null)
            {
                PostParams.AddRange(Criteria);
            }
        }

        protected override string CommandPath
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(((T)Activator.CreateInstance(typeof(T))).GetObjectType()).Append("/").Append(ApiConstants.ApiActionReport);
                return sb.ToString();
            }
        }

        protected override string ApiMethod
        {
            get { return ApiConstants.ApiMethodGet; }
        }

        public U Execute()// throws StreamAPIErrorException, Exception
        {
            ResponseJsonStatusCode resAndCode = MakeRequest();
            JsonEntityRoot<U> jsonRoot = JsonConvert.DeserializeObject<JsonEntityRoot<U>>(resAndCode.Json, internetTimeConverter);
            U ret = jsonRoot.Data;
            return ret;
        }
    }
}