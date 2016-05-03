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
            return default(T);
        }

        public T Resolve<T>(Type serviceType) { return default(T); }

        public T Resolve<T>(string instanceName) { return default(T); }

        public object Resolve(Type serviceType) { return default(object); }
    }
}