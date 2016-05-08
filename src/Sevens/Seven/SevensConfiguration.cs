using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Infrastructure.Dependency;
using Seven.Infrastructure.EventStore;
using Seven.Infrastructure.Repository;
using Seven.Infrastructure.Serializer;
using Seven.Infrastructure.Snapshoting;

namespace Seven
{
    public class SevensConfiguration
    {
        private static IContainerBuilder _containerBuilder;

        //初始化默认值

        static SevensConfiguration()
        {
            _containerBuilder = new AutofacContainerBuilder();

            Initialize();
        }

        public static void Initialize()
        {
            if (_containerBuilder == null)
                throw new ArgumentException("_containerBuilder can not be null.");

            UseBinarySerializer<DefaultBinarySerializer>();
            UseRepository<EventSouringRepository>();
            UseEventStore<MySqlEventStore>();
            UseSnapshotStorage<MysqlSnapshotStorage>();

            DependencyResolver.SetResolver(new DefaultDependencyResolver(_containerBuilder));
        }

        public static void SetContainerBuilder(IContainerBuilder containerBuilder)
        {
            _containerBuilder = containerBuilder;
        }

        public static void UseBinarySerializer<TBinarySerializer>() where TBinarySerializer : IBinarySerializer
        {
            _containerBuilder.RegisterInterface<TBinarySerializer, IBinarySerializer>();
        }

        public static void UseRepository<TRepository>() where TRepository : IRepository
        {
            _containerBuilder.RegisterInterface<TRepository, IRepository>();
        }

        public static void UseEventStore<TEventStore>() where TEventStore : IEventStore
        {
            _containerBuilder.RegisterInterface<TEventStore, IEventStore>();
        }

        public static void UseSnapshotStorage<TSnapshotStorage>() where TSnapshotStorage : ISnapshotStorage
        {
            _containerBuilder.RegisterInterface<TSnapshotStorage, ISnapshotStorage>();
        }

    }
}
