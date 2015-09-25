using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AtTask.OutlookAddIn.Domain.Model;

namespace AtTask.OutlookAddIn.StreamApi.Connector.Service
{
    /// <summary>
    /// Through this interface the client code can make Stream API Batch queries.
    /// The details about Batch queries you can find in Stream API doc.
    /// The methods here are the same as in IStreamAPIConnector.
    /// <example>
    /// <code>
    ///     IStreamApiConnector connector = StreamApiConnectorFactory.NewInstance();
    ///     IBatchQuery batch = connector.BatchQuery;
    ///     batch.Get<Task>("123456", null);
    ///     batch.Search<Project>(null, null);
    ///     object[] objArray = batch.Execute();
    /// </code>
    /// </example>
    /// You can use any number of method calls and then call Execute() method.
    ///
    /// All the methods have the same signature as in IStreamApiConnector, except return type.
    ///
    /// </summary>
    public interface IBatchQuery
    {
        /// <summary>
        /// The equivaltent of <see cref="IStreamApiConnector.Get{T}(string,string[],AtTask.OutlookAddIn.StreamApi.Connector.Service.StreamApiHeader)"/> method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="fields"></param>
        void Get<T>(string id, string[] fields) where T : EntityBase;

        /// <summary>
        /// The equivaltent to Get<T>(string[] ids, string[] fields) method of IStreamApiConnector
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <param name="fields"></param>
        void Get<T>(string[] ids, string[] fields) where T : EntityBase;

        /// <summary>
        /// The equivaltent to Search<T>(List<StringPair> criteria, string[] fields, string namedQuery = null) method of IStreamApiConnector
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        /// <param name="fields"></param>
        /// <param name="namedQuery"></param>
        void Search<T>(List<StringPair> criteria, string[] fields, string namedQuery = null) where T : StreamBase;

        /// <summary>
        /// The equivaltent to Count<T>(List<StringPair> criteria) method of IStreamApiConnector
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="criteria"></param>
        void Count<T>(List<StringPair> criteria) where T : StreamBase;

        /// <summary>
        /// The equivaltent to Create<T>(T bean, string[] fields) method of IStreamApiConnector
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bean"></param>
        /// <param name="fields"></param>
        void Create<T>(T bean, string[] fields) where T : StreamBase;

        /// <summary>
        /// The equivaltent to Update<T>(T bean, string[] fields) method of IStreamApiConnector
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bean"></param>
        /// <param name="fields"></param>
        void Update<T>(T bean, string[] fields) where T : EntityBase;

        /// <summary>
        /// The equivaltent to Action<T, U>(string id, string action, List<StringPair> criteria = null) method of IStreamApiConnector
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <typeparam name="U">The return type</typeparam>
        /// <param name="id"></param>
        /// <param name="action"></param>
        /// <param name="criteria"></param>
        void Action<T, U>(string id, string action, List<StringPair> criteria = null) where T : EntityBase;

        /// <summary>
        /// The equivaltent to Delete<T>(String id, bool force = false) method of IStreamApiConnector
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="force"></param>
        void Delete<T>(String id, bool force = false) where T : EntityBase;

        /// <summary>
        /// The equivaltent to Report() method of IStreamApiConnector interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="criteria"></param>
        void Report<T, U>(List<StringPair> criteria) where T : StreamBase;

        /// <summary>
        /// The equivalent to ApiMetadata() method of IStreamApiConnector interface
        /// </summary>
        void ApiMetadata();

        /// <summary>
        /// The equivalent to Metadata<T>() method of IStreamApiConnector interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void Metadata<T>() where T : StreamBase;

        /// <summary>
        /// Makes Stream API batch query based on previously called methods.
        /// Returns the response as an array of responses as if they were called one by one.
        /// If the parameter "atomic" is set to true, then the response array contains only one SuccessResult object.
        /// </summary>
        /// <param name="atomic">If true, the all the queries will be executed at server side in one transaction.
        /// Otherwise the calls will be done in the order that the clilent code called the methods of this interface.</param>
        /// <returns></returns>
        /// <exception cref="StreamApiException"></exception>
        object[] Execute(bool atomic = false);

        /// <summary>
        /// Makes Stream API batch query based on previously called methods.
        /// Returns the <see cref="T:System.Threading.Tasks.Task"/> which represents the response as an array of responses as if they were called one by one.
        /// If the parameter "atomic" is set to true, then the response array contains only one SuccessResult object.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to observe cancellation request from client code.</param>
        /// <param name="atomic">If true, the all the queries will be executed at server side in one transaction.
        /// Otherwise the calls will be done in the order that the clilent code called the methods of this interface.</param>
        /// <returns></returns>
        /// <exception cref="StreamApiException"></exception>
        Task<IList<object>> ExecuteAsync(CancellationToken cancellationToken, bool atomic = false);

        /// <summary>
        /// Returns the Uri string representation of Batch query according to Stream API spec (except ApiUriRoot part)
        /// </summary>
        /// <param name="urlEncoded"></param>
        /// <returns></returns>
        string GetCommandUriString(bool urlEncoded = false);

        /// <summary>
        /// Returns the Uri representing the Batch query according to Stream API spec
        /// </summary>
        Uri CommandUri { get; }
    }
}