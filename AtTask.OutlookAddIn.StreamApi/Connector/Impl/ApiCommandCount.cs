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
    internal class ApiCommandCount<T> : ApiCommand where T : StreamBase
    {
        private readonly string _namedQuery;

        public ApiCommandCount(IStreamApiConnector streamApiConnector, List<StringPair> criteria, StreamApiHeader streamApiHeader = null)
            : base(streamApiConnector, streamApiHeader)
        {
            this.ReturnType = typeof(ItemCount);

            if (criteria != null)
            {
                PostParams.AddRange(criteria);
            }
        }
        public ApiCommandCount(IStreamApiConnector streamApiConnector, List<StringPair> criteria, string namedQuery, StreamApiHeader streamApiHeader = null)
            : this(streamApiConnector, criteria, streamApiHeader)
        {
            _namedQuery = namedQuery;
        }

        protected override string CommandPath
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(((T)Activator.CreateInstance(typeof(T))).GetObjectType()).Append("/");
                sb.Append(ApiConstants.ApiActionCount);

                if (_namedQuery != null)
                {
                    sb.Append("/").Append(_namedQuery);
                }

                return sb.ToString();
            }
        }

        protected override string ApiMethod
        {
            get { return ApiConstants.ApiMethodGet; }
        }

        public int Execute()// throws StreamAPIErrorException, Exception
        {
            ResponseJsonStatusCode resAndCode = MakeRequest();
            JsonEntityRoot<ItemCount> jsonRoot = JsonConvert.DeserializeObject<JsonEntityRoot<ItemCount>>(resAndCode.Json, internetTimeConverter);
            ItemCount ret = jsonRoot.Data;
            return ret.Count;
        }
        public async Task<int> ExecuteAsync(CancellationToken token)// throws StreamAPIErrorException, Exception
        {
            ResponseJsonStatusCode resAndCode = await MakeRequestAsync(token).ConfigureAwait(false);
            JsonEntityRoot<ItemCount> jsonRoot = JsonConvert.DeserializeObject<JsonEntityRoot<ItemCount>>(resAndCode.Json, internetTimeConverter);
            ItemCount ret = jsonRoot.Data;
            return ret.Count;
        }
    }
}