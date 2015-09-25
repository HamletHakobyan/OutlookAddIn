using System.Globalization;
using Newtonsoft.Json.Converters;

namespace AtTask.OutlookAddIn.Core.JsonConverters
{
    public class IsoDateConverter : IsoDateTimeConverter
    {
        public IsoDateConverter()
        {
            DateTimeFormat = "yyyy-MM-dd";
            Culture = CultureInfo.InvariantCulture;
        }
    }
}