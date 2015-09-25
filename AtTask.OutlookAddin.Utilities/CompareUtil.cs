using System;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using Microsoft.Win32;

namespace AtTask.OutlookAddIn.Utilities
{
    public static class CompareUtil
    {
        public static int CompareNullableIntegers(int? x, int? y)
        {
            if (!x.HasValue)
            {
                if (!y.HasValue)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (!y.HasValue)
                {
                    return 1;
                }
                else
                {
                    return x.Value.CompareTo(y.Value);
                }
            }
        }

        public static int CompareNullableDateTimes(DateTime? x, DateTime? y)
        {
            if (!x.HasValue)
            {
                if (!y.HasValue)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (!y.HasValue)
                {
                    return 1;
                }
                else
                {
                    return x.Value.CompareTo(y.Value);
                }
            }
        }

        public static int CompareStrings(string x, string y)
        {
            int result;
            if (!CompareObjects(x, y, out result))
            {
                result = x.CompareTo(y);
            }

            return result;
        }

        public static bool CompareObjects(object x, object y, out int result)
        {
            if (x == y)
            {
                result = 0;
            }

            if (x == null)
            {
                if (y == null)
                {
                    result = 0;
                }
                else
                {
                    result = -1;
                }
            }
            else
            {
                if (y == null)
                {
                    result = 1;
                }
                else
                {
                    //this is the case when the result depends on object specific data
                    result = 0;
                    return false;
                }
            }

            //this is the case when we can compare objects regardless of their specific data
            return true;
        }
    }
}