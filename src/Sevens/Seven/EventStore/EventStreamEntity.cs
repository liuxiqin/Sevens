using Seven.Infrastructure.Persistence;

namespace Seven.Infrastructure.EventStore
{
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