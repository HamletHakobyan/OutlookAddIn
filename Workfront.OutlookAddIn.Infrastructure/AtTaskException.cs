using System;
using System.Text;

namespace Workfront.OutlookAddIn.Infrastructure
{
    public enum AtTaskExceptionCode
    {
        /// <summary>
        /// Connection failed
        /// </summary>
        CONNECTION_FAILURE,

        /// <summary>
        /// The passed argument is null
        /// </summary>
        ARGUMENT_NULL,

        /// <summary>
        /// User was not logged in
        /// </summary>
        NOT_LOGGED_IN,

        /// <summary>
        /// TODO
        /// </summary>
        AUTHENTICATION_ERROR,

        /// <summary>
        /// Session is invalid
        /// </summary>
        INVALID_SESSION_ID,

        /// <summary>
        /// Client needs to be updated
        /// </summary>
        UPGRADE_NEEDED,

        /// <summary>
        /// IO error writing file
        /// </summary>
        FILE_WRITE_ERROR,

        /// <summary>
        /// No access to file or directory
        /// </summary>
        FILE_ACCESS_ERROR,

        /// <summary>
        /// AtTask parent folder (root folder) doesn't exist
        /// </summary>
        NO_ROOT_FOLDER,

        /// <summary>
        /// AtTask parent folder (root folder) wasn't specified
        /// </summary>
        NO_ROOT_FOLDER_NAME,

        /// <summary>
        /// String which was passed from WebView Control to Outlook was incorrect
        /// </summary>
        INVALID_METHOD_CALL,

        /// <summary>
        /// Some inconsistency exists in the menu/folders/command bar structure
        /// </summary>
        INVALID_MENU_STRUCTURE,

        /// <summary>
        /// When there are too many (more than 500) AtTask folders (i.e. AtTask, AtTask1, AtTask2, AtTask3, ...)
        /// </summary>
        TOO_MANY_ATTASK_FOLDERS
    }

    /// <summary>
    /// Exception to be used in the application. Every other exception should be used only as InnerException to this one.
    /// </summary>
    public class AtTaskException : Exception
    {
        /// <summary>
        /// Exception code
        /// </summary>
        public AtTaskExceptionCode? Code
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new _instance of the AtTaskException class.
        /// </summary>
        public AtTaskException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new _instance of the AtTaskException class with a specified error message.
        /// </summary>
        /// <param name="message"></param>
        public AtTaskException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new _instance of the AtTaskException class with a specified code.
        /// </summary>
        /// <param name="code"></param>
        public AtTaskException(AtTaskExceptionCode code)
            : this(null, code, null)
        {
        }

        /// <summary>
        /// Initializes a new _instance of the AtTaskException class with a specified inner exception.
        /// </summary>
        /// <param name="innerException"></param>
        public AtTaskException(Exception innerException)
            : base(null, innerException)
        {
        }

        /// <summary>
        /// Initializes a new _instance of the AtTaskException class with specified code and inner exception.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="innerException"></param>
        public AtTaskException(AtTaskExceptionCode code, Exception innerException)
            : this(null, code, innerException)
        {
        }

        /// <summary>
        /// Initializes a new _instance of the AtTaskException class with specified message, code and inner exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        /// <param name="innerException"></param>
        public AtTaskException(string message, AtTaskExceptionCode code, Exception innerException)
            : base(message, innerException)
        {
            Code = code;
        }

        protected virtual void ToStringInternal(StringBuilder builder)
        {
            if (Code != null)
            {
                builder.Append("\tCode: ").AppendLine(Code.Value.ToString());
            }

            builder.Append("\tException stack trace: ").AppendLine(base.ToString());
            if (InnerException != null)
            {
                builder.Append(InnerException);
            }
        }

        public sealed override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(GetType().Name).AppendLine(":");
            ToStringInternal(builder);
            return builder.ToString();
        }
    }
}