using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Events;
using Seven.Infrastructure.Persistence;

namespace Seven.Infrastructure.EventStore
{
    public class EventStore : IEventStore
    {
        private IPersistence _persistence;

        public EventStore(IPersistence persistence)
        {
            _persistence = persistence;
        }

        public EventStream LoadEventStream(string aggregateRootId)
        {
            return _persistence.Get(aggregateRootId);
        }

        public EventStream LoadEventStream(string aggregateRootId, int version)
        {
            return _persistence.Get(aggregateRootId, version);
        }

        public void AppendToStream(string aggregateRootId, EventStream eventStream)
        {
            _persistence.Append(aggregateRootId, eventStream.Version, eventStream.Events);
        }
    }
}
