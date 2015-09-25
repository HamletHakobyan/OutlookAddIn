using System;
using System.Text;
using AtTask.OutlookAddIn.StreamApi.Connector.Service;
using AtTask.OutlookAddIn.Domain.Model;
using Newtonsoft.Json;

namespace AtTask.OutlookAddIn.StreamApi.Connector.Impl
{
    internal class ApiCommandDelete<T> : ApiCommand where T : EntityBase
    {
        private string id;

        public string Id
        {
            get { return id; }
            set
            {
                if (value != null)
                {
                    value = value.Trim();
                    if (string.Empty.Equals(value))
                    {
                        value = null;
                    }
                }
                if (value == null)
                {
                    throw new ArgumentException("Null or empty id");
                }
                this.id = value;
            }
        }

        public bool Force { get; set; }

        public ApiCommandDelete(IStreamApiConnector streamApiConnector, string id, bool force = false, StreamApiHeader streamApiHeader = null)
            : base(streamApiConnector, streamApiHeader)
        {
            this.Id = id;
            this.Force = force;
            this.ReturnType = typeof(SuccessResult);

            if (Force)
            {
                PostParams.Add(new StringPair(ApiConstants.ApiParamForce, Boolean.TrueString.ToLower()));
            }
        }

        protected override string CommandPath
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(((T)Activator.CreateInstance(typeof(T))).GetObjectType()).Append("/").Append(id);
                return sb.ToString();
            }
        }

        protected override string ApiMethod
        {
            get { return ApiConstants.ApiMethodDelete; }
        }

        public void Execute()// throws StreamAPIErrorException, Exception
        {
            ResponseJsonStatusCode resAndCode = MakeRequest();
            JsonEntityRoot<SuccessResult> jsonRoot = JsonConvert.DeserializeObject<JsonEntityRoot<SuccessResult>>(resAndCode.Json);
        }
    }
}