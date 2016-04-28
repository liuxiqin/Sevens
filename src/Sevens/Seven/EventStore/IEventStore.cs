using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Events;

namespace Seven.Infrastructure.EventStore
{
    public interface IEventStore
    {
        EventStreamRecord LoadEventStream(string aggregateRootId);

        EventStreamRecord LoadEventStream(string aggregateRootId, int version);

        bool AppendAsync(EventStreamRecord eventStream);
    }
}