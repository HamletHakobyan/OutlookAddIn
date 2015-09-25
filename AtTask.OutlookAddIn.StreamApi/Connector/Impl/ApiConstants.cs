namespace AtTask.OutlookAddIn.StreamApi.Connector.Impl
{
    internal class ApiConstants
    {
        public const string ApiEndpointPathStandard = "/attask/api/";
        public const string ApiEndpointPathInternal = "/attask/api-internal/";
        public const string ApiEndpointPathAsp = "/attask/api-asp/";

        public const string ApiParamUsername = "username";
        public const string ApiParamPassword = "password";
        public const string ApiParamTimeToLive = "timeToLive";
        public const string ApiParamSessionId = "sessionID";
        public const string ApiParamId = "id";
        public const string ApiParamFields = "fields";
        public const string ApiParamMethod = "method";
        public const string ApiParamUpdates = "updates";
        public const string ApiParamForce = "force";
        public const string ApiParamUri = "uri";
        public const string ApiParamAtomic = "atomic";

        public const string ApiMethodGet = "get";
        public const string ApiMethodPost = "post";
        public const string ApiMethodPut = "put";
        public const string ApiMethodDelete = "delete";

        public const string ApiPathLogin = "login";
        public const string ApiPathLoginAsp = "login";
        public const string ApiPathLoginSession = "session";
        public const string ApiPathLogout = "logout";
        public const string ApiPathBatch = "batch";
        public const string ApiPathMetadata = "metadata";

        public const string ApiPathInfo = "info";

        public const string ApiActionSearch = "search";
        public const string ApiActionCount = "count";
        public const string ApiActionReport = "report";
        public const string ApiActionAction = "action";
        public const string ApiActionMetadata = "metadata";

        public const string ApiCookieTimezone = "timezone";
    }
}