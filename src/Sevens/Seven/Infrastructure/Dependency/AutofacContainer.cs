using System;
using Autofac;

namespace Seven.Infrastructure.Dependency
{
    public class AutofacContainer : IObjectContainer
    {
        private readonly IContainer _container;

        public AutofacContainer(IContainer container)
        {
            _container = container;
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public T Resolve<T>(Type serviceType)
        {
            return (T)_container.Resolve(serviceType);
        }

        public T Resolve<T>(string serviceName)
        {
            return _container.ResolveNamed<T>(serviceName);
        }

        public object Resolve(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }
    }
}