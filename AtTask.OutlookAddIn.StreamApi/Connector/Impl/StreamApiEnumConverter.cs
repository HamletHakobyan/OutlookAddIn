using System;
using AtTask.OutlookAddIn.Domain.Model;
using Newtonsoft.Json.Converters;

namespace AtTask.OutlookAddIn.StreamApi.Connector.Impl
{
    public class StreamApiEnumConverter : StringEnumConverter
    {
        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (reader.Value != null)
            {
                Type t = GetTypeIfNullable(objectType);
                if (t.IsEnum && !t.IsEnumDefined(reader.Value))
                {
                    if (objectType.IsValueType)
                    {
                        return Activator.CreateInstance(objectType);
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            return base.ReadJson(reader, objectType, existingValue, serializer);
        }

        public override bool CanConvert(Type objectType)
        {
            Type t = GetTypeIfNullable(objectType);

            return (t.IsEnum && !Attribute.IsDefined(t, typeof(SuppressEnumStringConversionAttribute)));
        }

        private Type GetTypeIfNullable(Type objectType)
        {
            Type t;
            if (objectType.IsValueType &&
                objectType.IsGenericType &&
                objectType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                t = Nullable.GetUnderlyingType(objectType);
            }
            else
            {
                t = objectType;
            }

            return t;
        }
    }
}