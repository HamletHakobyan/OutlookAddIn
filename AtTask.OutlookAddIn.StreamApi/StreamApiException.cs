using System;
using System.Net;
using System.Text;
using AtTask.OutlookAddIn.Domain.Model;

namespace AtTask.OutlookAddIn.StreamApi
{
    /// <summary>
    /// Represents Stream API general exception.
    /// It may contain StreamApiError, which is the error response from @task server on REST API calls.
    /// As an inner exception it may have WebException in case of web connection errors.
    /// </summary>
    public class StreamApiException : Exception
    {
        public const string DataKeyProxyAddress = "ProxyAddress";
        public const string DataKeyRedirectLocation = "RedirectLocation";

        private HttpStatusCode? httpStatusCode;
        private StreamApiError error;

        /// <summary>
        /// Create StreamApiException instance with an inner exception.
        /// </summary>
        /// <param name="innerException"></param>
        public StreamApiException(Exception innerException) :
            base(string.Empty, innerException)
        {
        }

        /// <summary>
        /// Create StreamApiException instance with an message and HTTP status code.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="httpStatusCode"></param>
        public StreamApiException(HttpStatusCode httpStatusCode)
        {
            this.httpStatusCode = httpStatusCode;
        }

        /// <summary>
        /// Create StreamApiException instance with an inner exception and HTTP status code.
        /// </summary>
        /// <param name="httpStatusCode"></param>
        public StreamApiException(Exception innerException, HttpStatusCode httpStatusCode) :
            base(string.Empty, innerException)
        {
            this.httpStatusCode = httpStatusCode;
        }

        /// <summary>
        /// Create StreamApiException instance with WebException as inner exception and StreamApiError.
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="error"></param>
        public StreamApiException(WebException innerException, StreamApiError error) :
            this(innerException, error, string.Empty)
        {
        }

        /// <summary>
        /// Create StreamApiException instance with WebException as inner exception, StreamApiError and message.
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="error"></param>
        /// <param name="message"></param>
        public StreamApiException(WebException innerException, StreamApiError error, string message) :
            base(message, innerException)
        {
            this.error = error;
        }

        /// <summary>
        /// Returns StreamApiError instance.
        /// </summary>
        public StreamApiError Error { get { return error; } }

        /// <summary>
        /// Gets WebException if there is such inner exception, otherwise returns null.
        /// </summary>
        public WebException WebException { get { return InnerException as WebException; } }

        /// <summary>
        /// Returns HTTP status code of response.
        /// </summary>
        public HttpStatusCode? HttpStatusCode { get { return httpStatusCode; } }

        public override string ToString()
        {
            var builder = new StringBuilder();
            if (HttpStatusCode != null)
            {
                builder.Append("\tHttpStatusCode: ")
                    .Append(HttpStatusCode.Value)
                    .AppendFormat("({0}){1}", (int) HttpStatusCode.Value, Environment.NewLine);
            }

            if (Error != null)
            {
                builder.AppendLine("\tStream API Error:");
                builder.Append("\t\tTitle: ").AppendLine(error.Title);
                builder.Append("\t\tMsgKey: ").AppendLine(error.MsgKey);
                builder.Append("\t\tMessage: ").AppendLine(error.Message);
                builder.Append("\t\tClass: ").AppendLine(error.Class);
                if (error.Code.HasValue)
                {
                    builder.Append("\t\tCode: ").AppendLine(error.Code.Value.ToString());
                }
                if (error.Attributes != null && error.Attributes.Count != 0)
                {
                    builder.Append("\t\tAttributes: ");
                    for (int i = 0; i < error.Attributes.Count - 1; i++)
                    {
                        builder.Append(error.Attributes[i]).Append(", ");
                    }

                    builder.AppendLine(error.Attributes[error.Attributes.Count - 1]);
                }
            }

            return builder.ToString();
        }
    }
}