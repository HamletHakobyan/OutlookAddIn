using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AtTask.OutlookAddIn.StreamApi.Connector.Service;
using AtTask.OutlookAddIn.Domain.Model;
using Newtonsoft.Json;

namespace AtTask.OutlookAddIn.StreamApi.Connector.Impl
{
    internal class ApiCommandGet<T> : ApiCommand where T : EntityBase
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
                    throw new ArgumentException("Invalid id");
                }
                this.id = value;
            }
        }

        private string[] ids;

        public string[] Ids
        {
            get { return this.ids; }
            set
            {
                string[] tmp = NormalizeStringArray(value);
                if (tmp == null || tmp.Length == 0)
                {
                    throw new ArgumentException("Invalid ids");
                }
                this.ids = tmp;
            }
        }

        private string[] fields;

        public string[] Fields
        {
            get { return this.fields; }
            set
            {
                if (value != null)
                {
                    string[] tmp = NormalizeStringArray(value);
                    if (tmp == null)
                    {
                        throw new ArgumentException("Invalid fields array");
                    }
                    this.fields = tmp;
                }
                else
                {
                    this.fields = value;
                }
            }
        }

        public ApiCommandGet(IStreamApiConnector streamApiConnector, string id, string[] fields, StreamApiHeader streamApiHeader = null)
            : this(streamApiConnector, new string[] { id }, fields, streamApiHeader)
        {
            this.Id = id;
        }

        public ApiCommandGet(IStreamApiConnector streamApiConnector, string[] ids, string[] fields, StreamApiHeader streamApiHeader = null)
            : base(streamApiConnector, streamApiHeader)
        {
            this.Ids = ids;
            this.Fields = fields;
            this.ReturnType = typeof(T);

            string idsParam = ComposeCommaSeparatedString(Ids);
            PostParams.Add(new StringPair(ApiConstants.ApiParamId, idsParam));

            string fieldsParam = ComposeCommaSeparatedString(Fields);
            if (fieldsParam != null)
            {
                PostParams.Add(new StringPair(ApiConstants.ApiParamFields, fieldsParam));
            }
        }

        protected override string CommandPath
        {
            get { return ((T)Activator.CreateInstance(typeof(T))).GetObjectType(); }
        }

        protected override string ApiMethod
        {
            get { return ApiConstants.ApiMethodGet; }
        }

        public List<T> Execute()// throws StreamAPIErrorException, Exception
        {
            ResponseJsonStatusCode resAndCode = MakeRequest();
            List<T> ret = null;
            if (Id != null || Ids.Length == 1)
            {
                JsonEntityRoot<T> jsonRoot = JsonConvert.DeserializeObject<JsonEntityRoot<T>>(resAndCode.Json, internetTimeConverter, enumConverter);
                T elem = jsonRoot.Data;
                ret = new List<T>();
                ret.Add(elem);
            }
            else
            {
                JsonEntityRoot<List<T>> jsonRoot = JsonConvert.DeserializeObject<JsonEntityRoot<List<T>>>(resAndCode.Json, internetTimeConverter, enumConverter);
                ret = jsonRoot.Data;
            }
            return ret;
        }

        public async Task<List<T>> ExecuteAsync(CancellationToken token)// throws StreamAPIErrorException, Exception
        {
            ResponseJsonStatusCode resAndCode = await MakeRequestAsync(token).ConfigureAwait(false);
            List<T> ret = null;
            if (Id != null || Ids.Length == 1)
            {
                JsonEntityRoot<T> jsonRoot = JsonConvert.DeserializeObject<JsonEntityRoot<T>>(resAndCode.Json, internetTimeConverter, enumConverter);
                T elem = jsonRoot.Data;
                ret = new List<T>
                {
                    elem
                };
            }
            else
            {
                JsonEntityRoot<List<T>> jsonRoot = JsonConvert.DeserializeObject<JsonEntityRoot<List<T>>>(resAndCode.Json, internetTimeConverter, enumConverter);
                ret = jsonRoot.Data;
            }

            return ret;
        }
    }
}