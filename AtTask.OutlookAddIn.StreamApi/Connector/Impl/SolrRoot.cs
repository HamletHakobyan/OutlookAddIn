using System.Collections.Generic;
using AtTask.OutlookAddIn.Domain.Model;
using Newtonsoft.Json;

namespace AtTask.OutlookAddIn.StreamApi.Connector.Impl
{
    public class SolrRoot<T>
    {
        [JsonProperty("data")]
        public Data<T> Data { get; set; }

        [JsonProperty("error")]
        public StreamApiError Error { get; set; }
    }

    public class Data<T>
    {

        [JsonProperty("items")]
        public List<T> Items { get; set; }
    }
}