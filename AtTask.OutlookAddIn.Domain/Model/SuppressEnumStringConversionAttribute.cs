using System;

namespace AtTask.OutlookAddIn.Domain.Model
{
    /// <summary>
    /// Use this attribute with enums to tell StreamAPIEnumConverter to not convert the enum to string while serializing.
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum, AllowMultiple = false)]
    public sealed class SuppressEnumStringConversionAttribute : Attribute
    {
    }
}