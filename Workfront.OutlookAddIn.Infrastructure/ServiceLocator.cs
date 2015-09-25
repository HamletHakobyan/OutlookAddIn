using System;
using System.Collections.Generic;

namespace Workfront.OutlookAddIn.Infrastructure
{
    public class ServiceLocator : IServiceLocator
    {
        private ServiceLocator()
        {}
        static ServiceLocator()
        {}
        private static readonly ServiceLocator _default = new ServiceLocator();

        public static ServiceLocator Default
        {
            get { return _default; }
        }

        private readonly Dictionary<Type, Type> _interfaceClassMapper = new Dictionary<Type, Type>();

        public void Register(Type @interface, Type @class)
        {
            if (@interface == null)
            {
                throw new ArgumentNullException("interface");
            }

            if (@class == null)
            {
                throw new ArgumentNullException("class");
            }

            if (!@interface.IsInterface)
            {
                throw new ArgumentException("First argument must be of an interface type");
            }

            _interfaceClassMapper[@interface] = @class;
        }

        public bool Unregister(Type @interface)
        {
            if (@interface == null)
            {
                throw new ArgumentNullException("interface");
            }

            if (!@interface.IsInterface)
            {
                throw new ArgumentException("The argument must be of an interface type");
            }

            return _interfaceClassMapper.Remove(@interface);
        }

        public T Resolve<T>()
        {
            Type @class;
            if(_interfaceClassMapper.TryGetValue(typeof(T), out @class))
            {
                return (T)Activator.CreateInstance(@class);
            }

            throw new KeyNotFoundException("Provided interface type does not registered.");
        }
    }
}