using System;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Text;
using AtTask.OutlookAddIn.Assets;

namespace Workfront.OutlookAddIn.Infrastructure
{
    /// <summary>
    /// Logging level.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Level 0: Lowest level (write everything).
        /// Messages must be logged with this level.
        /// </summary>
        All,

        /// <summary>
        /// Level 1:
        /// </summary>
        Debug,

        /// <summary>
        /// Level 2:
        /// </summary>
        Info,

        /// <summary>
        /// Level 3:
        /// </summary>
        Warn,

        /// <summary>
        /// Level 4:
        /// </summary>
        Error,

        /// <summary>
        /// Level 5:
        /// </summary>
        Fatal,

        /// <summary>
        /// Level 6: Highest level (don't write anything)
        /// </summary>
        Off
    }

    /// <summary>
    /// Logger class for logging messages or exceptions. For every new day logger creates
    /// a new file in the rootDirectory, given by constructor. Logger has level and if the
    /// level is higher than the message's (or exception's) level the logging wouldn't occur.
    /// Message or exception logging couldn't be provided with LogLevel.Off level (highest level)
    /// otherwise exception will be thrown. This status is reserved only for creating logger, which
    /// wouldn't log anything.
    /// </summary>
    /// <exception cref="ArgumentException">If messageLevel is LogLevel.Off</exception>
    public class Logger
    {
        public static string GetLoggerPath()
        {
            try
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Settings.Default.AppDataFolder);
            }
            catch
            {
                return string.Empty;
            }
        }

        //private static Logger _instance = new Logger(Path.Combine(CommonContext.Instance.LoggerPath, "Logs"), LogLevel.All);
        private static Logger _instance = new Logger(Path.Combine(GetLoggerPath(), "Logs"), LogLevel.All);

        private string rootDirectory;

        /// <summary>
        /// Gets or sets whether the logger was inited successfully.
        /// </summary>
        private bool IsInitied { get; set; }

        /// <summary>
        /// Gets or sets the level of logging. The higher it is, less messages would be actually logged.
        /// </summary>
        private LogLevel Level { get; set; }

        /// <summary>
        /// Creates an _instance of logger. If rootDirectory doesn't exist tries to create it.
        /// If everything is successfull sets IsInited to true.
        /// </summary>
        /// <param name="rootDirectory">The directory, where logs should be created.</param>
        /// <param name="level">Level of loggin. Pass LogLevel.Off for turning off logging.</param>
        /// <exception cref="AtTaskException">
        ///     Code: FILE_ACCESS_ERROR: when the rootDirectory doesn't exist and couldn't be created or
        ///         when there is no permosision to write files in that directory
        /// </exception>
        private Logger(string rootDirectory, LogLevel level)
        {
            try
            {
                if (string.IsNullOrEmpty(rootDirectory))
                {
                    rootDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");

                    //throw new ArgumentException("rootDirectory is null or empty");
                    return;
                }

                if (!Directory.Exists(rootDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(rootDirectory);
                    }
                    catch (IOException)
                    {
                        //throw new AtTaskException(AtTaskExceptionCode.FILE_ACCESS_ERROR, ex);
                        return;
                    }
                }

                FileIOPermission directoryPermission = new FileIOPermission(FileIOPermissionAccess.Append, rootDirectory);

                PermissionSet permissionSet = new PermissionSet(PermissionState.None);
                permissionSet.AddPermission(directoryPermission);
                if (!permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet))
                {
                    //throw new AtTaskException(AtTaskExceptionCode.FILE_ACCESS_ERROR);
                    return;
                }

                this.rootDirectory = rootDirectory;
                Level = level;

                IsInitied = true;
            }
            catch
            {
                //DO NOTHING
            }
        }

        /// <summary>
        /// Returns path of the log file for given day.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string GetLogPathByDay(DateTime day)
        {
            lock (_instance)
            {
                if (_instance.IsInitied)
                {
                    return Path.Combine(_instance.rootDirectory, DateTime.Today.ToFileTime() + ".log");
                }

                return null;
            }
        }

        /// <summary>
        /// Logs message with date time and caption in the log file for current day.
        /// If there is no such a log file creates one. If messageLevel is lower than the logger's
        /// doesn't do anything. messageLevel can't be LogLevel.Off.
        /// </summary>
        /// <param name="caption">Is appended before message.</param>
        /// <param name="message">Message to be logged.</param>
        /// <param name="messageLevel">Level of logging. Can't be LogLevel.Off.</param>
        /// <exception cref="ArgumentException">If messageLevel is LogLevel.Off</exception>
        public static void Log(string caption, string message, LogLevel messageLevel)
        {
            lock (_instance)
            {
                if (messageLevel == LogLevel.Off)
                {
                    throw new ArgumentException("Message level cannot be LogLevel.Off");
                }

                if (!_instance.IsInitied || messageLevel < _instance.Level)
                {
                    return;
                }

                string filePath = Path.Combine(_instance.rootDirectory, DateTime.Today.ToFileTime() + ".log");

                FileStream fileStream = null;
                StreamWriter streamWriter = null;
                using (fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.None))
                {
                    StringBuilder text = new StringBuilder();
                    text.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")).Append(": <").Append(messageLevel).Append(">: ");
                    text.Append(caption).Append(": ").Append(message);

                    streamWriter = new StreamWriter(fileStream);
                    streamWriter.WriteLine(text.ToString());
                    streamWriter.Flush();
                }
            }
        }

        /// <summary>
        /// Logs message of exception contents with date time and caption in the log file for current day.
        /// If there is no such a log file creates one. Handles AtTaskException cases specifically.
        /// If messageLevel is lower than the logger's does nothing. messageLevel can't be LogLevel.Off.
        /// </summary>
        /// <param name="caption">Is appended before message.</param>
        /// <param name="exception"></param>
        /// <param name="messageLevel">Level of logging. Can't be LogLevel.Off.</param>
        public static void Log(string caption, Exception exception, LogLevel messageLevel)
        {
            Log(caption, exception.ToString(), messageLevel);
        }
    }
}