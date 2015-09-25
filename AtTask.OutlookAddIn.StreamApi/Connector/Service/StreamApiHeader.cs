using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using AtTask.OutlookAddIn.Domain.Model;

namespace AtTask.OutlookAddIn.StreamApi.Connector.Service
{
    public class StreamApiHeader
    {
        public CultureInfo Culture { get; set; }
        public UserAgent UserAgent { get; set; }
    }
}
