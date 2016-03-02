using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Events;
using Seven.Infrastructure.EventStore;
using Seven.Infrastructure.Snapshoting;

namespace Seven.EventStore
{
    public class EventStreamSpecification : ISpecification<EventStreamEntity>
    {
        public string AggregateRootId { get; set; }

        public int Version { get; set; }

        public EventStreamSpecification(string aggregateRootId, int version)
        {
            AggregateRootId = aggregateRootId;
            Version = version;
        }

        public bool IsSatisfiedBy(EventStreamEntity entity)
        {
            return entity.AggregateRootId.Equals(AggregateRootId) && entity.Version == Version;
        }

    }
}
