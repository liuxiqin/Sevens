using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seven.Events;

namespace Seven.Infrastructure.EventStore
{
    public interface IEventStore
    {
        DomainEventStream LoadEventStream(string aggregateRootId);

        DomainEventStream LoadEventStream(string aggregateRootId, int version);

        void AppendAsync(DomainEventStream eventStream);
    }
}