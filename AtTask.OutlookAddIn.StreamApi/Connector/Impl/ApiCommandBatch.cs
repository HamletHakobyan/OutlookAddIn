using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AtTask.OutlookAddIn.StreamApi.Connector.Service;
using AtTask.OutlookAddIn.Domain.Model;
using AtTask.OutlookAddIn.Utilities.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AtTask.OutlookAddIn.StreamApi.Connector.Impl
{
    internal class ApiCommandBatch : ApiCommand, IBatchQuery
    {
        private List<ApiCommand> queryList = new List<ApiCommand>();

        public ApiCommandBatch(IStreamApiConnector streamApiConnector, StreamApiHeader streamApiHeader = null)
            : base(streamApiConnector, streamApiHeader)
        {
        }

        #region IBatchQuery

        public void Get<T>(string id, string[] fields) where T : EntityBase
        {
            ApiCommandGet<T> cmd = new ApiCommandGet<T>(StreamApiConnector, id, fields);
            queryList.Add(cmd);
        }

        public void Get<T>(string[] ids, string[] fields) where T : EntityBase
        {
            ApiCommandGet<T> cmd = new ApiCommandGet<T>(StreamApiConnector, ids, fields);
            queryList.Add(cmd);
        }

        public void Search<T>(List<StringPair> criteria, string[] fields, string namedQuery = null) where T : StreamBase
        {
            ApiCommandSearch<T> cmd = new ApiCommandSearch<T>(StreamApiConnector, criteria, fields, namedQuery);
            queryList.Add(cmd);
        }

        public void Count<T>(List<StringPair> criteria) where T : StreamBase
        {
            ApiCommandCount<T> cmd = new ApiCommandCount<T>(StreamApiConnector, criteria);
            queryList.Add(cmd);
        }

        public void Create<T>(T entity, string[] fields) where T : StreamBase
        {
            ApiCommandCreate<T> cmd = new ApiCommandCreate<T>(StreamApiConnector, entity, fields);
            queryList.Add(cmd);
        }

        public void Update<T>(T entity, string[] fields) where T : EntityBase
        {
            ApiCommandUpdate<T> cmd = new ApiCommandUpdate<T>(StreamApiConnector, entity, fields);
            queryList.Add(cmd);
        }

        public void Action<T, U>(string id, string action, List<StringPair> criteria = null) where T : EntityBase
        {
            ApiCommandAction<T, U> cmd = new ApiCommandAction<T, U>(StreamApiConnector, id, action, criteria);
            queryList.Add(cmd);
        }

        public void Delete<T>(string id, bool force = false) where T : EntityBase
        {
            ApiCommandDelete<T> cmd = new ApiCommandDelete<T>(StreamApiConnector, id, force);
            queryList.Add(cmd);
        }

        public void Report<T, U>(List<StringPair> criteria) where T : StreamBase
        {
            ApiCommandReport<T, U> cmd = new ApiCommandReport<T, U>(StreamApiConnector, criteria);
            queryList.Add(cmd);
        }

        public void ApiMetadata()
        {
            ApiCommand cmd = new ApiCommandApiMetadata(StreamApiConnector);
            queryList.Add(cmd);
        }

        public void Metadata<T>() where T : StreamBase
        {
            ApiCommand cmd = new ApiCommandMetadata<T>(StreamApiConnector);
            queryList.Add(cmd);
        }

        public object[] Execute(bool atomic = false)
        {
            PostParams.Clear();

            List<Type> returnTypeList = new List<Type>();
            if (atomic)
            {
                PostParams.Add(new StringPair(ApiConstants.ApiParamAtomic, bool.TrueString.ToLower()));
                ReturnType = typeof(SuccessResult);
                returnTypeList.Add(ReturnType);
            }
            else
            {
                foreach (ApiCommand cmd in queryList)
                {
                    returnTypeList.Add(cmd.ReturnType);
                }
                ReturnType = typeof(List<Type>);
            }
            object[] ret = null;

             List<StringPair> myParams = PrepareCommandParams(true);
            PostParams.AddRange(myParams);
            ResponseJsonStatusCode resAndCode = MakeRequest();

            JObject root = JObject.Parse(resAndCode.Json);
            JToken rootElem = root["data"];
            //-- Normal Batch non-atomic result
            if (rootElem is JArray)
            {
                JArray dataArr = rootElem as JArray;
                IList<JToken> dataList = dataArr.Children().ToList();
                ret = new Object[dataList.Count];
                int i = 0;
                Type jsonConverType = typeof(JsonConvert);
                foreach (JToken token in dataList)
                {
                    //-- Find out if json object is single or array object
                    JToken dataElem = token["data"];
                    MethodInfo deserializeMethod = null;
                    Type resType = returnTypeList[i];
                    
                    if (dataElem is JObject)
                    {
                        //deserializeMethod = jsonConverType.GetGenericMethod("DeserializeObject", new[] { typeof(JsonEntityRoot<>).MakeGenericType(resType) }, new[] { typeof(string) }, null);
                        deserializeMethod = jsonConverType.GetGenericMethod("DeserializeObject", new[] { resType }, new[] { typeof(string), typeof(JsonConverter[]) }, null);
                    }
                    else if (dataElem is JArray)
                    {
                        //deserializeMethod = jsonConverType.GetGenericMethod("DeserializeObject", new[] { typeof(JsonEntityRoot<>).MakeGenericType(typeof(List<>).MakeGenericType(resType)) }, new[] { typeof(string) }, null);
                        deserializeMethod = jsonConverType.GetGenericMethod("DeserializeObject", new[] { typeof(List<>).MakeGenericType(resType) }, new[] { typeof(string), typeof(JsonConverter[]) }, null);
                    }

                    object retObject = deserializeMethod.Invoke(null, new object[] { dataElem.ToString(), new JsonConverter[] { internetTimeConverter } });
                    ret[i] = retObject;
                    ++i;
                }
            }
            else if (rootElem is JObject)   //-- Atomic response
            {
                JsonEntityRoot<SuccessResult> jsonRoot = JsonConvert.DeserializeObject<JsonEntityRoot<SuccessResult>>(resAndCode.Json);
                SuccessResult successRes = jsonRoot.Data;
                ret = new Object[1];
                ret[0] = successRes;
            }

            return ret;
        }


        public async Task<IList<object>> ExecuteAsync(CancellationToken cancellationToken, bool atomic = false)
        {
            PostParams.Clear();

            var returnTypeList = new List<Type>();
            if (atomic)
            {
                PostParams.Add(new StringPair(ApiConstants.ApiParamAtomic, bool.TrueString.ToLower()));
                ReturnType = typeof(SuccessResult);
                returnTypeList.Add(ReturnType);
            }
            else
            {
                foreach (ApiCommand cmd in queryList)
                {
                    returnTypeList.Add(cmd.ReturnType);
                }
                ReturnType = typeof(List<Type>);
            }

            List<StringPair> myParams = PrepareCommandParams(true);
            PostParams.AddRange(myParams);
            ResponseJsonStatusCode resAndCode = await MakeRequestAsync(cancellationToken).ConfigureAwait(false);

            JObject root = JObject.Parse(resAndCode.Json);
            JToken rootElem = root["data"];
            //-- Normal Batch non-atomic result
            return await ParseJsonAsync(rootElem, returnTypeList, resAndCode, cancellationToken).ConfigureAwait(false);
        }

        private Task<object[]> ParseJsonAsync(JToken rootElem, List<Type> returnTypeList,
            ResponseJsonStatusCode resAndCode, CancellationToken cancellationToken)
        {
            return System.Threading.Tasks.Task.Factory.StartNew(
                    () => ParseJson(rootElem, returnTypeList, resAndCode, cancellationToken), cancellationToken);
        }

        private object[] ParseJson(JToken rootElem, List<Type> returnTypeList, ResponseJsonStatusCode resAndCode, CancellationToken cancellationToken)
        {
            object[] ret = null;
            if (rootElem is JArray)
            {
                JArray dataArr = rootElem as JArray;
                IList<JToken> dataList = dataArr.Children().ToList();
                ret = new Object[dataList.Count];
                int i = 0;
                Type jsonConverType = typeof (JsonConvert);
                foreach (JToken token in dataList)
                {
                    //-- Find out if json object is single or array object
                    JToken dataElem = token["data"];
                    MethodInfo deserializeMethod = null;
                    Type resType = returnTypeList[i];

                    if (dataElem is JObject)
                    {
                        //deserializeMethod = jsonConverType.GetGenericMethod("DeserializeObject", new[] { typeof(JsonEntityRoot<>).MakeGenericType(resType) }, new[] { typeof(string) }, null);
                        deserializeMethod = jsonConverType.GetGenericMethod("DeserializeObject", new[] {resType},
                            new[] {typeof (string), typeof (JsonConverter[])}, null);
                    }
                    else if (dataElem is JArray)
                    {
                        //deserializeMethod = jsonConverType.GetGenericMethod("DeserializeObject", new[] { typeof(JsonEntityRoot<>).MakeGenericType(typeof(List<>).MakeGenericType(resType)) }, new[] { typeof(string) }, null);
                        deserializeMethod = jsonConverType.GetGenericMethod("DeserializeObject",
                            new[] {typeof (List<>).MakeGenericType(resType)}, new[] {typeof (string), typeof (JsonConverter[])},
                            null);
                    }

                    object retObject = deserializeMethod.Invoke(null,
                        new object[] {dataElem.ToString(), new JsonConverter[] {internetTimeConverter}});
                    ret[i] = retObject;
                    ++i;

                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
            else if (rootElem is JObject) //-- Atomic response
            {
                JsonEntityRoot<SuccessResult> jsonRoot =
                    JsonConvert.DeserializeObject<JsonEntityRoot<SuccessResult>>(resAndCode.Json);
                SuccessResult successRes = jsonRoot.Data;
                ret = new Object[1];
                ret[0] = successRes;
            }

            return ret;
        }


        private List<StringPair> PrepareCommandParams(bool urlEncoded)
        {
            List<StringPair> ret = new List<StringPair>();
            foreach (ApiCommand cmd in queryList)
            {
                ret.Add(new StringPair(ApiConstants.ApiParamUri, cmd.GetCommandUriString(urlEncoded)));
            }
            return ret;
        }

        public override string GetCommandUriString(bool urlEncoded = false)
        {
            List<StringPair> myParams = PrepareCommandParams(urlEncoded);
            FinalizePostParams(myParams);
            string strQuery = MakeFormParamsAsPostData(myParams, urlEncoded);
            return string.Format("/{0}?{1}", CommandPath, strQuery);
        }

        public override Uri CommandUri
        {
            get
            {
                Uri uri = StreamApiConnector.UriBase;
                Uri cmdUri;
                if (Uri.TryCreate(uri, CommandPath, out cmdUri))
                {
                    uri = cmdUri;
                }
                List<StringPair> myParams = PrepareCommandParams(true);
                FinalizePostParams(myParams);
                string strPostParams = MakeFormParamsAsPostData(myParams, true);
                string uriStr = string.Format("{0}?{1}", cmdUri.ToString(), strPostParams);
                return new Uri(uriStr);
            }
        }

        #endregion IBatchQuery

        #region ApiCommand

        protected override string CommandPath
        {
            get
            {
                return ApiConstants.ApiPathBatch;
            }
        }

        protected override string ApiMethod
        {
            get { return ApiConstants.ApiMethodGet; }
        }

        #endregion ApiCommand
    }
}