using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workfront.OutlookAddIn.Infrastructure
{
    public struct ConnectionSettings
    {
        public string Host { get; set; }
        public string UserAgent { get; set; }
        public string SessionId { get; set; }
    }
}
