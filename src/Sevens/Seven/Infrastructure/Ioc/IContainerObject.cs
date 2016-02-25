using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;

namespace Seven.Infrastructure.Ioc
{
    public interface IContainerObject
    {
        void RegisterType(Type type);

        void RegisterType<T>();

        void RegisterInstance<T>(T instance) where T : class;

        void RegisterInterface<TImplement, TInterface>();

        T Resolve<T>();

        T Resolve<T>(Type serviceType);
    }


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

    public class ObjectContainer
    {
        private static IContainerObject _containerObject;


        public static void SetContainer(IContainerObject containerObject)
        {
            _containerObject = containerObject;
        }

        public static void RegisterInstance<T>(T instance) where T : class
        {
            _containerObject.RegisterInstance(instance);
        }

        public static T Resolve<T>() where T : class
        {
            return _containerObject.Resolve<T>();
        }
    }
}
