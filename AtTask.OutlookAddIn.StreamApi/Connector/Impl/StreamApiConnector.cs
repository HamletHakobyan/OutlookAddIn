using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using AtTask.OutlookAddIn.StreamApi.Connector.Service;
using AtTask.OutlookAddIn.Domain.Model;
using AtTask.OutlookAddIn.Domain;

namespace AtTask.OutlookAddIn.StreamApi.Connector.Impl
{
    /// <summary>
    /// Implements IStreamApiConnector interface
    /// </summary>
    public sealed class StreamApiConnector : IStreamApiConnector
    {
        #region Member fields

        private bool inited;
        private LoginInfo loginInfo;

        private Uri uriBase;
        private StreamApiEndpoint endpoint;

        private string host;
        private string versionNumber;

        #endregion Member fields

        public StreamApiConnector()
        {
            this.endpoint = StreamApiEndpoint.StandardApi;
        }

        #region Properties

        public string SessionID
        {
            get
            {
                if (this.loginInfo != null && this.loginInfo.Session != null)
                {
                    return this.loginInfo.Session.SessionID;
                }

                return null;
            }
        }

        public Uri UriBase
        {
            get { return this.uriBase; }
        }

        public StreamApiEndpoint Endpoint
        {
            get { return this.endpoint; }
        }

        public CultureInfo Culture { get; set; }

        public UserAgent UserAgent { get; set; }

        public LoginInfo LoginInfo
        {
            get
            {
                if (loginInfo != null)
                {
                    return (LoginInfo)loginInfo.Clone();
                }
                return null;
            }
            set
            {
                loginInfo = value;
            }
        }

        public ProxyInfo ProxyInfo { get; set; }

        #endregion Properties

        public void Init(string host, string versionNumber, StreamApiEndpoint endpoint = StreamApiEndpoint.StandardApi)
        {
            this.loginInfo = null;
            this.inited = false;
            this.host = host;
            this.versionNumber = versionNumber;
            this.endpoint = endpoint;

            Uri hostUri = new Uri(host);

            string endpointPath = "";
            switch (this.endpoint)
            {
                case StreamApiEndpoint.StandardApi:
                    endpointPath = ApiConstants.ApiEndpointPathStandard;
                    break;

                case StreamApiEndpoint.InternalApi:
                    endpointPath = ApiConstants.ApiEndpointPathInternal;
                    break;

                case StreamApiEndpoint.AspApi:
                    endpointPath = ApiConstants.ApiEndpointPathAsp;
                    break;
                default:
                    endpointPath = ApiConstants.ApiEndpointPathStandard;
                    break;
            }
            Uri uri;
            Uri.TryCreate(hostUri, endpointPath, out uri);

            if (this.versionNumber != null)
            {
                this.versionNumber = this.versionNumber.Trim();
                if (!"".Equals(this.versionNumber))
                {
                    Uri tmpUri;
                    Uri.TryCreate(uri, "v" + this.versionNumber + "/", out tmpUri);
                    uri = tmpUri;
                }
            }
            this.uriBase = new Uri(uri.ToString());
            this.inited = true;
        }

        private void CheckInited()
        {
            if (!inited)
            {
                throw new ApplicationException("Stream API Connector is not initialized");
            }
        }

        public IBatchQuery GetBatchQuery(StreamApiHeader streamApiHeader = null)
        {
            CheckInited();
            return new ApiCommandBatch(this, streamApiHeader);
        }

        public LoginInfo Login(string username, string password, int? apiSessionTimeout = null, StreamApiHeader streamApiHeader = null)
        {
            CheckInited();
            ApiCommandLogin cmd = new ApiCommandLogin(this, username, password, apiSessionTimeout, streamApiHeader);
            loginInfo = cmd.Execute();

            //-- return the clone of loginInfo
            return LoginInfo;
        }

        public LoginInfo Login(string sessionID, StreamApiHeader streamApiHeader = null)
        {
            CheckInited();
            ApiCommandLogin cmd = new ApiCommandLogin(this, sessionID, streamApiHeader);
            loginInfo = cmd.Execute();

            //-- return the clone of loginInfo
            return LoginInfo;
        }

        public async Task<LoginInfo> LoginAsync
            (string username, string password, int? apiSessionTimeout = null, StreamApiHeader streamApiHeader = null, CancellationToken token = new CancellationToken())
        {
            CheckInited();
            ApiCommandLogin cmd = new ApiCommandLogin(this, username, password, apiSessionTimeout, streamApiHeader);
            LoginInfo = await cmd.ExecuteAsync(token);

            return LoginInfo;
        }

        public async Task<LoginInfo> LoginAsync(string sessionID, StreamApiHeader streamApiHeader = null, CancellationToken token = new CancellationToken())
        {
            CheckInited();
            ApiCommandLogin cmd = new ApiCommandLogin(this, sessionID, streamApiHeader);
            LoginInfo = await cmd.ExecuteAsync(token);

            return LoginInfo;
        }

        public LoginInfo LoginByOtherSessionID(string username, string otherSessionID, StreamApiHeader streamApiHeader = null)
        {
            CheckInited();
            ApiCommandLoginViaSessionId cmd = new ApiCommandLoginViaSessionId(this, username, otherSessionID, streamApiHeader);
            loginInfo = cmd.Execute();

            //-- return the clone of loginInfo
            return LoginInfo;
        }

        public void Logout(StreamApiHeader streamApiHeader = null)
        {
            CheckInited();
            new ApiCommandLogout(this, streamApiHeader).Execute();
            loginInfo = null;
        }

        public T Get<T>(string id, string[] fields, StreamApiHeader streamApiHeader = null) where T : EntityBase
        {
            CheckInited();
            ApiCommandGet<T> cmd = new ApiCommandGet<T>(this, id, fields, streamApiHeader);
            List<T> retArray = cmd.Execute();
            T ret = (retArray == null || retArray.Count == 0) ? null : retArray[0];
            return ret;
        }

        public List<T> Get<T>(string[] ids, string[] fields, StreamApiHeader streamApiHeader = null) where T : EntityBase
        {
            CheckInited();
            ApiCommandGet<T> cmd = new ApiCommandGet<T>(this, ids, fields, streamApiHeader);
            return cmd.Execute();
        }

        /// <summary>
        /// Returns the <see cref="T:System.Threading.Tasks.Task{T}"/> that represents the Stream API object specified by id and the type. Returns the fields of the object specified by fields argument.
        /// </summary>
        /// <typeparam name="T">Class of required object</typeparam>
        /// <param name="id">The id of required object; cannot be null</param>
        /// <param name="fields">Array of fields to be returned in the object requested; can be null: in that case the default fields will be returned.</param>
        /// <param name="token">Cancellation token to observe cancellation request from client code.</param>
        /// <param name="streamApiHeader"></param>
        /// <returns>The Stream API object with the specified id and T type</returns>
        /// <exception cref="StreamApiException">To obtain API specific error user Error property. WebException member gives the full web exception info</exception>
        /// <exception cref="ApplicationException">If Init() method was not called properly, i.e. it has not thrown an exception</exception>
        public async Task<T> GetAsync<T>(string id, string[] fields, CancellationToken token, StreamApiHeader streamApiHeader = null) where T : EntityBase
        {
            CheckInited();
            ApiCommandGet<T> cmd = new ApiCommandGet<T>(this, id, fields, streamApiHeader);
            List<T> retArray = await cmd.ExecuteAsync(token).ConfigureAwait(false);
            T ret = (retArray == null || retArray.Count == 0) ? null : retArray[0];
            return ret;
        }

        /// <summary>
        /// Returns the <see cref="T:System.Threading.Tasks.Task"/> that represents the Stream API objects specified by ids and the type. Returns the fields of the object specified by fields argument.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids">The array of ids of objects; cannot be null or empty array, or array of empty ids.</param>
        /// <param name="fields">Array of fields to be returned in the objects requested; can be null</param>
        /// <param name="token">Cancellation token to observe cancellation request from client code.</param>
        /// <param name="streamApiHeader"></param>
        /// <returns>The Stream API object list with the specified id and T type</returns>
        /// <exception cref="StreamApiException">To obtain API specific error user Error property. WebException member gives the full web exception info</exception>
        /// <exception cref="ApplicationException">If Init() method was not called properly, i.e. it has not thrown an exception</exception>
        public async Task<List<T>> GetAsync<T>(string[] ids, string[] fields, CancellationToken token, StreamApiHeader streamApiHeader = null) where T : EntityBase
        {
            CheckInited();
            ApiCommandGet<T> cmd = new ApiCommandGet<T>(this, ids, fields, streamApiHeader);
            return await cmd.ExecuteAsync(token).ConfigureAwait(false);
        }

        public List<T> Search<T>(List<StringPair> criteria, string[] fields, string namedQuery = null, StreamApiHeader streamApiHeader = null) where T : StreamBase
        {
            CheckInited();
            ApiCommandSearch<T> cmd = new ApiCommandSearch<T>(this, criteria, fields, namedQuery, streamApiHeader);
            return cmd.Execute();
        }

        /// <summary>
        /// Returns the <see cref="T:System.Threading.Tasks.Task"/> that represents the list of objects of type T which meet the criteria and/or namedQuery.
        /// </summary>
        /// <typeparam name="T">The type of object to be returned</typeparam>
        /// <param name="criteria">The list of name/value pairs as criteria for the server side search; can be null</param>
        /// <param name="fields">Array of fields to be returned in the object requested; can be null: in that case the default fields will be returned.</param>
        /// <param name="token">Cancellation token to observe cancellation request from client code.</param>
        /// <param name="namedQuery">Option parameter to involve named query of Stream API</param>
        /// <param name="streamApiHeader"></param>
        /// <returns>The list of objects which meet to the object type, criteria and/or namedQuery</returns>
        /// <exception cref="StreamApiException">To obtain API specific error user Error property. WebException member gives the full web exception info</exception>
        /// <exception cref="ApplicationException">If Init() method was not called properly, i.e. it has not thrown an exception</exception>
        public async Task<List<T>> SearchAsync<T>(List<StringPair> criteria, string[] fields, CancellationToken token, string namedQuery = null, StreamApiHeader streamApiHeader = null) where T : StreamBase
        {
            CheckInited();
            ApiCommandSearch<T> cmd = new ApiCommandSearch<T>(this, criteria, fields, namedQuery, streamApiHeader);
            return await cmd.ExecuteAsync(token).ConfigureAwait(false);
        }

        public async Task<List<T>> SolrSearchAsync<T>(SolrSearchBuilder<T> builder, CancellationToken token, StreamApiHeader streamApiHeader = null) where T : EntityBase, new()
        {
            CheckInited();
            var criteria = builder.GetCriteria();
            var commandSolrSearch = new ApiCommandSolrSearch<T>(this, criteria, streamApiHeader);
            return await commandSolrSearch.ExecuteAsync(token).ConfigureAwait(false);
        }

        public int Count<T>(List<StringPair> criteria, StreamApiHeader streamApiHeader = null) where T : StreamBase
        {
            CheckInited();
            ApiCommandCount<T> cmd = new ApiCommandCount<T>(this, criteria, streamApiHeader);
            return cmd.Execute();
        }

        public async Task<int> CountAsync<T>(List<StringPair> criteria, CancellationToken token, StreamApiHeader streamApiHeader = null) where T : StreamBase
        {
            CheckInited();
            ApiCommandCount<T> cmd = new ApiCommandCount<T>(this, criteria, streamApiHeader);
            return await cmd.ExecuteAsync(token).ConfigureAwait(false);
        }

        public async Task<int> CountAsync<T>(List<StringPair> criteria, string namedQuery, CancellationToken token, StreamApiHeader streamApiHeader = null) where T : StreamBase
        {
            CheckInited();
            ApiCommandCount<T> cmd = new ApiCommandCount<T>(this, criteria, namedQuery, streamApiHeader);
            return await cmd.ExecuteAsync(token).ConfigureAwait(false);
        }

        public T Create<T>(T entity, string[] fields, StreamApiHeader streamApiHeader = null) where T : StreamBase
        {
            CheckInited();
            ApiCommandCreate<T> cmd = new ApiCommandCreate<T>(this, entity, fields, streamApiHeader);
            return cmd.Execute();
        }

        /// <summary>
        /// Returns the <see cref="T:System.Threading.Tasks.Task{T}"/> which represents the StreamAPI Create operation.
        /// </summary>
        /// <typeparam name="T">The type of object to be created</typeparam>
        /// <param name="entity">The object which contains the fields that will be set on object creation in server side</param>
        /// <param name="fields">Array of fields to be returned in the object requested; can be null: in that case the default fields will be returned.</param>
        /// <param name="token">Cancellation token to observe cancellation request from client code.</param>
        /// <param name="streamApiHeader"></param>
        /// <returns>The created object with the fields specified in appropriate argument</returns>
        /// <exception cref="StreamApiException">To obtain API specific error user Error property. WebException member gives the full web exception info</exception>
        /// <exception cref="ApplicationException">If Init() method was not called properly, i.e. it has not thrown an exception</exception>
        public async Task<T> CreateAsync<T>(T entity, string[] fields, CancellationToken token, StreamApiHeader streamApiHeader = null) where T : StreamBase
        {
            CheckInited();
            ApiCommandCreate<T> cmd = new ApiCommandCreate<T>(this, entity, fields, streamApiHeader);
            return await cmd.ExecuteAsync(token).ConfigureAwait(false);
        }

        public T Update<T>(T entity, string[] fields, StreamApiHeader streamApiHeader = null) where T : EntityBase
        {
            CheckInited();
            ApiCommandUpdate<T> cmd = new ApiCommandUpdate<T>(this, entity, fields, streamApiHeader);
            return cmd.Execute();
        }

        public async Task<T> UpdateAsync<T>(T entity, string[] fields, CancellationToken token, StreamApiHeader streamApiHeader = null) where T : EntityBase
        {
            CheckInited();
            var cmd = new ApiCommandUpdate<T>(this, entity, fields, streamApiHeader);
            return await cmd.ExecuteAsync(token).ConfigureAwait(false);
        }

        public U Action<T, U>(string id, string action, List<StringPair> criteria = null, StreamApiHeader streamApiHeader = null) where T : EntityBase
        {
            CheckInited();
            ApiCommandAction<T, U> cmd = new ApiCommandAction<T, U>(this, id, action, criteria, streamApiHeader);
            return cmd.Execute();
        }

        public void Delete<T>(string id, bool force = false, StreamApiHeader streamApiHeader = null) where T : EntityBase
        {
            CheckInited();
            new ApiCommandDelete<T>(this, id, force, streamApiHeader).Execute();
        }

        public U Report<T, U>(List<StringPair> criteria, StreamApiHeader streamApiHeader = null) where T : StreamBase
        {
            CheckInited();
            ApiCommandReport<T, U> cmd = new ApiCommandReport<T, U>(this, criteria, streamApiHeader);
            return cmd.Execute();
        }

        public Dictionary<string, Dictionary<string, ApiObjectInfo>> ApiMetadata(StreamApiHeader streamApiHeader = null)
        {
            CheckInited();
            ApiCommandApiMetadata cmd = new ApiCommandApiMetadata(this, streamApiHeader);
            return cmd.Execute();
        }

        public DomainObjectInfo Metadata<T>(StreamApiHeader streamApiHeader = null) where T : StreamBase
        {
            CheckInited();
            ApiCommandMetadata<T> cmd = new ApiCommandMetadata<T>(this, streamApiHeader);
            return cmd.Execute();
        }

        public ServerInfo ApiServerInfo(StreamApiHeader streamApiHeader = null)
        {
            CheckInited();
            ApiCommandApiServerInfo cmd = new ApiCommandApiServerInfo(this, streamApiHeader);
            return cmd.Execute();
       }
    }
}