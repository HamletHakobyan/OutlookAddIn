using System.Net;
using AtTask.OutlookAddIn.Domain;

namespace AtTask.OutlookAddIn.StreamApi.Helpers
{
    public class WebHelper
    {
        /// <summary>
        /// Tries to set proxy credentials if given proxyInfo is for given proxyHost.
        /// If sets returns true, otherwise false.
        /// </summary>
        /// <param name="proxyHost"></param>
        /// <param name="proxyInfo"></param>
        /// <returns></returns>
        public static NetworkCredential GetProxyCredentials(string proxyHost, ProxyInfo proxyInfo)
        {
            if (proxyInfo == null || proxyInfo.Address != proxyHost)
            {
                return null;
            }

            return new NetworkCredential(proxyInfo.Username, proxyInfo.Password);;
        }
    }
}
