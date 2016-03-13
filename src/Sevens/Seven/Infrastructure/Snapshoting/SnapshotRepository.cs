using Seven.Aggregates;
using Seven.Infrastructure.Persistence;

namespace Seven.Infrastructure.Snapshoting
{
    public class SnapshotRepository : ISnapshotRepository
    {
        private readonly IPersistence<SnapshotEntity> _persistence;

        private readonly SnapshotFactory _snapshotFactory;
        public SnapshotRepository(IPersistence<SnapshotEntity> persistence, SnapshotFactory snapshotFactory)
        {
            _persistence = persistence;
            _snapshotFactory = snapshotFactory;
        }

        public Snapshot Get(string aggregateRootId)
        {
            var snapshot = _persistence.GetById(aggregateRootId);

            return _snapshotFactory.Create(snapshot);
        }

        /// <summary>
        /// 建立快照
        /// </summary>
        /// <param name="aggregateRoot"></param>
        public void Create(IAggregateRoot aggregateRoot) 
        {
            var snapShot = new Snapshot(aggregateRoot);

            var entity = _snapshotFactory.Create(aggregateRoot);

            _persistence.Save(entity);
        }
    }


}