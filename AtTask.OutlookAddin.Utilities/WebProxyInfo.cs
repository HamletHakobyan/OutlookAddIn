using System;
using System.Net;

namespace AtTask.OutlookAddin.Utilities
{
    public class WebProxyInfo
    {
        public IWebProxy WebProxy { get; set; }
        public string ProxyAddress { get; set; }
    }
}