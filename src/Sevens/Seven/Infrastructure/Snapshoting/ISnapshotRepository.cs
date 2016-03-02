using Seven.Aggregates;

namespace Seven.Infrastructure.Snapshoting
{
    /// <summary>
    /// 快照仓储
    /// </summary>
    public interface ISnapshotRepository
    {
        /// <summary>
        /// 获取快照
        /// </summary>
        /// <typeparam name="TAggregateRoot"></typeparam>
        /// <param name="aggregateRootId"></param>
        /// <returns></returns>
        Snapshot<TAggregateRoot> Get<TAggregateRoot>(string aggregateRootId) where TAggregateRoot : IAggregateRoot;

        /// <summary>
        /// 建立快照
        /// </summary>
        /// <typeparam name="TAggregateRoot"></typeparam>
        /// <param name="aggregateRoot"></param>
        void Build<TAggregateRoot>(TAggregateRoot aggregateRoot) where TAggregateRoot :  IAggregateRoot;
    }
}