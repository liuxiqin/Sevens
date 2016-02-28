using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Seven.Events;

namespace Seven.Aggregates
{

    public interface IAggregateRoot
    {
        string AggregateRootId { get; }

        void ApplyEvent(IDomainEvent evnt);

        IList<IEvent> Commit();

        void ApplyEvents(IList<IDomainEvent> events);

        IList<IEvent> GetChanges(); 

        int Version { get;}
    }
}
