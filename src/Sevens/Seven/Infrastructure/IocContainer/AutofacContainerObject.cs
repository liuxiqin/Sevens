using System;
using Autofac;

namespace Seven.Infrastructure.IocContainer
{
    public class AutofacContainerObject : IContainerObject
    {
        private readonly ContainerBuilder _containerBuilder;

        private IContainer _container;

        public AutofacContainerObject()
        {
            _containerBuilder = new ContainerBuilder();
        }

        public void RegisterType(Type type)
        {
            _containerBuilder.RegisterType(type);
        }

        public void RegisterType<T>()
        {
            _containerBuilder.RegisterType<T>();
        }

        public void RegisterInstance<T>(T instance) where T : class
        {
            _containerBuilder.RegisterInstance(instance);
        }

        public void RegisterInterface<TInterface, TImplement>()
        {
            _containerBuilder.RegisterType<TImplement>().As<TInterface>();
        }

        public T Resolve<T>()
        {
            if (_container == null)
            {
                _container = _containerBuilder.Build();
            }

            return _container.Resolve<T>();
        }

        public T Resolve<T>(Type serviceType)
        {
            if (_container == null)
            {
                _container = _containerBuilder.Build();
            }
            return (T)_container.Resolve(serviceType);
        }
    }
}