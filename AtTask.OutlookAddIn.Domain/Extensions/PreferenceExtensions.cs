using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AtTask.OutlookAddIn.Domain.Model;

namespace AtTask.OutlookAddIn.Domain.Extensions
{
    public static class PreferenceExtensions
    {
        private const char PreferenceValueDelimiter = '\t';
        private static readonly string[] EmptyStringArray = new string[0];

        public static string[] GetValues(this Preference pref)
        {
            if (pref.Value != null)
            {
                return pref.Value.Split(PreferenceValueDelimiter);
            }

            return EmptyStringArray;
        }
    }
}
