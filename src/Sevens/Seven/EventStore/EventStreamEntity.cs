using Seven.Infrastructure.Persistence;

namespace Seven.Infrastructure.EventStore
{
    public class EventStreamEntity : EntityBase
    {
        public string CommandId { get; set; }

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

    public class EventStreamRecord
    {
        public string CommandId { get; set; }

        public string AggregateRootId { get; set; }

        public int Version { get; set; }

        public byte[] EventDatas { get; set; }
    }

    public class EventStreamRecordFactory
    {
        public static EventStreamRecord Create(string aggregateRootId, string commandId, int version, byte[] eventDatas)
        {
            return new EventStreamRecord()
            {
                AggregateRootId = aggregateRootId,
                Version = version,
                EventDatas = eventDatas,
                CommandId = commandId
            };
        }
    }
}