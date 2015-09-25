using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AtTask.OutlookAddIn.Domain.Model;
using AtTask.OutlookAddIn.StreamApi.Connector.Service;
using Newtonsoft.Json;
using Task = AtTask.OutlookAddIn.Domain.Model.Task;

namespace AtTask.OutlookAddIn.StreamApi.Connector.Impl
{
    class ApiCommandSolrSearch<T> : ApiCommand where T : EntityBase
    {
        public ApiCommandSolrSearch(IStreamApiConnector streamApiConnector, List<StringPair> criteria, StreamApiHeader streamApiHeader = null)
            : base(streamApiConnector, streamApiHeader)
        {
            if (criteria != null)
            {
                PostParams.AddRange(criteria);
            }
        }

        protected override string CommandPath
        {
            get { return "search"; }
        }

        protected override string ApiMethod
        {
            get { return ApiConstants.ApiMethodGet; }
        }

        public async Task<List<T>> ExecuteAsync(CancellationToken token)// throws StreamAPIErrorException, Exception
        {
            ResponseJsonStatusCode resAndCode = await MakeRequestAsync(token).ConfigureAwait(false);
            SolrRoot<T> solrRoot = JsonConvert.DeserializeObject<SolrRoot<T>>(resAndCode.Json, internetTimeConverter, enumConverter);
            return solrRoot.Data.Items;
        }
    }
}