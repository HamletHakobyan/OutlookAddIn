using System;
using System.Collections.Generic;
using System.Text;
using AtTask.OutlookAddIn.StreamApi.Connector.Service;
using AtTask.OutlookAddIn.Domain.Model;
using Newtonsoft.Json;

namespace AtTask.OutlookAddIn.StreamApi.Connector.Impl
{
    internal class ApiCommandAction<T, U> : ApiCommand where T : EntityBase
    {
        public string Id
        { get; set; }

        private List<StringPair> criteria;

        public List<StringPair> Criteria
        {
            get { return criteria; }
            set
            {
                this.criteria = value;
            }
        }

        private string action;

        public string Action
        {
            get { return this.action; }
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
                    throw new ArgumentException("Null or empty action");
                }

                this.action = value;
            }
        }

        public ApiCommandAction(IStreamApiConnector streamApiConnector, string id, string action, List<StringPair> criteria, StreamApiHeader streamApiHeader = null)
            : base(streamApiConnector, streamApiHeader)
        {
            this.Id = id;
            this.Action = action;
            this.Criteria = criteria;
            this.ReturnType = typeof(ActionResult<U>);

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
                sb.Append(((T)Activator.CreateInstance(typeof(T))).GetObjectType()).Append("/");
                if (Id != null)
                {
                    sb.Append(Id).Append("/");
                }
                sb.Append(Action);
                return sb.ToString();
            }
        }

        protected override string ApiMethod
        {
            get { return ApiConstants.ApiMethodPut; }
        }

        public U Execute()// throws StreamAPIErrorException, Exception
        {
            ResponseJsonStatusCode resAndCode = MakeRequest();
            JsonEntityRoot<ActionResult<U>> jsonRoot = JsonConvert.DeserializeObject<JsonEntityRoot<ActionResult<U>>>(resAndCode.Json, internetTimeConverter, enumConverter);
            ActionResult<U> actionResult = jsonRoot.Data;
            return actionResult.Result;
        }
    }
}