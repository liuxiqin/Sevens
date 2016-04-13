using System.Collections.Generic;
using Seven.Events;
using Seven.Infrastructure.Serializer;

namespace Seven.Infrastructure.EventStore
{
    public class EventStreamFactory
    {
        private readonly IBinarySerializer _binarySerializer;

        public EventStreamFactory(IBinarySerializer binarySerializer)
        {
            _binarySerializer = binarySerializer;
        }

        public DomainEventStream Create(EventStreamEntity entity)
        {
            var events = _binarySerializer.Deserialize<IList<IEvent>>(entity.EventDatas);

            return new DomainEventStream(entity.AggregateRootId, entity.Version, entity.CommandId, events);
        }

        public EventStreamEntity Create(string aggregateRootId, int version, IList<IEvent> events)
        {
            var datas = _binarySerializer.Serialize(events);

            return new EventStreamEntity(aggregateRootId, version, datas);
        }
    }
}