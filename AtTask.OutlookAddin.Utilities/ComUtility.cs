using System;
using System.Runtime.InteropServices;

namespace AtTask.OutlookAddIn.Utilities
{
    public static class ComUtility
    {
        /// <summary>
        /// E_ABORT
        /// </summary>
        private const int HResultOperationAborted = -2147467260;

        public static void ReleaseObject(object obj)
        {
            if (obj != null)
            {
                try
                {
                    Marshal.FinalReleaseComObject(obj);
                }
                catch (System.Exception)
                {
                    //do nothing
                }
            }
        }

        public static void SafeReleaseObject(object comObject)
        {
            try
            {
                if (comObject != null)
                {
                    int count = Marshal.ReleaseComObject(comObject);
                    //Debug.WriteLine(count);
                }
            }
            catch (Exception ex)
            {
            }
        }


        /// <summary>
        /// Returns whether or not the exception is COMException with HResult = E_ABORT.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static bool IsOperationAbortedError(Exception exception)
        {
            COMException comEx = exception as COMException;
            return comEx != null &&
                comEx.ErrorCode == HResultOperationAborted;
        }
    }
}