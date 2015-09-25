using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using AtTask.OutlookAddIn.StreamApi.Connector.Service;
using AtTask.OutlookAddIn.Domain.Model;
using AtTask.OutlookAddin.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using AtTask.OutlookAddIn.Utilities;
using AtTask.OutlookAddIn.StreamApi.Helpers;

namespace AtTask.OutlookAddIn.StreamApi.Connector.Impl
{
    /// <summary>
    /// This class represent base class for all commands that implement concrete Stream API request method.
    /// This class does the actual server call.
    /// Any specifics are defined in virtual properties that the derived class must implement.
    /// - CommandPath: provides command specific path to be added to the API URI root.
    /// - ApiMethod: provide HTTP requests methods: GET, PUT, POST, DELETE, which will be added as method=xx request parameter
    ///
    /// All server calls are actually using POST HTTP method, and all parameters are sent in HTTP body.
    /// All parameters are URL encoded using UTF-8.
    /// Also, in constructor set ReturnType property as it will be used in Batch queries.
    /// </summary>
    internal abstract class ApiCommand
    {
        protected const string ContentType = "application/x-www-form-urlencoded;charset=utf-8";
        protected const string ContentType1 = "application/x-www-form-urlencoded";
        protected const string AcceptEncoding = "gzip";

        #region Json Converters

        /// <summary>
        /// Internet time converter
        /// </summary>
        protected IsoDateTimeConverter internetTimeConverter = new IsoDateTimeConverter()
        {
            DateTimeFormat = "yyyy-MM-dd\\THH:mm:ss:fffzzz",
            Culture = CultureInfo.InvariantCulture
        };

        protected IsoDateTimeConverter usTimeConverter = new IsoDateTimeConverter()
        {
            DateTimeFormat = "yyyy/MM/dd HH:mm:ss",
            Culture = CultureInfo.InvariantCulture
        };

        protected StreamApiEnumConverter enumConverter = new StreamApiEnumConverter();

        protected IContractResolver propertyNamesContractResolver = new StreamApiPropertyNamesContractResolver();

        #endregion Json Converters


        private readonly StreamApiHeader _streamApiHeader;
        private List<StringPair> postParams = new List<StringPair>();


        public StreamApiHeader StreamApiHeader
        {
            get
            {
                return _streamApiHeader;
            }
        }

        protected List<StringPair> PostParams
        {
            get
            {
                return this.postParams;
            }
            set
            {
                this.postParams = value;
            }
        }

        protected IStreamApiConnector StreamApiConnector { get; set; }

        public Type ReturnType { get; set; }

        protected ApiCommand(IStreamApiConnector streamApiConnector, StreamApiHeader streamApiHeader = null)
        {
            if (streamApiConnector == null)
            {
                throw new ArgumentException("Connector is null");
            }

            _streamApiHeader = streamApiHeader;

            StreamApiConnector = streamApiConnector;
        }

        public virtual string GetCommandUriString(bool urlEncoded = false)
        {
            List<StringPair> myParams = new List<StringPair>(postParams);
            FinalizePostParams(myParams);
            string queryString = MakeFormParamsAsPostData(myParams, urlEncoded);
            return string.Format("/{0}?{1}", CommandPath, queryString);
        }

        public virtual Uri CommandUri
        {
            get
            {
                Uri uri = StreamApiConnector.UriBase;
                Uri cmdUri;
                if (Uri.TryCreate(uri, CommandPath, out cmdUri))
                {
                    uri = cmdUri;
                }
                List<StringPair> myParams = new List<StringPair>(postParams);
                FinalizePostParams(myParams);
                string postParamsString = MakeFormParamsAsPostData(myParams, true);
                string uriStr = string.Format("{0}?{1}", cmdUri.ToString(), postParamsString);
                return new Uri(uriStr);
            }
        }

        /// <summary>
        /// Implement this method to provide command specific path to be added to the API URI root.
        /// </summary>
        protected abstract string CommandPath { get; }

        /// <summary>
        /// Implement this method to provide HTTP requests methods: GET, PUT, POST, DELETE, which will be added as method=xx request paramater
        /// </summary>
        protected abstract string ApiMethod { get; }

        protected string JsonSerializeWithoutNulls(object entity)
        {
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Converters = { internetTimeConverter, enumConverter },
                ContractResolver = propertyNamesContractResolver
            };
            string entityJson = JsonConvert.SerializeObject(entity, Formatting.None, serializerSettings);
            return entityJson;
        }

        /// <summary>
        /// Removes null and empty strings, also trims the strings
        /// </summary>
        /// <param name="strs"></param>
        /// <returns>Normalized string array</returns>
        protected string[] NormalizeStringArray(string[] strs)
        {
            if (strs == null)
            {
                return null;
            }
            // -- Remove trimmed empty, null elements
            List<string> arrStr = new List<string>();
            foreach (string s in strs)
            {
                if (s != null)
                {
                    string tmp = s.Trim();
                    if (!string.Empty.Equals(tmp))
                    {
                        arrStr.Add(tmp);
                    }
                }
            }
            return arrStr.ToArray();
        }

        protected string ComposeCommaSeparatedString(string[] strs)
        {
            if (strs == null || strs.Length == 0)
            {
                return null;
            }
            // -- Remove empty, null elements
            List<String> arrStr = new List<String>();
            foreach (string s in strs)
            {
                if (s != null)
                {
                    string tmp = s.Trim();
                    if (!"".Equals(tmp))
                    {
                        arrStr.Add(tmp);
                    }
                }
            }
            if (arrStr.Count == 0)
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(arrStr[0]);
            if (arrStr.Count > 1)
            {
                for (int i = 1, len = arrStr.Count; i < len; ++i)
                {
                    sb.Append(",").Append(arrStr[i]);
                }
            }
            return sb.ToString();
        }

        protected string MakeFormParamsAsPostData(List<StringPair> postParams, bool urlEncoded)
        {
            if (postParams != null)
            {
                StringBuilder sb = new StringBuilder();
                bool isFirst = true;
                foreach (StringPair pair in postParams)
                {
                    if (!isFirst)
                    {
                        sb.Append("&");
                    }
                    else
                    {
                        isFirst = false;
                    }

                    if (urlEncoded)
                    {
                        sb.Append(HttpUtility.UrlEncode(pair.Name, Encoding.UTF8)).Append("=").Append(HttpUtility.UrlEncode(pair.Value, Encoding.UTF8));
                    }
                    else
                    {
                        sb.Append(pair.Name).Append("=").Append(pair.Value);
                    }
                }
                return sb.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        protected void FinalizePostParams(List<StringPair> postParams)
        {
            if (postParams != null)
            {
                postParams.Add(new StringPair(ApiConstants.ApiParamMethod, ApiMethod));
                if (StreamApiConnector.SessionID != null)
                {
                    postParams.Add(new StringPair(ApiConstants.ApiParamSessionId, StreamApiConnector.SessionID));
                }
            }
        }

        /// <summary>
        /// Reads response stream and returns response string.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static string GetResponseString(HttpWebResponse response)
        {
            try
            {
                return WebUtil.GetResponseString(response);
            }
            catch (Exception ex)
            {
                throw new StreamApiException(ex);
            }
        }

        public async Task<ResponseJsonStatusCode> MakeRequestAsync(CancellationToken token)
        {
            var uri = GetRequestUri();

            using (var clientHandler = new HttpClientHandler())
            {
                InitializeHttpClienHandler(clientHandler, uri);
                var proxy = WebUtil.GetProxy(StreamApiConnector.UriBase);

                string proxyAddress = null;

                if (proxy != null)
                {
                    SetHttpClientHandlerProxy(clientHandler, proxy);
                    proxyAddress = proxy.ProxyAddress;
                }


                using (var client = new HttpClient(clientHandler))
                {
                    client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue(AcceptEncoding));
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType1));
                    //client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));

                    var userAgentString = GetUserAgentString();
                    client.DefaultRequestHeaders.UserAgent.TryParseAdd(userAgentString);

                    var acceptLanguage = GetAcceptLanguage();
                    if (acceptLanguage != null)
                    {
                        client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(acceptLanguage));
                    }

                    //-- Create POST data
                    FinalizePostParams(postParams);
                    string postData = MakeFormParamsAsPostData(postParams, true);

                    HttpContent content = new StringContent(postData, Encoding.UTF8, ContentType1);

                    try
                    {
                        using (var responseMessage = await client.PostAsync(uri, content, token).ConfigureAwait(false))
                        {
                            if (responseMessage.IsSuccessStatusCode)
                            {
                                var requestHeaders = new NameValueCollection();
                                foreach (var header in client.DefaultRequestHeaders.AsEnumerable())
                                {
                                    requestHeaders[header.Key] = string.Join(",", header.Value);
                                }

                                var responseHeaders = new NameValueCollection();
                                foreach (var header in responseMessage.Headers.AsEnumerable())
                                {
                                    responseHeaders[header.Key] = string.Join(",", header.Value);
                                }

                                string serverResponseString =
                                    await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

                                return new ResponseJsonStatusCode(responseMessage.StatusCode, serverResponseString,
                                    requestHeaders, responseHeaders);
                            }


                            if (responseMessage.StatusCode == HttpStatusCode.ProxyAuthenticationRequired)
                            {
                                var streamApiException = new StreamApiException(HttpStatusCode.ProxyAuthenticationRequired);
                                streamApiException.Data[StreamApiException.DataKeyProxyAddress] = proxyAddress;
                                throw streamApiException;
                            }

                            int status = (int) responseMessage.StatusCode;
                            if (status >= 300 && status < 400)
                            {
                                var streamApiEx = new StreamApiException(responseMessage.StatusCode);
                                if (responseMessage.StatusCode == HttpStatusCode.Redirect &&
                                    responseMessage.Headers.Contains(HttpResponseHeader.Location.ToString()))
                                {
                                    streamApiEx.Data[StreamApiException.DataKeyRedirectLocation] =
                                        responseMessage.Headers.Location;
                                }

                                throw streamApiEx;
                            }

                            if (status >= 400 && status <= 500)
                            {

                                JsonEntityRoot<StreamApiError> jsonRoot = null;
                                try
                                {
                                    Stream stream = await responseMessage.Content.ReadAsStreamAsync();
                                    jsonRoot = GetJsonFromStream<StreamApiError>(stream);
                                }
                                catch (Exception)
                                {
                                    //for non-JSON format responses
                                }

                                if (jsonRoot != null)
                                {
                                    StreamApiError error = jsonRoot.Error;
                                    error.Code = status;
                                    if (error.Message == null)
                                    {
                                        throw new StreamApiException(null, error);
                                    }

                                    throw new StreamApiException(null, error, error.Message);
                                }
                            }

                            //if we have no StreamApiError specify HttpStatusCode
                            throw new StreamApiException(responseMessage.StatusCode);
                        }
                    }
                    catch (HttpRequestException e)
                    {
                        var webException = e.InnerException as WebException;
                        if (webException == null)
                        {
                            throw new StreamApiException(e.InnerException);
                        }

                        var httpWebResponse = webException.Response as HttpWebResponse;
                        if (httpWebResponse != null
                            && httpWebResponse.StatusCode == HttpStatusCode.ProxyAuthenticationRequired)
                        {
                            var streamApiException = new StreamApiException(HttpStatusCode.ProxyAuthenticationRequired);
                            streamApiException.Data[StreamApiException.DataKeyProxyAddress] = proxyAddress;
                            throw streamApiException;
                        }

                        throw new StreamApiException(e.InnerException);
                    }
                }
            }
        }

        private static JsonEntityRoot<T> GetJsonFromStream<T>(Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(streamReader))
            {
                var jsonSerializer = new JsonSerializer();
                return jsonSerializer.Deserialize<JsonEntityRoot<T>>(jsonTextReader);
            }
        }

        private void InitializeHttpClienHandler(HttpClientHandler clientHandler, Uri uri)
        {
            clientHandler.AllowAutoRedirect = false;
            clientHandler.AutomaticDecompression = DecompressionMethods.GZip;

            var cookieContainer = new CookieContainer();
            string timezoneCookieString = WebUtil.GetTimezoneCookieString();
            var timezoneCookie = new Cookie(ApiConstants.ApiCookieTimezone, timezoneCookieString);
            cookieContainer.Add(uri, timezoneCookie);

            clientHandler.CookieContainer = cookieContainer;
        }

        private void SetHttpClientHandlerProxy(HttpClientHandler clientHandler, WebProxyInfo webProxyInfo)
        {
            if (webProxyInfo == null)
            {
                return;
            }

            clientHandler.Proxy = webProxyInfo.WebProxy;

            var networkCredential = WebHelper.GetProxyCredentials(
                webProxyInfo.ProxyAddress,
                StreamApiConnector.ProxyInfo);

            if (networkCredential != null)
            {
                clientHandler.Proxy.Credentials = networkCredential;
            }
        }

        /// <summary>
        /// Makes REST call (web request) to @task server
        /// </summary>
        /// <returns></returns>
        public ResponseJsonStatusCode MakeRequest()
        {
            Stream requestDataStream = null;
            HttpWebResponse response = null;

            try
            {
                var uri = GetRequestUri();

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                request.Method = WebRequestMethods.Http.Post;
                request.AllowAutoRedirect = false;

                //-- Set proxy settings
                string proxyHost = WebUtil.GetProxyHost(request);
                if (proxyHost != null && StreamApiConnector.ProxyInfo != null)
                {
                    var proxyCredentials = WebHelper.GetProxyCredentials(proxyHost, StreamApiConnector.ProxyInfo);
                    if (proxyCredentials != null)
                    {
                        WebRequest.DefaultWebProxy.Credentials = proxyCredentials;
                    }
                }

                //-- Set HTTP responseHeaders for Stream API calls
                request.ContentType = ContentType;
                request.Headers.Add(HttpRequestHeader.AcceptEncoding, AcceptEncoding);

                request.UserAgent = GetUserAgentString();

                var acceptLanguage = GetAcceptLanguage();
                if (acceptLanguage != null)
                {
                    request.Headers.Add(HttpRequestHeader.AcceptLanguage, acceptLanguage);
                }

                //-- Create POST data and convert it to a byte array.
                FinalizePostParams(postParams);
                string postData = MakeFormParamsAsPostData(postParams, true);
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = byteArray.Length;
                request.CookieContainer = new CookieContainer();

                string timezoneCookieString = WebUtil.GetTimezoneCookieString();
                Cookie timezoneCookie = new Cookie(ApiConstants.ApiCookieTimezone, timezoneCookieString);
                request.CookieContainer.Add(request.RequestUri, timezoneCookie);

                //-- Get the response.
                try
                {
                    requestDataStream = request.GetRequestStream();
                    requestDataStream.Write(byteArray, 0, byteArray.Length);
                    requestDataStream.Close();

                    response = (HttpWebResponse)request.GetResponse();
                }
                catch (WebException ex)
                {
                    if (ex.Response is HttpWebResponse &&
                        ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.ProxyAuthenticationRequired)
                    {
                        StreamApiException streamApiEx = new StreamApiException(ex);
                        streamApiEx.Data[StreamApiException.DataKeyProxyAddress] = proxyHost;
                        throw streamApiEx;
                    }

                    throw;
                }

                int status = (int)response.StatusCode;
                if (status >= 300 && status < 400)
                {
                    StreamApiException streamApiEx = new StreamApiException(response.StatusCode);
                    if (response.StatusCode == HttpStatusCode.Redirect &&
                        response.Headers.AllKeys.Contains<string>(HttpResponseHeader.Location.ToString()))
                    {
                        streamApiEx.Data[StreamApiException.DataKeyRedirectLocation] = response.Headers[HttpResponseHeader.Location];
                    }
                    throw streamApiEx;
                }

                NameValueCollection requestHeaders = new NameValueCollection(request.Headers);
                NameValueCollection responseHeaders = new NameValueCollection(response.Headers);
                string serverResponseString = GetResponseString(response);

                return new ResponseJsonStatusCode(response.StatusCode, serverResponseString, requestHeaders, responseHeaders);
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    int status = (int)((HttpWebResponse)ex.Response).StatusCode;

                    if (status >= 400 && status <= 500)
                    {
                        string responseFromServer = GetResponseString((HttpWebResponse)ex.Response);

                        JsonEntityRoot<StreamApiError> jsonRoot = null;
                        try
                        {
                            jsonRoot = JsonConvert.DeserializeObject<JsonEntityRoot<StreamApiError>>(responseFromServer);
                        }
                        catch (Exception)
                        {
                            //for non-JSON format responses
                        }

                        if (jsonRoot != null)
                        {
                            StreamApiError error = jsonRoot.Error;
                            error.Code = status;
                            if (error.Message == null)
                            {
                                throw new StreamApiException(ex, error);
                            }
                            else
                            {
                                throw new StreamApiException(ex, error, error.Message);
                            }
                        }
                    }

                    //if we have no StreamApiError specify HttpStatusCode
                    throw new StreamApiException(ex, ((HttpWebResponse)ex.Response).StatusCode);
                }

                throw new StreamApiException(ex);
            }
            catch (StreamApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new StreamApiException(ex);
            }
            finally
            {
                if (requestDataStream != null)
                {
                    try
                    {
                        requestDataStream.Close();
                    }
                    catch (Exception)
                    { }
                }
                if (response != null)
                {
                    try
                    {
                        response.Close();
                    }
                    catch (Exception)
                    { }
                }
            }
        }

        private string GetAcceptLanguage()
        {
            if (_streamApiHeader != null)
            {
                if (_streamApiHeader.Culture != null)
                {
                    return _streamApiHeader.Culture.Name;
                }
                if (StreamApiConnector.Culture != null)
                {
                    return  StreamApiConnector.Culture.Name;
                }
            }
            else if (StreamApiConnector.Culture != null)
            {
                return StreamApiConnector.Culture.Name;
            }

            return null;
        }

        private string GetUserAgentString()
        {
            if (_streamApiHeader != null)
            {
                return _streamApiHeader.UserAgent != null
                    ? _streamApiHeader.UserAgent.ToString()
                    : string.Empty;
            }

            return StreamApiConnector.UserAgent != null
                ? StreamApiConnector.UserAgent.ToString()
                : string.Empty;
        }

        private Uri GetRequestUri()
        {
            Uri uri = StreamApiConnector.UriBase;
            Uri cmdUri;
            //-- Form request path
            if (Uri.TryCreate(uri, CommandPath, out cmdUri))
            {
                uri = cmdUri;
            }
            return uri;
        }
    }
}