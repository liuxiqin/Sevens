using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Infrastructure.EventStore;

namespace Seven.Infrastructure.Repository
{
    public class EventSouringRepository : IRepository
    {
        private IEventStore _eventStore;

        public EventSouringRepository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public void Add(Aggregates.IAggregateRoot aggregateRoot)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string aggregateRootId) where T : Aggregates.IAggregateRoot
        {
            throw new NotImplementedException();
        }
    }
}
