using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Events
{
    public class DomainEventStream
    {
        public int Version { get; set; }

        public IList<IEvent> Events { get; set; }

        public string CommandId { get; set; }

        public string AggregateRootId { get; set; }

        public DomainEventStream(string aggregateRootId, int version, string commandId, IList<IEvent> events)
        {
            Version = version;
            Events = events;
            AggregateRootId = aggregateRootId;
            CommandId = commandId;
        }
    }

}
