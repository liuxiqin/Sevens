using System;

namespace Seven.Infrastructure.Dependency
{
    public interface IDependencyResolver
    {
        T Resolve<T>();

        T Resolve<T>(Type serviceType);

        T Resolve<T>(string instanceName);

        object Resolve(Type serviceType);
    }
}