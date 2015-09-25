using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using AtTask.OutlookAddin.Utilities;

namespace AtTask.OutlookAddIn.Utilities
{
    public static class WebUtil
    {
        private static readonly object proxyLock = new object();

        /// <summary>
        /// Dedicated to get the default proxy for given host, to make further calls faster.
        /// </summary>
        /// <param name="host"></param>
        public static void Ping(string host)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(host);

                lock (proxyLock)
                {
                    request.Proxy = HttpWebRequest.DefaultWebProxy;
                }
            }
            catch (Exception)
            {
                //do nothing as ping is not important
            }
        }

        /// <summary>
        /// Returns current proxy host for given request, if any.
        /// Otherwise returns null.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetProxyHost(HttpWebRequest request)
        {
            lock (proxyLock)
            {
                //this is the default behavior, set this only for readability
                //first get of DefaultWebProxy is slow, so process it in lock
                request.Proxy = HttpWebRequest.DefaultWebProxy;
            }

            Uri proxyUri = HttpWebRequest.DefaultWebProxy.GetProxy(request.RequestUri);

            if (request.RequestUri.Equals(proxyUri))
            {
                return null;
            }

            return GetBaseHostWithPort(proxyUri);
        }

        public static WebProxyInfo GetProxy(Uri hostUri)
        {
            var webProxy = WebRequest.DefaultWebProxy;
            var proxy = webProxy.GetProxy(hostUri);
            if (proxy != hostUri)
            {
                return new WebProxyInfo
                {
                    WebProxy = webProxy,
                    ProxyAddress = proxy.AbsoluteUri.TrimEnd('/'),
                };
            }

            return null;
        }

        /// <summary>
        /// Returns string representaion of uri's base.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string GetBaseHostWithPort(Uri uri)
        {
            return new StringBuilder(uri.Scheme).Append(Uri.SchemeDelimiter).Append(uri.Host)
                .Append(":").Append(uri.Port).ToString();
        }

        /// <summary>
        /// Retrieves host and port from uri string.
        /// Returns true if parse is OK.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool ParseHostAndPort(string uriString, out string host, out int port)
        {
            Uri uri;
            try
            {
                uri = new Uri(uriString);
            }
            catch (Exception)
            {
                host = null;
                port = -1;
                return false;
            }

            host = uri.Host;
            port = uri.Port;

            return true;
        }

        /// <summary>
        /// Reads response stream and returns response string.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static string GetResponseString(HttpWebResponse response)
        {
            Stream dataStream = null;
            StreamReader reader = null;

            try
            {
                if (response.ContentEncoding.ToUpperInvariant() == "GZIP")
                {
                    dataStream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
                }
                else
                {
                    dataStream = response.GetResponseStream();
                }
                reader = new StreamReader(dataStream);
                return reader.ReadToEnd();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    try
                    {
                        reader.Close();
                    }
                    catch (Exception)
                    { }
                }

                IOUtil.CloseStream(dataStream);
            }
        }

        /// <summary>
        /// Returs a string, which has the following parts separated by "*".
        /// - The raw offset for the timezone in millis for January 1st in current year.
        /// - The raw offset for the timezone in millis for July 1st in current year.
        /// - The fall transition date for the current year.
        /// - The spring transition date for the current year.
        /// If in the timezone there is not DST, then the spring/fall transition dates are missing
        /// </summary>
        /// <returns></returns>
        public static string GetTimezoneCookieString()
        {
            DateTime today = DateTime.Now;
            TimeZone localZone = TimeZone.CurrentTimeZone;
            DaylightTime dlt = localZone.GetDaylightChanges(today.Year);
            string[] valArr = null;
            if (dlt.Start != DateTime.MinValue)
            {
                valArr = new string[4];
            }
            else
            {
                valArr = new string[2];
            }

            DateTime jan = new DateTime(today.Year, 1, 1);
            DateTime jul = new DateTime(today.Year, 7, 1);
            double janTzShift = localZone.GetUtcOffset(jan).TotalMilliseconds;
            double julTzShift = localZone.GetUtcOffset(jul).TotalMilliseconds;
            valArr[0] = (-janTzShift).ToString();
            valArr[1] = (-julTzShift).ToString();

            if (dlt.Start != DateTime.MinValue)
            {
                DateTime dStart = dlt.Start.AddHours(1);
                DateTime dEnd = dlt.End.AddHours(-1);

                DateTime utcStart = new DateTime(1970, 1, 1);
                TimeSpan valStart = dStart - utcStart;
                TimeSpan valEnd = dEnd - utcStart;
                string txtStart = valStart.TotalMilliseconds.ToString();
                string txtEnd = valEnd.TotalMilliseconds.ToString();
                if (dEnd.CompareTo(dStart) > 0)
                {
                    valArr[2] = txtEnd;
                    valArr[3] = txtStart;
                }
                else
                {
                    valArr[2] = txtStart;
                    valArr[3] = txtEnd;
                }
            }

            return string.Join("*", valArr);
        }

        /// <summary>
        /// Returns whether given string is valid absoulute URI (tries also with appending https://).
        /// </summary>
        /// <param name="uriString"></param>
        /// <returns></returns>
        public static bool IsValidAbsoluteUri(string uriString)
        {
            Uri uri;
            if (Uri.TryCreate(uriString, UriKind.Absolute, out uri))
            {
                return true;
            }
            if (Uri.TryCreate("https://" + uriString, UriKind.Absolute, out uri))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the string itself if it is valid URL or appends https:// if it is valid with it.
        /// Otherwise returns null.
        /// </summary>
        /// <param name="uriString"></param>
        /// <returns></returns>
        public static string GetValidAbsoluteUriString(string uriString)
        {
            Uri uri;
            if (uriString != null)
            {
                if ((uriString.StartsWith("http://") || uriString.StartsWith("https://")) && Uri.TryCreate(uriString, UriKind.Absolute, out uri))
                {
                    return uriString;
                }
                uriString = "https://" + uriString;
                if (Uri.TryCreate(uriString, UriKind.Absolute, out uri))
                {
                    return uriString;
                }
            }
            return null;
        }

        public static Uri GetValidAbsoluteUri(string uriString)
        {
            Uri uri;
            if (uriString != null)
            {
                if ((uriString.StartsWith("http://") || uriString.StartsWith("https://")) && Uri.TryCreate(uriString, UriKind.Absolute, out uri))
                {
                    return uri;
                }

                uriString = "https://" + uriString;
                if (Uri.TryCreate(uriString, UriKind.Absolute, out uri))
                {
                    return uri;
                }
            }
            return null;
        }

        public static string EncodeJson(string json)
        {
            var s =  System.Web.Helpers.Json.Encode(json);
            return s.Substring(1, s.Length - 2);
        }
    }
}
