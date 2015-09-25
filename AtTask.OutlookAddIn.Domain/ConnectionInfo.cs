using System;
using System.Collections.Generic;
using System.Linq;

namespace AtTask.OutlookAddIn.Domain
{
    [Serializable()]
    /// <summary>
    /// Contains information, which is necessary to login.
    /// </summary>
    public class ConnectionInfo
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Host { get; set; }

        public string FullHost { get; set; }

        public bool IsSAML2 { get; set; }

        public string SsoSessionId { get; set; }

        public int? Code { get; set; }

        /// <summary>
        /// The key is the URL string of proxy, value is Username-Password pair.
        /// </summary>
        public ProxyInfo ProxyInfo { get; set; }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj is ConnectionInfo)//if obj is null this is false
            {
                ConnectionInfo that = (ConnectionInfo)obj;
                if ((this.Username == that.Username) && (this.Password == that.Password) && (this.Host == that.Host))
                {
                    if (ProxyInfo == null)
                    {
                        if (that.ProxyInfo == null)
                        {
                            return true;
                        }
                    }
                    else if (ProxyInfo.Equals(that.ProxyInfo))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hashCode = Username.GetHashCode();
            hashCode = 29 * hashCode + Password.GetHashCode();
            hashCode = 29 * hashCode + Host.GetHashCode();
            hashCode = 29 * hashCode;
            if (ProxyInfo != null)
            {
                hashCode += ProxyInfo.GetHashCode();
            }
            return hashCode;
        }

        public override string ToString()
        {
            string username = Username != null ? Username : "-";
            string host = Host != null ? Host : "-";
            return string.Format("{0} [{1}, proxy: {2}]", username, host, ProxyInfo != null);
        }
    }
}