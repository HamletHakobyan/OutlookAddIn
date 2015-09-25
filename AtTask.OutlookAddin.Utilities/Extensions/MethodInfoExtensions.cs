using System;
using System.Linq;
using System.Reflection;

namespace AtTask.OutlookAddIn.Utilities.Extensions
{
    public static class MethodInfoExtensions
    {
        public static MethodInfo GetGenericMethod(this Type t, string name, Type[] genericArgTypes, Type[] argTypes, Type returnType)
        {
            //MethodInfo method = (from m in t.GetMethods(BindingFlags.Public | BindingFlags.Static)
            //                   where m.Name == name &&
            //                   m.GetGenericArguments().Length == genericArgTypes.Length &&
            //                   m.GetParameters().Select(pi => pi.ParameterType).SequenceEqual(argTypes) &&
            //                   m.ReturnType == returnType
            //                   select m).Single().MakeGenericMethod(genericArgTypes);

            //return method;

            MethodInfo foo1 = (from m in t.GetMethods(BindingFlags.Public | BindingFlags.Static)
                               where m.Name == name
                               && m.GetGenericArguments().Length == genericArgTypes.Length
                               && m.GetParameters().Select(pi => pi.ParameterType.IsGenericType ? pi.ParameterType.GetGenericTypeDefinition() : pi.ParameterType).SequenceEqual(argTypes) &&
                               (returnType == null || (m.ReturnType.IsGenericType ? m.ReturnType.GetGenericTypeDefinition() : m.ReturnType) == returnType)
                               select m).FirstOrDefault();
            if (foo1 != null)
            {
                return foo1.MakeGenericMethod(genericArgTypes);
            }
            return null;
        }
    }
}