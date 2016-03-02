using Seven.Aggregates;
using Seven.Infrastructure.Persistence;

namespace Seven.Infrastructure.Snapshoting
{
    public class SnapshotRepository : ISnapshotRepository
    {
        private readonly IPersistence _persistence;

        private readonly SnapshotFactory _snapshotFactory;
        public SnapshotRepository(IPersistence persistence, SnapshotFactory snapshotFactory)
        {
            _persistence = persistence;
            _snapshotFactory = snapshotFactory;
        }

        public Snapshot<TAggregateRoot> Get<TAggregateRoot>(string aggregateRootId)
            where TAggregateRoot : IAggregateRoot
        {
            var snapshot = _persistence.Get(new SnapshotSpecification(aggregateRootId));

            return _snapshotFactory.Create<TAggregateRoot>(snapshot);
        }

        /// <summary>
        /// 建立快照
        /// </summary>
        /// <typeparam name="TAggregateRoot"></typeparam>
        /// <param name="aggregateRoot"></param>
        public void Build<TAggregateRoot>(TAggregateRoot aggregateRoot) where TAggregateRoot : IAggregateRoot
        {
            var snapShot = new Snapshot<TAggregateRoot>(aggregateRoot);

            var entity = _snapshotFactory.Create(snapShot);

            _persistence.Save(entity);
        }
    }


}