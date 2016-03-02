using Seven.Aggregates;

namespace Seven.Infrastructure.Snapshoting
{
    public class SnapshotSpecification : ISpecification<SnapshotEntity>
    {
        private readonly string _aggregateRootId;

        public SnapshotSpecification(string aggregateRootId)
        {
            _aggregateRootId = aggregateRootId;

        }

        public bool IsSatisfiedBy(SnapshotEntity snapshot)
        {
            return snapshot.AggregateRootId.Equals(_aggregateRootId);
        }
    }
}