using System.Collections.Specialized;
using System.IO;
using System.Net;

namespace AtTask.OutlookAddIn.StreamApi.Connector.Impl
{
    internal class ResponseJsonStatusCode
    {
        public HttpStatusCode? StatusCode { get; set; }

        public string Json { get; set; }

        public NameValueCollection RequestHeaders { get; set; }

        public NameValueCollection ResponseHeaders { get; set; }

        //TODO: Set Cookie here
        //private Map<String, String> responseHeaders = null;

        public ResponseJsonStatusCode(HttpStatusCode? statusCode, string json, NameValueCollection requestHeaders, NameValueCollection responseHeaders)
        {
            this.StatusCode = statusCode;
            this.Json = json;
            this.RequestHeaders = requestHeaders;
            this.ResponseHeaders = responseHeaders;
        }
    }
}