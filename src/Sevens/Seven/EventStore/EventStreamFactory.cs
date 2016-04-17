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

        public DomainEventStream Create(EventStreamRecord entity)
        {
            var events = _binarySerializer.Deserialize<IList<IEvent>>(entity.EventDatas);

            return new DomainEventStream(entity.AggregateRootId, entity.Version, entity.CommandId, events);
        }

        public EventStreamRecord Create(string aggregateRootId, string commandId, int version, IList<IEvent> events)
        {
            var datas = _binarySerializer.Serialize(events);

            return EventStreamRecordFactory.Create(aggregateRootId, commandId, version, datas); 
        }
    }
}