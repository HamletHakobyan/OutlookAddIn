using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AtTask.OutlookAddIn.Core.JsonConverters
{
    public class ParameterValuesConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType != typeof(Dictionary<string, object>))
            {
                return null;
            }

            reader.Read();
            if (reader.TokenType == JsonToken.EndObject)
            {
                return null;
            }
            Dictionary<string, object> result = new Dictionary<string, object>();

            while (reader.TokenType != JsonToken.EndObject)
            {
                while (reader.TokenType == JsonToken.PropertyName)
                {
                    //-- this is the key
                    string paramName = (string)reader.Value;
                    object paramVal = null;
                    reader.Read();
                    if (reader.TokenType == JsonToken.StartArray)
                    {
                        List<object> lst = new List<object>();
                        reader.Read();
                        while (reader.TokenType != JsonToken.EndArray)
                        {
                            lst.Add(reader.Value);
                            reader.Read();
                        }
                        paramVal = lst;
                    }
                    else
                    {
                        paramVal = reader.Value;
                    }
                    result.Add(paramName, paramVal);
                    reader.Read();
                }
            }

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}