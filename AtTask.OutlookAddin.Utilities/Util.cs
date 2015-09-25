using System;

namespace AtTask.OutlookAddIn.Utilities
{
    public static class Util
    {
        /// <summary>
        /// Returns true if nullable bool has a value and it's true.
        /// </summary>
        /// <param name="nullableBool"></param>
        /// <returns></returns>
        public static bool IsNullableTrue(bool? nullableBool)
        {
            return nullableBool.HasValue && nullableBool.Value;
        }        
    }
}