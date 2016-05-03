using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Seven.Infrastructure.Dependency
{
    public interface IContainerBuilder
    {
        IObjectContainer Build();

        void RegisterType(Type type);

        void RegisterType<T>();

        void RegisterInstance<T>(T instance);

        void RegisterInterface<TImplement, TInterface>();

    }


    public class AutofacContainerBuilder : IContainerBuilder
    {
        private readonly ContainerBuilder _containerBuilder;

        private IContainer _container;

        public IObjectContainer Build()
        {
            var container = _containerBuilder.Build();

            return new AutofacContainer(container);
        }

        public void RegisterType(Type implementationType)
        {
            _containerBuilder.RegisterType(implementationType);
        }

        public void RegisterType<T>() { }

        public void RegisterInstance<T>(T instance)
        {

        }

        public void RegisterInterface<TImplement, TInterface>() { }


    }
}
