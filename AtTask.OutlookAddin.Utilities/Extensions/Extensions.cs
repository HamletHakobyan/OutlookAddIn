using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace AtTask.OutlookAddin.Utilities.Extensions
{
    public static class Extensions
    {
        public static string GetIssueDescription<T>(this T @enum) where T : struct, IConvertible
        {
            return GetDescription(@enum, typeof (IssueDescriptionAttribute));
        }

        public static string GetP2PDescription<T>(this T @enum) where T : struct, IConvertible
        {
            return GetDescription(@enum, typeof(P2PDescriptionAttribute));
        }

        public static string GetPropertyName<T>(this T @enum) where T : struct, IConvertible
        {
            var attribute = GetIssuePropertyAttribute(@enum);
            if (attribute == null)
            {
                return null;
            }

            return attribute.Name;
        }

        public static Type GetPropertyType<T>(this T @enum) where T : struct, IConvertible
        {
            var attribute = GetIssuePropertyAttribute(@enum);
            if (attribute == null)
            {
                return null;
            }

            return attribute.Type;
        }

        private static IssuePropertyAttribute GetIssuePropertyAttribute<T>(T @enum) where T : struct, IConvertible
        {
            Type type = typeof(T);
            if (!type.IsEnum)
            {
                throw new ArgumentException("T must be an enum.");
            }

            MemberInfo member = type.GetMember(@enum.ToString(CultureInfo.InvariantCulture)).FirstOrDefault();
            if (member == null)
            {
                return null;
            }

            return member.GetCustomAttributes(typeof(IssuePropertyAttribute), false).FirstOrDefault() as IssuePropertyAttribute;
        }

        public static string GetDescription<T>(this T @enum, Type attributeType) where T : struct, IConvertible
        {
            Type type = typeof(T);
            if (!type.IsEnum)
            {
                throw new ArgumentException("T must be an enum.");
            }

            MemberInfo member = type.GetMember(@enum.ToString(CultureInfo.InvariantCulture)).FirstOrDefault();
            if (member == null)
            {
                return string.Empty;
            }

            var attribute = member.GetCustomAttributes(attributeType, false).FirstOrDefault() as DescriptionAttribute;
            if (attribute == null)
            {
                return string.Empty;
            }

            return attribute.Description;
        }


        public static bool Contains(this string source, string part, StringComparison comparison)
        {
            if (string.IsNullOrWhiteSpace(part))
            {
                return true;
            }

            return source.IndexOf(part, comparison) >= 0;
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
        {
            return new ObservableCollection<T>(enumerable);
        }
    }
}
