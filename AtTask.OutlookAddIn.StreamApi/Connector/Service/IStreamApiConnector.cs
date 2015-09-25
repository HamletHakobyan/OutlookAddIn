using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using AtTask.OutlookAddIn.Domain;
using AtTask.OutlookAddIn.Domain.Model;

namespace AtTask.OutlookAddIn.StreamApi.Connector.Service
{
    /// <summary>
    /// Stream API Connector interface throw which to make REST calls to @task server.
    /// You can find the Atream API documentation here: https://wiki.attask.com/@task/Stream_API
    /// This interface provides a C# interface to communicate to @task Stream API server and return in form of Domain objects,
    /// which represent Stream API objects of @task system.
    /// If you try to call any API related method without prior calling Init() method, then those methods will throw ApplicationException.
    /// </summary>
    /// <example>
    /// A typical usage example:
    /// <code><![CDATA[
    /// //-- Init the Stream API Connector
    /// string host = "http://attask-sandbox1";
    /// string version = "1.0";
    /// IStreamApiConnector connector = StreamApiConnectorFactory.NewInstance();
    /// connector.Init(host, version, null);
    ///
    /// //-- Login to system
    /// string username = "someuser";
    /// string password = "user";
    /// LoginInfo loginInfo = connector.Login(username, password);
    ///
    /// //-- Get current user
    /// User currentUser = connector.Get<User>(loginInfo.SessionAttributes.UserID, new String[] { "firstName", "lastName" });
    /// loginInfo.User = currentUser;
    ///
    /// //-- Get "Working On" list
    /// string[] fields = new string[] { "ID", "status", "workItem:priority" };
    ///   List<Work> myWork = connector.Search<Work>(null, fields, "myWork");
    ///
    /// ]]>
    /// </code>
    /// </example>
    public interface IStreamApiConnector
    {
        /// <summary>
        /// Initializes API Connector with host, version number and httpHeaders that will be sent in each server call.
        ///
        /// </summary>
        /// <param name="host">The host of the @task server</param>
        /// <param name="versionNumber">A string in form: "1.0", "2.1". If null, this means don't use a special version of API, but the latest one.</param>
        /// <param name="endpoint">Represents Stream API endpoint: Standard, Internal or ASP. Defaults to Standard endpoint.</param>
        /// <exception cref="ArgumentNullException">If the host is null</exception>
        /// <exception cref="UriFormatException">If the host does not have URI format</exception>
        void Init(string host, string versionNumber, StreamApiEndpoint endpoint = StreamApiEndpoint.StandardApi);

        /// <summary>
        /// This is the Uri instance representing tha @task server API root Uri.
        /// E.g. if in Init() method you specified "http://www.host.com" as host value and "1.0" as a API version then this property will return
        /// "http://www.host.com/attask/api/v1.0/" string
        /// </summary>
        Uri UriBase { get; }

        /// <summary>
        /// Returns LoginInfo instance which represents the logged in user ID and the session ID in SessionAttributes domain object
        /// </summary>
        LoginInfo LoginInfo { get; set; }

        string SessionID { get; }

        /// <summary>
        /// The culture info to specify to server what language the client wants to get response in.
        /// If this is null, then no language is sent to server.
        /// </summary>
        CultureInfo Culture { get; set; }

        /// <summary>
        /// The User-agent header sent to @task server.
        /// This is used as a log on server requests for statistics.
        /// Typically, the client sends which type client is calling and server machine, OS and system info.
        /// E.g. Outlook Add-in can send:
        /// AtTask-Outlook (x86; Dell PC; Windows 7)
        ///
        /// If this property is null or after truncation is empty, then user-agent header is not sent.
        /// </summary>
        UserAgent UserAgent { get; set; }

        /// <summary>
        /// Proxy host with username and passwords.
        /// </summary>
        ProxyInfo ProxyInfo { get; set; }

        /// <summary>
        /// Endpoint of Stream API Server
        /// </summary>
        StreamApiEndpoint Endpoint { get; }

        /// <summary>
        /// Returns Batch query object through which the client can make Stream API Batch queries.
        /// </summary>
        /// <exception cref="ApplicationException">If Init() method was not called properly, i.e. it has not thrown an exception</exception>
        IBatchQuery GetBatchQuery(StreamApiHeader streamApiHeader = null);

        /// <summary>
        /// Logs in and returns LoginInfo containing Session info (SessionID), user info (user ID) and the cookie set via server
        /// </summary>
        /// <param name="username">The username; cannot be null</param>
        /// <param name="password">The password; cannot be null</param>
        /// <param name="apiSessionTimeout">
        ///     Specified the timeout in seconds for Stream API session to stay alive. Can be null; in that case the timeout is defined by server.
        ///     Note: This works only if server is configured to use EhCache. When using HazelCast it ignores the attribute and by default uses 8 hours.
        /// </param>
        /// <returns>LoginInfo </returns>
        /// <exception cref="StreamApiException">To obtain API specific error user Error property. WebException member gives the full web exception info</exception>
        /// <exception cref="ApplicationException">If Init() method was not called properly, i.e. it has not thrown an exception</exception>
        LoginInfo Login(string username, string password, int? apiSessionTimeout = null, StreamApiHeader streamApiHeader = null);

        /// <summary>
        /// Logs in using sessionID and returns LoginInfo containing Session info (SessionID), user info (user ID) and the cookie set via server
        /// </summary>
        /// <param name="sessionID">The sessionID obtained externally; cannot be null</param>
        /// <returns>LoginInfo </returns>
        /// <exception cref="StreamApiException">To obtain API specific error user Error property. WebException member gives the full web exception info</exception>
        /// <exception cref="ApplicationException">If Init() method was not called properly, i.e. it has not thrown an exception</exception>
        LoginInfo Login(string sessionID, StreamApiHeader streamApiHeader = null);

        /// <summary>
        /// Logs in and returns LoginInfo containing Session info (SessionID), user info (user ID) and the cookie set via server
        /// </summary>
        /// <param name="username">The username; cannot be null</param>
        /// <param name="password">The password; cannot be null</param>
        /// <param name="apiSessionTimeout">
        ///     Specified the timeout in seconds for Stream API session to stay alive. Can be null; in that case the timeout is defined by server.
        ///     Note: This works only if server is configured to use EhCache. When using HazelCast it ignores the attribute and by default uses 8 hours.
        /// </param>
        /// <returns>LoginInfo </returns>
        /// <exception cref="StreamApiException">To obtain API specific error user Error property. WebException member gives the full web exception info</exception>
        /// <exception cref="ApplicationException">If Init() method was not called properly, i.e. it has not thrown an exception</exception>
        Task<LoginInfo> LoginAsync(string username, string password, int? apiSessionTimeout = null, StreamApiHeader streamApiHeader = null, CancellationToken token = new CancellationToken());

        /// <summary>
        /// Logs in using sessionID and returns LoginInfo containing Session info (SessionID), user info (user ID) and the cookie set via server
        /// </summary>
        /// <param name="sessionID">The sessionID obtained externally; cannot be null</param>
        /// <returns>LoginInfo </returns>
        /// <exception cref="StreamApiException">To obtain API specific error user Error property. WebException member gives the full web exception info</exception>
        /// <exception cref="ApplicationException">If Init() method was not called properly, i.e. it has not thrown an exception</exception>
        Task<LoginInfo> LoginAsync(string sessionID, StreamApiHeader streamApiHeader = null, CancellationToken token = new CancellationToken());


        /// <summary>
        /// Logs in using username and the sessionID of another logged-in user and returns LoginInfo containing Session info (new SessionID), user info (user ID) and the cookie set via server
        /// </summary>
        /// <param name="username">The username; cannot be null</param>
        /// <param name="otherSessionID">Other user's sessionID; cannot be null</param>
        /// <returns>LoginInfo </returns>
        /// <exception cref="StreamApiException">To obtain API specific error user Error property. WebException member gives the full web exception info</exception>
        /// <exception cref="ApplicationException">If Init() method was not called properly, i.e. it has not thrown an exception</exception>
        LoginInfo LoginByOtherSessionID(string username, string otherSessionID, StreamApiHeader streamApiHeader = null);

        /// <summary>
        /// Logs out, i.e. the SessionID after this call becomes invalid
        /// </summary>
        /// <returns>void</returns>
        /// <exception cref="StreamApiException">To obtain API specific error user Error property. WebException member gives the full web exception info</exception>
        /// <exception cref="ApplicationException">If Init() method was not called properly, i.e. it has not thrown an exception</exception>
        void Logout(StreamApiHeader streamApiHeader = null);

        /// <summary>
        /// Returns the Stream API object specified by id and the type. Returns the fields of the object specified by fields argument.
        /// </summary>
        /// <typeparam name="T">Class of required object</typeparam>
        /// <param name="id">The id of required object; cannot be null</param>
        /// <param name="fields">Array of fields to be returned in the object requested; can be null: in that case the default fields will be returned.</param>
        /// <returns>The Stream API object with the specified id and T type</returns>
        /// <exception cref="StreamApiException">To obtain API specific error user Error property. WebException member gives the full web exception info</exception>
        /// <exception cref="ApplicationException">If Init() method was not called properly, i.e. it has not thrown an exception</exception>
        T Get<T>(string id, string[] fields, StreamApiHeader streamApiHeader = null) where T : EntityBase;

        /// <summary>
        /// Returns the Stream API objects specified by ids and the type. Returns the fields of the object specified by fields argument.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids">The array of ids of objects; cannot be null or empty array, or array of empty ids.</param>
        /// <param name="fields">Array of fields to be returned in the objects requested; can be null</param>
        /// <returns>The Stream API object list with the specified id and T type</returns>
        /// <exception cref="StreamApiException">To obtain API specific error user Error property. WebException member gives the full web exception info</exception>
        /// <exception cref="ApplicationException">If Init() method was not called properly, i.e. it has not thrown an exception</exception>
        List<T> Get<T>(string[] ids, string[] fields, StreamApiHeader streamApiHeader = null) where T : EntityBase;

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
        Task<T> GetAsync<T>(string id, string[] fields, CancellationToken token, StreamApiHeader streamApiHeader = null) where T : EntityBase;

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
        Task<List<T>> GetAsync<T>(string[] ids, string[] fields, CancellationToken token, StreamApiHeader streamApiHeader = null) where T : EntityBase;

        /// <summary>
        /// Returns the list of objects of type T which meet the criteria and/or namedQuery.
        /// </summary>
        /// <typeparam name="T">The type of object to be returned</typeparam>
        /// <param name="criteria">The list of name/value pairs as criteria for the server side search; can be null</param>
        /// <param name="fields">Array of fields to be returned in the object requested; can be null: in that case the default fields will be returned.</param>
        /// <param name="namedQuery">Option parameter to involve named query of Stream API</param>
        /// <returns>The list of objects which meet to the object type, criteria and/or namedQuery</returns>
        /// <exception cref="StreamApiException">To obtain API specific error user Error property. WebException member gives the full web exception info</exception>
        /// <exception cref="ApplicationException">If Init() method was not called properly, i.e. it has not thrown an exception</exception>
        List<T> Search<T>(List<StringPair> criteria, string[] fields, string namedQuery = null, StreamApiHeader streamApiHeader = null) where T : StreamBase;

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
        Task<List<T>> SearchAsync<T>(List<StringPair> criteria, string[] fields, CancellationToken token, string namedQuery = null, StreamApiHeader streamApiHeader = null) where T : StreamBase;

        /// <summary>
        /// Returns the <see cref="T:System.Threading.Tasks.Task"/> that represents the list of objects which meet to the builder criteria.
        /// </summary>
        /// <param name="builder">Solr search query builder</param>
        /// <param name="token">Cancellation token to observe cancellation request from client code.</param>
        /// <param name="streamApiHeader"></param>
        /// <returns>The <see cref="T:System.Threading.Tasks.Task"/> that represents the list of objects which meet to the builder criteria.</returns>
        /// <exception cref="StreamApiException">To obtain API specific error user Error property. WebException member gives the full web exception info</exception>
        /// <exception cref="ApplicationException">If Init() method was not called properly, i.e. it has not thrown an exception</exception>
        Task<List<T>> SolrSearchAsync<T>(SolrSearchBuilder<T> builder, CancellationToken token, StreamApiHeader streamApiHeader = null) where T : EntityBase, new();


        /// <summary>
        /// Returns the count of specified search object determined by criteria
        /// </summary>
        /// <typeparam name="T">The type of object</typeparam>
        /// <param name="criteria">The list of name/value pairs as criteria for the server side search; can be null</param>
        /// <returns>The number of objects found</returns>
        /// <exception cref="ApplicationException">If Init() method was not called properly, i.e. it has not thrown an exception</exception>
        int Count<T>(List<StringPair> criteria, StreamApiHeader streamApiHeader = null) where T : StreamBase;

        /// <summary>
        /// Returns the count of specified search object determined by criteria asynchronously
        /// </summary>
        /// <typeparam name="T">The type of object</typeparam>
        /// <param name="criteria">The list of name/value pairs as criteria for the server side search; can be null</param>
        /// <param name="token">Cancellation token to observe cancellation request from client code.</param>
        /// <returns>The number of objects found</returns>
        /// <exception cref="ApplicationException">If Init() method was not called properly, i.e. it has not thrown an exception</exception>
        Task<int> CountAsync<T>(List<StringPair> criteria, CancellationToken token, StreamApiHeader streamApiHeader = null) where T : StreamBase;

        /// <summary>
        /// Returns the count of specified search object determined by criteria asynchronously
        /// </summary>
        /// <typeparam name="T">The type of object</typeparam>
        /// <param name="criteria">The list of name/value pairs as criteria for the server side search; can be null</param>
        /// <param name="token">Cancellation token to observe cancellation request from client code.</param>
        /// <param name="namedQuery">Neame query to retrieve data</param>
        /// <param name="streamApiHeader"></param>
        /// <returns>The number of objects found</returns>
        /// <exception cref="ApplicationException">If Init() method was not called properly, i.e. it has not thrown an exception</exception>
        Task<int> CountAsync<T>(List<StringPair> criteria, string namedQuery, CancellationToken token, StreamApiHeader streamApiHeader = null) where T : StreamBase;

        /// <summary>
        /// Creates an object of type T with the fields specified in entity.
        /// </summary>
        /// <typeparam name="T">The type of object to be created</typeparam>
        /// <param name="entity">The object which contains the fields that will be set on object creation in server side</param>
        /// <param name="fields">Array of fields to be returned in the object requested; can be null: in that case the default fields will be returned.</param>
        /// <returns>The created object with the fields specified in appropriate argument</returns>
        /// <exception cref="StreamApiException">To obtain API specific error user Error property. WebException member gives the full web exception info</exception>
        /// <exception cref="ApplicationException">If Init() method was not called properly, i.e. it has not thrown an exception</exception>
        T Create<T>(T entity, string[] fields, StreamApiHeader streamApiHeader = null) where T : StreamBase;

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
        Task<T> CreateAsync<T>(T entity, string[] fields, CancellationToken token, StreamApiHeader streamApiHeader = null) where T : StreamBase;

        /// <summary>
        /// Updates the specified object with the fields set in entity.
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <param name="entity">The object to be updated</param>
        /// <param name="fields">Array of fields to be returned in the object requested; can be null: in that case the default fields will be returned.</param>
        /// <returns>The updated object with the fields specified</returns>
        /// <exception cref="StreamApiException">To obtain API specific error user Error property. WebException member gives the full web exception info</exception>
        /// <exception cref="ApplicationException">If Init() method was not called properly, i.e. it has not thrown an exception</exception>
        T Update<T>(T entity, string[] fields, StreamApiHeader streamApiHeader = null) where T : EntityBase;

        /// <summary>
        /// Updates the specified object with the fields set in entity.
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <param name="entity">The object to be updated</param>
        /// <param name="fields">Array of fields to be returned in the object requested; can be null: in that case the default fields will be returned.</param>
        /// <param name="token">Cancellation token to observe cancellation request from client code.</param>
        /// <param name="streamApiHeader"></param>
        /// <returns>The updated object with the fields specified</returns>
        /// <exception cref="StreamApiException">To obtain API specific error user Error property. WebException member gives the full web exception info</exception>
        /// <exception cref="ApplicationException">If Init() method was not called properly, i.e. it has not thrown an exception</exception>
        Task<T> UpdateAsync<T>(T entity, string[] fields, CancellationToken token, StreamApiHeader streamApiHeader = null) where T : EntityBase;

        /// <summary>
        /// Makes specified action on the object of type T identified by id.
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <typeparam name="U">The return type</typeparam>
        /// <param name="id">The id of the object of type T</param>
        /// <param name="actionName">Required action to perform on the object</param>
        /// <param name="criteria">The criteria/action parameters for the action needed; can be null</param>
        /// <param name="streamApiHeader"></param>
        /// <exception cref="StreamApiException">To obtain API specific error user Error property. WebException member gives the full web exception info</exception>
        /// <exception cref="ApplicationException">If Init() method was not called properly, i.e. it has not thrown an exception</exception>
        U Action<T, U>(string id, string actionName, List<StringPair> criteria = null, StreamApiHeader streamApiHeader = null) where T : EntityBase;

        /// <summary>
        /// Deletes the specified object of type T specified by id
        /// </summary>
        /// <typeparam name="T">The type of object</typeparam>
        /// <param name="id">The id of object</param>
        /// <param name="force">Flag identifying if force the delete operation</param>
        /// <exception cref="StreamApiException">To obtain API specific error user Error property. WebException member gives the full web exception info</exception>
        /// <exception cref="ApplicationException">If Init() method was not called properly, i.e. it has not thrown an exception</exception>
        void Delete<T>(string id, bool force = false, StreamApiHeader streamApiHeader = null) where T : EntityBase;

        /// <summary>
        /// Returns the report result specified by T type of object and criteria
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <typeparam name="U">The return type</typeparam>
        /// <param name="criteria">The list of criteria that Report is based on.</param>
        /// <returns>The report result of type E</returns>
        /// <exception cref="StreamApiException">To obtain API specific error user Error property. WebException member gives the full web exception info</exception>
        /// <exception cref="ApplicationException">If Init() method was not called properly, i.e. it has not thrown an exception</exception>
        U Report<T, U>(List<StringPair> criteria, StreamApiHeader streamApiHeader = null) where T : StreamBase;

        /// <summary>
        /// Returns Stream API metadata, i.e. the list of supported Stream API objects
        /// </summary>
        /// <returns></returns>
        Dictionary<string, Dictionary<string, ApiObjectInfo>> ApiMetadata(StreamApiHeader streamApiHeader = null);

        /// <summary>
        /// Returns Domain object's metadata info
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <returns>The dictionary of object metadata info</returns>
        DomainObjectInfo Metadata<T>(StreamApiHeader streamApiHeader = null) where T : StreamBase;

        /// <summary>
        /// Returns Server info, i.e. list of version and SSO info of the server.
        /// </summary>
        /// <returns>The ServerInfo object</returns>
        ServerInfo ApiServerInfo(StreamApiHeader streamApiHeader = null);

    }
}