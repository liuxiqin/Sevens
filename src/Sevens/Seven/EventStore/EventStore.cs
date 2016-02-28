using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Events;
using Seven.EventStore;
using Seven.Infrastructure.Persistence;
using Seven.Infrastructure.Serializer;

namespace Seven.Infrastructure.EventStore
{
    public class EventStore : IEventStore
    {
        private IPersistence _persistence;

        private EventStreamFactory _eventStreamFactory;

        public EventStore(IPersistence persistence, EventStreamFactory eventStreamFactory)
        {
            _persistence = persistence;

            _eventStreamFactory = eventStreamFactory;
        }

        public EventStream LoadEventStream(string aggregateRootId)
        {
            var entity = _persistence.GetById<EventStreamEntity>(aggregateRootId);

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
            var entity = _eventStreamFactory.Create(aggregateRootId, eventStream.Version, eventStream.Events);

            _persistence.Save(entity);
        }
    }

    public class EventStreamEntity : EntityBase
    {
        public string AggregateRootId { get; private set; }

        public int Version { get; private set; }

        public byte[] EventDatas { get; private set; }

        public EventStreamEntity(string aggregateRootId, int version, byte[] eventDatas)
        {
            AggregateRootId = aggregateRootId;
            Version = version;
            EventDatas = eventDatas;
        }
    }

    public class EventStreamFactory
    {
        private readonly IBinarySerializer _binarySerializer;

        public EventStreamFactory(IBinarySerializer binarySerializer)
        {
            _binarySerializer = binarySerializer;
        }

        public EventStream Create(EventStreamEntity entity)
        {
            var events = _binarySerializer.Deserialize<IList<IDomainEvent>>(entity.EventDatas);

            return new EventStream(entity.Version, events);
        }

        public EventStreamEntity Create(string aggregateRootId, int version, IList<IDomainEvent> events)
        {
            var datas = _binarySerializer.Serialize(events);

            return new EventStreamEntity(aggregateRootId, version, datas);
        }
    }
}
