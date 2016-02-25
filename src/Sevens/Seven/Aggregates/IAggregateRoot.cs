using System.Collections;
using System.Collections.Generic;
using Seven.Events;

namespace Seven.Aggregates
{

    public interface IAggregateRoot
    {
        string AggregateRootId { get; }

        void ApplyEvent(IDomainEvent evnt);

        IList<IDomainEvent> Commit();
    }
}
