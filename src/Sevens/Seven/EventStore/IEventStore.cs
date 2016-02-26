using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seven.Events;

namespace Seven.Infrastructure.EventStore
{
    public interface IEventStore
    {
        EventStream LoadEventStream(string aggregateRootId);

        EventStream LoadEventStream(string aggregateRootId, int version);

        void AppendToStream(string aggregateRootId, EventStream eventStream);
    }
}