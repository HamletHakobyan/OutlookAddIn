using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AtTask.OutlookAddIn.StreamApi.Connector.Service;
using AtTask.OutlookAddIn.Domain.Model;
using Newtonsoft.Json;

namespace AtTask.OutlookAddIn.StreamApi.Connector.Impl
{
    internal class ApiCommandSearch<T> : ApiCommand where T : StreamBase
    {
        private string namedQuery;

        public string NamedQuery
        {
            get { return namedQuery; }
            set
            {
                if (value != null)
                {
                    value = value.Trim();
                    if (string.Empty.Equals(value))
                    {
                        throw new ArgumentException("Named Query must be null or non-empty string");
                    }
                }
                this.namedQuery = value;
            }
        }

        private List<StringPair> criteria;

        public List<StringPair> Criteria
        {
            get { return criteria; }
            set
            {
                this.criteria = value;
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

        public ApiCommandSearch(IStreamApiConnector streamApiConnector, List<StringPair> criteria, string[] fields, string namedQuery = null, StreamApiHeader streamApiHeader = null)
            : base(streamApiConnector, streamApiHeader)
        {
            this.Criteria = criteria;
            this.Fields = fields;
            this.NamedQuery = namedQuery;
            this.ReturnType = typeof(T);

            string fieldsParam = ComposeCommaSeparatedString(Fields);
            if (fieldsParam != null)
            {
                PostParams.Add(new StringPair(ApiConstants.ApiParamFields, fieldsParam));
            }

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
                string str = (NamedQuery != null) ? NamedQuery : ApiConstants.ApiActionSearch;
                sb.Append(str);
                return sb.ToString();
            }
        }

        protected override string ApiMethod
        {
            get { return ApiConstants.ApiMethodGet; }
        }

        public List<T> Execute()// throws StreamAPIErrorException, Exception
        {
            ResponseJsonStatusCode resAndCode = MakeRequest();
            JsonEntityRoot<List<T>> jsonRoot = JsonConvert.DeserializeObject<JsonEntityRoot<List<T>>>(resAndCode.Json, internetTimeConverter, enumConverter);
            List<T> ret = jsonRoot.Data;
            return ret;
        }

        public async Task<List<T>> ExecuteAsync(CancellationToken token)// throws StreamAPIErrorException, Exception
        {
            ResponseJsonStatusCode resAndCode = await MakeRequestAsync(token).ConfigureAwait(false);
            JsonEntityRoot<List<T>> jsonRoot = JsonConvert.DeserializeObject<JsonEntityRoot<List<T>>>(resAndCode.Json, internetTimeConverter, enumConverter);
            List<T> ret = jsonRoot.Data;
            return ret;
        }
    }
}