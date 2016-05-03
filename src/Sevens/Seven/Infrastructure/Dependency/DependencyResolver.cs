using System;

namespace Seven.Infrastructure.Dependency
{
    public class DependencyResolver
    {
        private static IDependencyResolver _dependencyResolver = null;

        public static void SetResolver(IDependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;
        }

        public static T Resolve<T>()
        {
            return _dependencyResolver.Resolve<T>();
        }

        public static T Resolve<T>(Type serviceType)
        {
            return _dependencyResolver.Resolve<T>(serviceType);
        }

        public static T Resolve<T>(string instanceName)
        {
            return _dependencyResolver.Resolve<T>(instanceName);
        }

        public static object Resolve(Type serviceType)
        {
            return _dependencyResolver.Resolve(serviceType);
        }
    }
}