using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AtTask.OutlookAddIn.Utilities
{
    public static class EnumUtil
    {
        /// <summary>
        /// Returns enum value if given value can be converted to the enum, otherwise returns null.
        /// </summary>
        /// </summary>
        /// <typeparam name="T">Must be enum.</typeparam>
        /// <param name="value">Nullable integer.</param>
        /// <returns></returns>
        public static T? GetEnum<T>(int? value) where T : struct, IConvertible
        {
            if (typeof(T).IsEnum && value.HasValue && Enum.IsDefined(typeof(T), value.Value))
            {
                return (T)Enum.ToObject(typeof(T), value.Value);
            }

            return null;
        }

        /// <summary>
        /// Returns enum value if there is an enum value by equal to given string, otherwise returns null.
        /// </summary>
        /// <typeparam name="T">Must be enum.</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T? GetEnumFromString<T>(string value) where T : struct, IConvertible
        {
            if (typeof(T).IsEnum && value != null)
            {
                Array enumValues = Enum.GetValues(typeof(T));
                foreach (T enumValue in enumValues)
                {
                    if (enumValue.ToString() == value)
                    {
                        return enumValue;
                    }
                }
            }

            return null;
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            
            return value.ToString();
        }

    }
}
