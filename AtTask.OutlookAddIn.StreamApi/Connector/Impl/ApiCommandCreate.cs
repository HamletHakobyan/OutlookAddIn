using System;
using System.Threading;
using System.Threading.Tasks;
using AtTask.OutlookAddIn.StreamApi.Connector.Service;
using AtTask.OutlookAddIn.Domain.Model;
using Newtonsoft.Json;

namespace AtTask.OutlookAddIn.StreamApi.Connector.Impl
{
    internal class ApiCommandCreate<T> : ApiCommand where T : StreamBase
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

        public ApiCommandCreate(IStreamApiConnector streamApiConnector, T entity, string[] fields, StreamApiHeader streamApiHeader = null)
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

            string entityJson = JsonSerializeWithoutNulls(Entity);
            PostParams.Add(new StringPair(ApiConstants.ApiParamUpdates, entityJson));
        }

        protected override string CommandPath
        {
            get { return Entity.GetObjectType(); }
        }

        protected override string ApiMethod
        {
            get { return ApiConstants.ApiMethodPost; }
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

        public async Task<T> ExecuteAsync(CancellationToken token)// throws StreamAPIErrorException, Exception
        {
            ResponseJsonStatusCode resAndCode = await MakeRequestAsync(token).ConfigureAwait(false);
            JsonEntityRoot<T> jsonRoot = JsonConvert.DeserializeObject<JsonEntityRoot<T>>(resAndCode.Json,
                new JsonSerializerSettings()
                {
                    Converters = { internetTimeConverter, enumConverter },
                    ContractResolver = propertyNamesContractResolver
                });
            T ret = jsonRoot.Data;
            return ret;
        }
    }
}