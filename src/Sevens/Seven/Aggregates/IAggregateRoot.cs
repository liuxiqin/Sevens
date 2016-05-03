using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using Seven.Events;

namespace Seven.Aggregates
{
    public interface IAggregateRoot
    {
        string AggregateRootId { get; }

        void ApplyEvent(IEvent evnt);
         
        void ApplyEvents(IList<IEvent> events);

        IList<IEvent> GetChanges();

        void Clear();

        int Version { get; }
    }
}
