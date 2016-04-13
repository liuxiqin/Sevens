using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Events;
using Seven.Infrastructure.Persistence;

namespace Seven.Infrastructure.EventStore
{
    public class EventStore : IEventStore
    {
        private IPersistence<EventStreamEntity> _persistence;

        private EventStreamFactory _eventStreamFactory;

        public EventStore(IPersistence<EventStreamEntity> persistence, EventStreamFactory eventStreamFactory)
        {
            _persistence = persistence;

            _eventStreamFactory = eventStreamFactory;
        }

        public DomainEventStream LoadEventStream(string aggregateRootId)
        {
            var entity = _persistence.GetById(aggregateRootId);

            return _eventStreamFactory.Create(entity);
        }

        /// <summary>
        /// 加载版本后面所有的事件
        /// </summary>
        /// <param name="aggregateRootId"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public DomainEventStream LoadEventStream(string aggregateRootId, int version)
        {
            var entity = _persistence.Get(aggregateRootId, version);

            return _eventStreamFactory.Create(entity);
        }

        public void AppendAsync( DomainEventStream eventStream)
        {
            var entity = _eventStreamFactory.Create(eventStream.AggregateRootId, eventStream.Version,eventStream.Events);

            _persistence.Save(entity);
        }
    }
}
