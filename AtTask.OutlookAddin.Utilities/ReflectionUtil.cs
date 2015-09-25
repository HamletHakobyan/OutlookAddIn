using System;
using System.Linq;
using System.Reflection;

namespace AtTask.OutlookAddIn.Utilities
{
    public static class ReflectionUtil
    {
        /// <summary>
        /// Casts and returns value of a property by given name for given object. If the property exists and is of given type returns true, otherwise false.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryGetPropertyValue<T>(object obj, string propertyName, out T value)
        {
            Type t = obj.GetType();
            PropertyInfo property = t.GetProperty(propertyName);
            if (property != null)
            {
                object result = property.GetValue(obj, null);
                if (result is T)
                {
                    value = (T)result;
                    return true;
                }
            }

            value = default(T);
            return false;
        }

        public static string GetConstValue(Type type, string name)
        {
            return type.GetFields(BindingFlags.Public | BindingFlags.Static)
                .First(i => i.Name == name)
                .GetRawConstantValue()
                .ToString();
        }
    }
}