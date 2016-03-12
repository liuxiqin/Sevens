using Seven.Aggregates;
using Seven.Infrastructure.Serializer;

namespace Seven.Infrastructure.Snapshoting
{
    public class SnapshotFactory
    {
        private readonly IBinarySerializer _binarySerializer;

        public SnapshotFactory(IBinarySerializer binarySerializer)
        {
            _binarySerializer = binarySerializer;
        }

        public SnapshotEntity Create<TAggregateRoot>(Snapshot<TAggregateRoot> aggregateRoot)
            where TAggregateRoot : IAggregateRoot
        {
            var datas = _binarySerializer.Serialize(aggregateRoot);

            return new SnapshotEntity(aggregateRoot.AggregateRootId, aggregateRoot.Version, datas);
        }

        public Snapshot<TAggregateRoot> Create<TAggregateRoot>(SnapshotEntity snapshot)
            where TAggregateRoot : IAggregateRoot
        {
            var aggregateRoot = (TAggregateRoot)_binarySerializer.Deserialize<IAggregateRoot>(snapshot.Datas);

            return new Snapshot<TAggregateRoot>(aggregateRoot);
        }
    }
}