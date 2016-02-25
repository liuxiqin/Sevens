using System.Collections.Generic;
using Seven.Aggregates;

namespace Seven.Infrastructure.Repository
{
    public class MemoryCacheRepository : IRepository
    {
        private IDictionary<string, IAggregateRoot> _aggregateRoots;

        public MemoryCacheRepository(IDictionary<string, IAggregateRoot> aggregateRoots)
        {
            _aggregateRoots = aggregateRoots;
        }

        public void Add(IAggregateRoot aggregateRoot)
        {
            if (!_aggregateRoots.ContainsKey(aggregateRoot.AggregateRootId))
            {
                _aggregateRoots.Add(aggregateRoot.AggregateRootId, aggregateRoot);
            }
        }

        public T Get<T>(object aggregateRootId) where T : IAggregateRoot
        {
            if (_aggregateRoots.ContainsKey(aggregateRootId.ToString()))
            {
                return (T)_aggregateRoots[aggregateRootId.ToString()];
            }

            return default(T);
        }
    }
}
