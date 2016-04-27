using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Aggregates;

namespace Seven.Infrastructure.Repository
{
    public class AggregateRootMemoryCache : IAggregateRootMemoryCache
    {
        private ConcurrentDictionary<string, IAggregateRoot> _aggregateRoots;

        public AggregateRootMemoryCache()
        {
            _aggregateRoots = new ConcurrentDictionary<string, IAggregateRoot>();
        }

        public void Add(IAggregateRoot aggregateRoot)
        {
            if (!_aggregateRoots.ContainsKey(aggregateRoot.AggregateRootId))
            {
                _aggregateRoots.TryAdd(aggregateRoot.AggregateRootId, aggregateRoot);
            }
        }

        public IAggregateRoot Get(string aggregateRootId)
        {
            if (_aggregateRoots.ContainsKey(aggregateRootId))
            {
                return _aggregateRoots[aggregateRootId];
            }

            return null;
        }
    }
}
