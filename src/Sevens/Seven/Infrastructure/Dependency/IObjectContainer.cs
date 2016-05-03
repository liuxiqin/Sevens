using System;

namespace Seven.Infrastructure.Dependency
{
    public interface IObjectContainer
    {
        T Resolve<T>();

        T Resolve<T>(Type serviceType);
        
        T Resolve<T>(string serviceName);

        object Resolve(Type serviceType);

    }
}