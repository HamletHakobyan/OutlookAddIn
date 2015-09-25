using System;
using System.Diagnostics.Contracts;

namespace AtTask.OutlookAddin.Utilities
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false)]
    public class IssuePropertyAttribute : Attribute
    {
        public string Name { get; private set; }
        public Type Type { get; private set; }

        public IssuePropertyAttribute(string name, Type type)
        {
            Contract.Requires(type != null);
            Contract.Requires(string.IsNullOrWhiteSpace(name));
            Name = name;
            Type = type;
        }
    }
}
