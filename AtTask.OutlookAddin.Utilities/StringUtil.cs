using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace AtTask.OutlookAddIn.Utilities
{
    public static class StringUtil
    {
        private const Decimal OneKiloByte = 1024M;
        private const Decimal OneMegaByte = OneKiloByte * 1024M;
        private const Decimal OneGigaByte = OneMegaByte * 1024M;

        private static readonly Regex MultipleSpacesRegex = new Regex("[ ]{2,}", RegexOptions.Compiled);
        const string SolrEscapePattern = @"-|\+|&&|\|\||!|\(|\)|\{|\}|\[|\]|\^|""|\~|\*|\?|\:|\/";


        public static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static string LowercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToLower(s[0]) + s.Substring(1);
        }

        /// <summary>
        /// Joins elements with separator, which are not null or empty.
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static string JoinExcludingEmpties(List<string> elements, string separator)
        {
            int count = elements.Count;

            int i = 0;
            string notEmptyElem = null;
            while (i < count && string.IsNullOrEmpty(notEmptyElem = elements[i++]))
            {
                //just find first not empty or null
            }

            if (notEmptyElem == null)
            {
                return string.Empty;
            }

            StringBuilder result = new StringBuilder(notEmptyElem);

            while (i < count)
            {
                if (!string.IsNullOrEmpty(elements[i]))
                {
                    result.Append(separator).Append(elements[i]);
                }

                i++;
            }

            return result.ToString();
        }

        /// <summary>
        /// If the str length is more than maxChars returns the first maxChars characters of the string
        /// followed by '...'. Otherwise returns the string itself.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxChars"></param>
        /// <returns></returns>
        public static string ShortenString(string str, int maxChars)
        {
            if (str.Length > maxChars)
            {
                str = str.Substring(0, maxChars) + "...";
            }
            return str;
        }

        /// <summary>
        /// If the file name length is more than maxChars returns the first maxChars without extension lenght
        /// followed by '..' the extension. Otherwise returns the file name itself.
        /// </summary>
        /// <param name="extension"></param>
        /// <param name="maxChars"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ShortenFileName(string name, string extension, int maxChars)
        {
            if (!extension.StartsWith("."))
            {
                extension = "." + extension;
            }

            const string fullNameFormat = "{0}{1}";

            int fullLength = name.Length + extension.Length;
            if (fullLength <= maxChars)
            {
                return string.Format(fullNameFormat, name, extension);
            }

            int length = (maxChars < extension.Length + - 1) ? 0 : maxChars - extension.Length - 1;
            name = name.Substring(0, length / 2 + length % 2) + "\u2026" + name.Substring(name.Length - length / 2, length / 2);
            return string.Format(fullNameFormat, name, extension);
        }

        /// <summary>
        /// If the str length is more than maxChars returns the first maxChars characters of the string
        /// in the middle placed '...'. Otherwise returns the string itself.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxChars"></param>
        /// <returns></returns>
        public static string ShortenStringMiddle(string str, int maxChars)
        {
            if (str.Length > maxChars)
            {
                int halfMaxChars = maxChars / 2;

                str = str.Substring(0, halfMaxChars) + "\u2026" + str.Substring(str.Length - halfMaxChars);
            }
            return str;
        }

        /// <summary>
        /// Replaces multiple spaces in the string with one space.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceMultipleSpacesWithOneSpace(string str)
        {
            return MultipleSpacesRegex.Replace(str, " ");
        }

        public static string GetBytesString(decimal bytes, string format = null)
        {
            string suffix;
            if (bytes > OneGigaByte)
            {
                bytes /= OneGigaByte;
                suffix = "GB";
            }
            else if (bytes > OneMegaByte)
            {
                bytes /= OneMegaByte;
                suffix = "MB";
            }
            else if (bytes > OneKiloByte)
            {
                bytes /= OneKiloByte;
                suffix = "kB";
            }
            else
            {
                suffix = "B";
            }

            string precision = null;
            if (format != null)
            {
                precision = format.Substring(2);
            }

            if (string.IsNullOrEmpty(precision))
            {
                precision = "2";
            }

            return String.Format("{0:N" + precision + "} {1}", bytes, suffix);
        }

        public static string RemoveNewLineCharacter(string stringWithNewLine)
        {
            return Regex.Replace(stringWithNewLine, "(\\s)+", " ");
        }

        public static string CamelCaseName(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return propertyName;
            }

            //don't lowercase 1-letter strings or strings starting with two uppercase letters (e.g. IDs)
            if (!char.IsUpper(propertyName[0]) || propertyName.Length == 1 ||
                (propertyName.Length >= 2 && char.IsUpper(propertyName[1])))
            {
                return propertyName;
            }

            string camelCaseName =
                char.ToLower(propertyName[0], CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
            if (propertyName.Length > 1)
                camelCaseName += propertyName.Substring(1);

            return camelCaseName;
        }

        public static string EscapeSolrString(string queryString)
        {
            return Regex.Replace(queryString, SolrEscapePattern, " ");
        }
    }
}