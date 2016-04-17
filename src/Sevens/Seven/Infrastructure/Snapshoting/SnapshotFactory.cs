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

        public SnapshotRecord Create(IAggregateRoot aggregateRoot)
        {
            var datas = _binarySerializer.Serialize(aggregateRoot);

            return new SnapshotRecord(aggregateRoot.AggregateRootId, aggregateRoot.Version, datas);
        }

        public Snapshot Create(SnapshotRecord snapshot)
        {
            var aggregateRoot = _binarySerializer.Deserialize<IAggregateRoot>(snapshot.Datas);

            return new Snapshot(aggregateRoot);
        }
    }
}