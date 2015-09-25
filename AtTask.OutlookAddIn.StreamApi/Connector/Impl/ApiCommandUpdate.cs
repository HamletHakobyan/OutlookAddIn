using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AtTask.OutlookAddIn.StreamApi.Connector.Service;
using AtTask.OutlookAddIn.Domain.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AtTask.OutlookAddIn.StreamApi.Connector.Impl
{
    internal class ApiCommandUpdate<T> : ApiCommand where T : EntityBase
    {
        private T entity;

        public T Entity
        {
            get { return entity; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Null Entity");
                }
                if (value.ID == null)
                {
                    throw new ArgumentException("Null Entity ID");
                }

                this.entity = value;
            }
        }

        private string[] fields;

        public string[] Fields
        {
            get { return this.fields; }
            set
            {
                if (value != null)
                {
                    string[] tmp = NormalizeStringArray(value);
                    if (tmp == null)
                    {
                        throw new ArgumentException("Invalid fields array");
                    }
                    this.fields = tmp;
                }
                else
                {
                    this.fields = value;
                }
            }
        }

        public ApiCommandUpdate(IStreamApiConnector streamApiConnector, T entity, string[] fields, StreamApiHeader streamApiHeader = null)
            : base(streamApiConnector, streamApiHeader)
        {
            this.Entity = entity;
            this.Fields = fields;
            this.ReturnType = Entity.GetType();

            string fieldsParam = ComposeCommaSeparatedString(Fields);
            if (fieldsParam != null)
            {
                PostParams.Add(new StringPair(ApiConstants.ApiParamFields, fieldsParam));
            }

            //-- We make json without ID so that the ID is added to the URL path
            string tmpId = Entity.ID;
            Entity.ID = null;

            //--
            string entityJson = "";
            JsonSerializer serializer = new JsonSerializer()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Converters = { internetTimeConverter, enumConverter },
                ContractResolver = propertyNamesContractResolver
            };

            if (Entity.NullableFields != null && Entity.NullableFields.Count > 0)
            {
                JObject entityJObject = JObject.FromObject(Entity, serializer);
                if (Entity.NullableFields != null && Entity.NullableFields.Count > 0)
                {
                    InsertNullFields(entityJObject, Entity.NullableFields);
                }
                //if (Entity.CustomDataFields != null && Entity.CustomDataFields.Count > 0)
                //{
                //    InsertCustomDataFields(entityJObject, Entity.CustomDataFields);
                //}
                entityJson = entityJObject.ToString(Formatting.None);
            }
            else
            {
                entityJson = JsonSerializeWithoutNulls(Entity);
            }

            //--
            PostParams.Add(new StringPair(ApiConstants.ApiParamUpdates, entityJson));
            //-- restore the ID
            Entity.ID = tmpId;
        }

        private void InsertNullFields(JObject source, List<string> nullFields)
        {
            if (nullFields != null)
            {
                foreach (string field in nullFields)
                {
                    source.Add(field, null);
                }
            }
        }

        //private void InsertCustomDataFields(JObject source, Dictionary<string, object> customFields)
        //{
        //    if (customFields != null)
        //    {
        //        JsonSerializer serializer = new JsonSerializer()
        //        {
        //            NullValueHandling = NullValueHandling.Ignore,
        //            Converters = { internetTimeConverter, enumConverter },
        //            ContractResolver = propertyNamesContractResolver
        //        };

        //        foreach (KeyValuePair<string, object> field in customFields)
        //        {
        //            JToken entityJToken = JToken.FromObject(field.Value, serializer);
        //            source.Add(field.Key, entityJToken);
        //        }
        //    }
        //}

        protected override string CommandPath
        {
            get { return Entity.GetObjectType() + "/" + Entity.ID; }
        }

        protected override string ApiMethod
        {
            get { return ApiConstants.ApiMethodPut; }
        }

        public T Execute()// throws StreamAPIErrorException, Exception
        {
            ResponseJsonStatusCode resAndCode = MakeRequest();
            JsonEntityRoot<T> jsonRoot = JsonConvert.DeserializeObject<JsonEntityRoot<T>>(resAndCode.Json,
                                new JsonSerializerSettings()
                                {
                                    Converters = { internetTimeConverter, enumConverter },
                                    ContractResolver = propertyNamesContractResolver
                                });
            T ret = jsonRoot.Data;
            return ret;
        }
        public async Task<T> ExecuteAsync(CancellationToken token)
        {
            ResponseJsonStatusCode resAndCode = await MakeRequestAsync(token).ConfigureAwait(false);
            JsonEntityRoot<T> jsonRoot = JsonConvert.DeserializeObject<JsonEntityRoot<T>>(resAndCode.Json,
                                new JsonSerializerSettings()
                                {
                                    Converters = { internetTimeConverter, enumConverter },
                                    ContractResolver = propertyNamesContractResolver
                                });
            return jsonRoot.Data;
        }
    }
}