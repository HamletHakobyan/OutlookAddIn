using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AtTask.OutlookAddIn.Assets;
using AtTask.OutlookAddIn.Domain;
using AtTask.OutlookAddIn.Domain.Model;
using AtTask.OutlookAddIn.StreamApi;
using AtTask.OutlookAddIn.StreamApi.Connector.Service;
using AtTask.OutlookAddIn.Utilities;
using Newtonsoft.Json;

namespace AddNoteToWorkfrontWeb.Controllers
{
    public class AuthenticationController : ApiController
    {
        private IStreamApiConnector GetApiConnector(ConnectionInfo info)
        {
            var host = WebUtil.GetValidAbsoluteUriString(info.Host);
            var connector = StreamApiConnectorFactory.NewInstance();
            connector.Init(host, Settings.Default.StreamAPIVersion, StreamApiEndpoint.InternalApi);
            connector.UserAgent = new UserAgent();
            return connector;
        }

        private string GetCookie(string cookieString)
        {
            var indexOf = cookieString.IndexOf("%", StringComparison.InvariantCulture);

            var cookie = cookieString.Substring(0, indexOf);
            return cookie.Split('=')[1];
        }

        // POST: api/Login
        public async Task<HttpResponseMessage> Login([FromBody]ConnectionInfo info)
        {
            var connector = GetApiConnector(info);
            try
            {
                var loginInfo = await connector.LoginAsync(info.Username, info.Password);
                var message = new HttpResponseMessage(HttpStatusCode.OK);
                var sessionId = GetCookie(loginInfo.Cookie);
                var cookieSession = new CookieHeaderValue("workfront-session", sessionId);
                cookieSession.Expires = DateTimeOffset.Now.AddDays(1);
                //cookie.Domain = Request.RequestUri.Host;
                cookieSession.Path = "/";
                var cookieHost = new CookieHeaderValue("workfront-host", info.Host);
                cookieHost.Expires = DateTimeOffset.Now.AddDays(1);
                //cookieHost.Domain = Request.RequestUri.Host;
                cookieHost.Path = "/";
                message.Headers.AddCookies(new[] { cookieSession, cookieHost });
                return message;
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

                return Request.CreateErrorResponse(status, resEx);
            }
        }
    }
}
