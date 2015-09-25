using System;

namespace Workfront.OutlookAddIn.Infrastructure
{
    public interface IServiceLocator
    {
        void Register(Type @interface, Type @class);
        T Resolve<T>();
    }
}