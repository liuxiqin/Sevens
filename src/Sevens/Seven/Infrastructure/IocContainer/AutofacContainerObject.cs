using System;
using Autofac;

namespace Seven.Infrastructure.IocContainer
{
    public class AutofacContainerObject : IContainerObject
    {
        private readonly ContainerBuilder _containerBuilder;

        private IContainer _container;

        private object _lockObj = new object();

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
            ContainerBuild();

            return _container.Resolve<T>();
        }

        public T Resolve<T>(Type serviceType)
        {
            ContainerBuild();

            return (T)_container.Resolve(serviceType);
        }

        private void ContainerBuild()
        {
            if (_container == null)
            {
                lock (_lockObj)
                {
                    if (_container == null)
                    {
                        _container = _containerBuilder.Build();
                    }
                }
            }
        }
    }
}