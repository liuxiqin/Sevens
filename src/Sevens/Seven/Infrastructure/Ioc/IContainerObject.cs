using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
}
