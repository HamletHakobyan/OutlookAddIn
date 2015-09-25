using System.Collections.Generic;

namespace AtTask.OutlookAddIn.Domain.Model
{
    /// <summary>
    /// Represent Stream API error result class
    /// </summary>
    public class StreamApiError
    {
        public string Message { get; set; }

        public string MsgKey { get; set; }

        public string Class { get; set; }

        public string Title { get; set; }

        public int? Code { get; set; }

        public List<string> Attributes { get; set; }
    }
}