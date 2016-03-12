using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Events;
using Seven.EventStore;
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

        public EventStream LoadEventStream(string aggregateRootId)
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
        public EventStream LoadEventStream(string aggregateRootId, int version)
        {
            var entity = _persistence.Get(new EventStreamSpecification(aggregateRootId, version));

            return _eventStreamFactory.Create(entity);
        }

        public void AppendToStream(string aggregateRootId, EventStream eventStream)
        {
            var entity = _eventStreamFactory.Create(aggregateRootId, eventStream.Version,eventStream.Events);

            _persistence.Save(entity);
        }
    }
}
