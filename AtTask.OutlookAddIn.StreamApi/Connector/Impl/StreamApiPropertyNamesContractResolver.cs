using System.Globalization;
using AtTask.OutlookAddIn.Utilities;
using Newtonsoft.Json.Serialization;

namespace AtTask.OutlookAddIn.StreamApi.Connector.Impl
{
    public class StreamApiPropertyNamesContractResolver : CamelCasePropertyNamesContractResolver
    {
        /// <summary>
        /// Resolves the name of the property.
        /// If the first two letters of the name are upper-case or property consists of one letter does nothing with it.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The property name camel cased.</returns>
        protected override string ResolvePropertyName(string propertyName)
        {
            return StringUtil.CamelCaseName(propertyName);
        }
    }
}