using Seven.Infrastructure.Persistence;

namespace Seven.Infrastructure.EventStore
{
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
}