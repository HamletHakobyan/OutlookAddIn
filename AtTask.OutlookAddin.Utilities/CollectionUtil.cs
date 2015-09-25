using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtTask.OutlookAddIn.Utilities
{
    public static class CollectionUtil
    {
        /// <summary>
        /// Returns the next item of the given item in the list, or previous one if the item is the last one.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static T GetNearestItem<T>(IList<T> list, T item)
        {
            if (list.Count == 0 || list.Count == 1)
            {
                return default(T);
            }

            int i = list.IndexOf(item);
            if (i == list.Count - 1)
            {
                i--;
            }
            else
            {
                i++;
            }

            return list[i];
        }
    }
}
