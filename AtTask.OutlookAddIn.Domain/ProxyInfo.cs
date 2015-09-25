using System;

namespace AtTask.OutlookAddIn.Domain
{
    [Serializable()]
    /// <summary>
    /// Contains information about proxy address, username and password.
    /// </summary>
    public class ProxyInfo
    {
        public string Address { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public override bool Equals(object obj)
        {
            ProxyInfo that = obj as ProxyInfo;
            if (that == null)
            {
                return false;
            }

            if (this == that)
            {
                return true;
            }

            //consider true only the case when all the values are set
            return (this.Address != null && this.Address.Equals(that.Address) &&
                this.Username != null && this.Username.Equals(that.Username) &&
                this.Password != null && this.Password.Equals(that.Password));
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            if (Address != null)
            {
                hashCode += Address.GetHashCode();
            }
            hashCode = 29 * hashCode;
            if (Username != null)
            {
                hashCode += Username.GetHashCode();
            }
            hashCode = 29 * hashCode;
            if (Password != null)
            {
                hashCode += Password.GetHashCode();
            }
            return hashCode;
        }

        public override string ToString()
        {
            return string.Format("[Address: {0}, Username: {1}, Password: {2}]", Address, Username, Password);
        }
    }
}