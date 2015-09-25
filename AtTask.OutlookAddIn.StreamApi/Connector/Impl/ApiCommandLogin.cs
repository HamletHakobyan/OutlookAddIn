using System;
using System.Threading;
using System.Threading.Tasks;
using AtTask.OutlookAddIn.StreamApi.Connector.Service;
using AtTask.OutlookAddIn.Domain.Model;
using Newtonsoft.Json;

namespace AtTask.OutlookAddIn.StreamApi.Connector.Impl
{
    internal class ApiCommandLogin : ApiCommand
    {
        private const string HeaderSetCookie = "Set-Cookie";

        private string username;

        public string Username
        {
            get { return username; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Null Username");
                }
                this.username = value;
            }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Null Password");
                }
                this.password = value;
            }
        }

        public int? ApiSessionTimeout { get; set; }

        private string sessionID;

        protected ApiCommandLogin(IStreamApiConnector streamApiConnector, StreamApiHeader streamApiHeader = null)
            : base(streamApiConnector, streamApiHeader)
        { 
        }

        public ApiCommandLogin(IStreamApiConnector streamApiConnector, string username, string password, int? apiSessionTimeout, StreamApiHeader streamApiHeader = null)
            : this(streamApiConnector, streamApiHeader)
        {
            this.Username = username;
            this.Password = password;
            this.ApiSessionTimeout = apiSessionTimeout;
            this.ReturnType = typeof(LoginInfo);

            PostParams.Add(new StringPair(ApiConstants.ApiParamUsername, Username));
            PostParams.Add(new StringPair(ApiConstants.ApiParamPassword, Password));
            if (ApiSessionTimeout.HasValue)
            {
                PostParams.Add(new StringPair(ApiConstants.ApiParamTimeToLive, ApiSessionTimeout.Value.ToString()));
            }
        }

        public ApiCommandLogin(IStreamApiConnector streamApiConnector, string sessionID, StreamApiHeader streamApiHeader = null)
            : this(streamApiConnector, streamApiHeader)
        {
            this.sessionID = sessionID;
            this.ReturnType = typeof(LoginInfo);

            PostParams.Add(new StringPair(ApiConstants.ApiParamId, sessionID));
            PostParams.Add(new StringPair(ApiConstants.ApiParamSessionId, sessionID));
        }
        
        protected override string CommandPath
        {
            get
            {
                string cmdPath = ApiConstants.ApiPathLoginSession;
                if (this.sessionID == null)
                {
                    cmdPath = ApiConstants.ApiPathLogin;
                    if (StreamApiConnector.Endpoint == StreamApiEndpoint.AspApi)
                    {
                        cmdPath = ApiConstants.ApiPathLoginAsp;
                    }
                }
                return cmdPath;
            }
        }

        protected override string ApiMethod
        {
            get { return ApiConstants.ApiMethodGet; }
        }

        public LoginInfo Execute()// throws StreamAPIErrorException, Exception
        {
            ResponseJsonStatusCode resAndCode = MakeRequest();
            JsonEntityRoot<Session> jsonRoot = null;
            try
            {
                jsonRoot = JsonConvert.DeserializeObject<JsonEntityRoot<Session>>(resAndCode.Json, internetTimeConverter);
            }
            catch (Exception)
            {
                jsonRoot = JsonConvert.DeserializeObject<JsonEntityRoot<Session>>(resAndCode.Json, usTimeConverter);
            }

            Session sessions = jsonRoot.Data;
            LoginInfo loginInfo = new LoginInfo();
            loginInfo.Session = sessions;
            loginInfo.Cookie = resAndCode.ResponseHeaders[HeaderSetCookie];
            return loginInfo;
        }
        public async Task<LoginInfo> ExecuteAsync(CancellationToken token)// throws StreamAPIErrorException, Exception
        {
            ResponseJsonStatusCode resAndCode = await MakeRequestAsync(token);
            JsonEntityRoot<Session> jsonRoot = null;
            try
            {
                jsonRoot = JsonConvert.DeserializeObject<JsonEntityRoot<Session>>(resAndCode.Json, internetTimeConverter);
            }
            catch (Exception)
            {
                jsonRoot = JsonConvert.DeserializeObject<JsonEntityRoot<Session>>(resAndCode.Json, usTimeConverter);
            }

            Session sessions = jsonRoot.Data;
            LoginInfo loginInfo = new LoginInfo();
            loginInfo.Session = sessions;
            loginInfo.Cookie = resAndCode.ResponseHeaders[HeaderSetCookie];
            return loginInfo;
        }
    }
}