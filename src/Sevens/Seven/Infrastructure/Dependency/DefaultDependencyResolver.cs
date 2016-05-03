using System;

namespace Seven.Infrastructure.Dependency
{
    public class DefaultDependencyResolver : IDependencyResolver
    {
        private readonly IObjectContainer _container;

        public DefaultDependencyResolver(IObjectContainer container)
        {
            _container = container;
        }

        public DefaultDependencyResolver(IContainerBuilder containerBuilder)
        {
            _container = containerBuilder.Build();
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public T Resolve<T>(Type serviceType)
        {
            return _container.Resolve<T>(serviceType);
        }

        public T Resolve<T>(string instanceName)
        {
            return _container.Resolve<T>(instanceName);
        }

        public object Resolve(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }
    }
}