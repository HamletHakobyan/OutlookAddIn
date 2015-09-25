using System;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using Microsoft.Win32;

namespace AtTask.OutlookAddIn.Utilities
{
    /// <summary>
    /// Provides utility functionality for I/O.
    /// </summary>
    public static class IOUtil
    {
        /// <summary>
        /// Closes the stream safely if it's not null.
        /// </summary>
        /// <param name="stream"></param>
        public static void CloseStream(Stream stream)
        {
            if (stream != null)
            {
                try
                {
                    stream.Close();
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// Creates directory with given path.
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="AtTaskException">If there was an exception while creating directory.</exception>
        public static void CreateDirectory(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);

            if (!directory.Exists)
            {
                directory.Create();
            }
        }

        /// <summary>
        /// Returns temporary file name, based on given name and extension.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileExt"></param>
        /// <returns></returns>
        public static string GetTempFileName(string attachmentsPath, string fileName, string fileExt)
        {
            StringBuilder newName = new StringBuilder(fileName).Append('_').Append(Guid.NewGuid().ToString("N"));
            if (!string.IsNullOrEmpty(fileExt))
            {
                newName.Append('.').Append(fileExt);
            }

            return Path.Combine(attachmentsPath, newName.ToString());
        }

        /// <summary>
        /// Returns true if the file with such path exists and is not in use.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool IsReadyForUpload(string filePath)
        {
            FileInfo file = new FileInfo(filePath);

            FileStream stream = null;
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //  still being written to
                //  or being processed by another thread
                //  or does not exist (has already been processed)
                return false;
            }
            finally
            {
                CloseStream(stream);
            }

            //file is not locked
            return true;
        }

        /// <summary>
        /// Returns valid file name for current environment (replaces invalid chars with given argument).
        /// </summary>
        /// <param name="title"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string GetValidFileName(string title, string replacement)
        {
            string validFileName = title.Trim();

            char[] invalidChars = Path.GetInvalidFileNameChars();
            foreach (char invalidChar in invalidChars)
            {
                validFileName = validFileName.Replace(invalidChar.ToString(), replacement);
            }

            invalidChars = Path.GetInvalidPathChars();
            foreach (char invalidChar in invalidChars)
            {
                validFileName = validFileName.Replace(invalidChar.ToString(), replacement);
            }

            return validFileName;
        }

        #region Directory cleanup

        /// <summary>
        /// Deletes asynchronously all files and folders in given folder.
        /// If an error occurs during some item deletion skips it.
        /// </summary>
        /// <param name="folderPath"></param>
        public static void DeleteFolderContentsAsync(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath))
            {
                return;
            }

            Action<string> action = new Action<string>(DeleteFolderContents);
            action.BeginInvoke(folderPath, DeleteFolderContentsCallback, null);
        }

        /// <summary>
        /// Deletes all files and folders in given folder.
        /// If an error occurs during some item deletion skips it.
        /// </summary>
        /// <param name="folderPath"></param>
        public static void DeleteFolderContents(string folderPath)
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(folderPath);
                if (!directory.Exists)
                {
                    return;
                }

                //get sub-items first, to delete only those ones, which were existing during call
                DirectoryInfo[] subDirs = directory.GetDirectories();
                FileInfo[] files = directory.GetFiles();

                foreach (DirectoryInfo subDir in subDirs)
                {
                    subDir.Delete(true);
                }

                foreach (FileInfo file in files)
                {
                    file.Delete();
                }
            }
            catch (Exception)
            {
                //do nothing as this is not important
            }
        }

        /// <summary>
        /// Deletes all files and folders in given folder.
        /// If an error occurs during some item deletion skips it.
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="exclude"></param>
        public static void DeleteFolderContents(string folderPath, string[] excludedFolders, string[] excludedFiles)
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(folderPath);
                if (!directory.Exists)
                {
                    return;
                }

                //get sub-items first, to delete only those ones, which were existing during call
                DirectoryInfo[] subDirs = directory.GetDirectories();
                FileInfo[] files = directory.GetFiles();

                foreach (DirectoryInfo subDir in subDirs)
                {
                    if (!excludedFolders.Contains<string>(subDir.Name))
                    {
                        subDir.Delete(true);
                    }
                }

                foreach (FileInfo file in files)
                {
                    if (!excludedFiles.Contains<string>(file.Name))
                    {
                        file.Delete();
                    }
                }
            }
            catch (Exception)
            {
                //do nothing as this is not important
            }
        }

        private static void DeleteFolderContentsCallback(IAsyncResult asyncResult)
        {
            Action<string> action = (Action<string>)((AsyncResult)asyncResult).AsyncDelegate;
            try
            {
                action.EndInvoke(asyncResult);
            }
            catch (Exception)
            {
                //do nothing as this is not important
            }
        }

        #endregion Directory cleanup

        public static string GetMimeType(FileInfo fileInfo)
        {
            string mimeType = "application/unknown";

            RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(fileInfo.Extension.ToLower());

            if (regKey != null)
            {
                object contentType = regKey.GetValue("Content Type");

                if (contentType != null)
                    mimeType = contentType.ToString();
            }

            return mimeType;
        }

        public static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            int pos = fileName.LastIndexOf('.');
            if (pos != -1)
            {
                string ext = fileName.Substring(pos);
                RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(ext.ToLower());
                if (regKey != null)
                {
                    object contentType = regKey.GetValue("Content Type");
                    if (contentType != null)
                        mimeType = contentType.ToString();
                }
            }
            return mimeType;
        }

        public static string ReadFileAsString(FileInfo fileInfo)
        {
            string ret = "";
            byte[] buffer;
            FileStream fileStream = null;
            try
            {
                fileStream = fileInfo.OpenRead();
                int length = (int)fileStream.Length;
                buffer = new byte[length];
                int count;
                int sum = 0;

                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                {
                    sum += count;
                }
                ret = Encoding.UTF8.GetString(buffer);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }

            return ret;
        }

        public static byte[] ReadFileAsByteArray(FileInfo fileInfo)
        {
            if (fileInfo.Length > int.MaxValue)
            {
                throw new Exception("File is too big.");
            }

            byte[] buffer;
            FileStream fileStream = null;
            try
            {
                fileStream = fileInfo.OpenRead();
                int length = (int)fileStream.Length;
                buffer = new byte[length];
                int count;
                int sum = 0;

                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                {
                    sum += count;
                }
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
            return buffer;
        }
    }
}