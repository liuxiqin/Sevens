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
        /// <param name="aggregateRootId"></param>
        /// <returns></returns>
        Snapshot Get(string aggregateRootId);

        /// <summary>
        /// 建立快照
        /// </summary>
        /// <param name="aggregateRoot"></param>
        void Create(IAggregateRoot aggregateRoot);
    }
}