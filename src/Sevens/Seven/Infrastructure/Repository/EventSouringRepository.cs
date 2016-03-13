using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Seven.Aggregates;
using Seven.Infrastructure.EventStore;
using Seven.Infrastructure.Snapshoting;

namespace Seven.Infrastructure.Repository
{
    public class EventSouringRepository : IRepository
    {
        private readonly IEventStore _eventStore;

        private readonly ISnapshotRepository _snapshotRepository;

        public EventSouringRepository(IEventStore eventStore, ISnapshotRepository snapshotRepository)
        {
            _eventStore = eventStore;
            _snapshotRepository = snapshotRepository;
        }

        public void Add(IAggregateRoot aggregateRoot)
        {
            throw new NotImplementedException();
        }

        public TAggregateRoot Get<TAggregateRoot>(string aggregateRootId) where TAggregateRoot : IAggregateRoot
        {
            var snapshot = _snapshotRepository.Get(aggregateRootId);

            var aggregateRoot = default(TAggregateRoot);

            if (snapshot != null)
            {
                aggregateRoot = (TAggregateRoot)snapshot.AggregateRoot;
            }

            if (aggregateRoot == null)
            {
                aggregateRoot = (TAggregateRoot)FormatterServices.GetUninitializedObject(typeof(TAggregateRoot));
            }

            var eventStream = _eventStore.LoadEventStream(aggregateRootId, aggregateRoot.Version);

            aggregateRoot.ApplyEvents(eventStream.Events);

            return aggregateRoot;
        }
    }
}
