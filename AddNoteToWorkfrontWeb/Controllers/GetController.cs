using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using AddNoteToWorkfrontWeb.Extensions;
using AtTask.OutlookAddIn.Assets;
using AtTask.OutlookAddIn.Domain.Model;
using AtTask.OutlookAddIn.StreamApi;
using AtTask.OutlookAddIn.StreamApi.Connector.Service;
using Newtonsoft.Json;

namespace AddNoteToWorkfrontWeb.Controllers
{
    public class GetController : ApiController
    {
        private IStreamApiConnector GetApiConnector(string host)
        {
            var connector = StreamApiConnectorFactory.NewInstance();
            connector.Init(host, Settings.Default.StreamAPIVersion, StreamApiEndpoint.InternalApi);
            connector.UserAgent = new UserAgent();
            return connector;
        }
        // GET: api/Get
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Get/5
        public async Task<IEnumerable<Work>> MyWork()
        {
            try
            {
                var sessionId = Request.GetCookie("workfront-session");
                var host = Request.GetCookie("workfront-host");
                var connector = GetApiConnector(host);
                await connector.LoginAsync(sessionId);

                var criteria = new List<StringPair>
                {
                    new StringPair(CriteriaLimit, "10")
                };

                return await connector.SearchAsync<Work>(criteria, ApiFieldsWorkMyWorkAndRequests, CancellationToken.None, "myWork");
            }
            catch (Exception e)
            {
                var resEx = e;
                var status = HttpStatusCode.BadRequest;
                var ex = e as StreamApiException;
                if (ex != null)
                {
                    status = HttpStatusCode.BadRequest;
                    resEx = ex;
                    var wex = ex.WebException;
                    if (wex != null)
                    {
                        if (wex.Status == WebExceptionStatus.ProtocolError)
                        {
                            var response = wex.Response as HttpWebResponse;
                            if (response != null)
                            {
                                status = response.StatusCode;
                                resEx = wex;
                            }
                        }
                    }
                }

                throw new HttpResponseException(Request.CreateErrorResponse(status, resEx));
            }
        }

        private static readonly string[] ApiFieldsWorkMyWorkAndRequests =
        {
            "ID", "name", "description", "plannedStartDate", "dueDate", "actualCompletionDate","commitDate", "plannedCompletionDate"
        };

        private const string CriteriaLimit = "$$LIMIT";
    }
}
