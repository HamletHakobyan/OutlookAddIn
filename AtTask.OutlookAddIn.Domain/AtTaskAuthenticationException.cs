using System;
using System.Text;
using Workfront.OutlookAddIn.Infrastructure;

namespace AtTask.OutlookAddIn.Domain
{
    /// <summary>
    /// Login exception occurs when something is wrong during log in try.
    /// </summary>
    public class AtTaskAuthenticationException : AtTaskException
    {
        private ConnectionInfo connectionInfo;

        /// <summary>
        /// Gets connection info, which contains details used while trying to log in.
        /// </summary>
        public ConnectionInfo ConnectionInfo
        {
            get
            {
                return connectionInfo;
            }
        }
        
        /// <summary>
        /// Initializes a new _instance of the AtTaskException class with a specified error message.
        /// </summary>
        /// <param name="message"></param>
        public AtTaskAuthenticationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates AtTaskLoginException _instance with connection details used while trying to log in
        /// and inner exception which occured during log in.
        /// </summary>
        /// <param name="connectionInfo"></param>
        /// <param name="exception"></param>
        public AtTaskAuthenticationException(ConnectionInfo connectionInfo, Exception exception)
            : base(exception)
        {
            this.connectionInfo = connectionInfo;
        }

        protected override void ToStringInternal(StringBuilder builder)
        {
            if (connectionInfo != null)
            {
                builder.AppendLine("\tConnectionInfo");
                builder.Append("\t\tHost: ").AppendLine(connectionInfo.Host);
                builder.Append("\t\tUsername: ").AppendLine(connectionInfo.Username);
                builder.Append("\t\tPassword: ").AppendLine("********");
            }
        }
    }
}