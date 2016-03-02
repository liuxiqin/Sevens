using System.Collections.Concurrent;
using System.Collections.Generic;
using Seven.Aggregates;

namespace Seven.Infrastructure.Repository
{
    public class DefaultRepository : IRepository
    {
        private readonly IDictionary<string, IAggregateRoot> _aggregateRoots;

        private readonly EventSouringRepository _eventSouringRepository;

        public DefaultRepository(EventSouringRepository eventSouringRepository)
        {
            _aggregateRoots = new ConcurrentDictionary<string, IAggregateRoot>();
            _eventSouringRepository = eventSouringRepository;
        }

        public void Add(IAggregateRoot aggregateRoot)
        {
            if (!_aggregateRoots.ContainsKey(aggregateRoot.AggregateRootId))
            {
                _aggregateRoots.Add(aggregateRoot.AggregateRootId, aggregateRoot);
            }
        }

        public T Get<T>(string aggregateRootId) where T : IAggregateRoot
        {
            if (_aggregateRoots.ContainsKey(aggregateRootId))
            {
                return (T)_aggregateRoots[aggregateRootId];
            }
            return _eventSouringRepository.Get<T>(aggregateRootId);
        }
    }
}
