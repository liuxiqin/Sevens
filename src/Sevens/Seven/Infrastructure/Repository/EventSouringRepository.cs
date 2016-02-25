using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Infrastructure.Repository
{
    public class EventSouringRepository : IRepository
    {
        public void Add(Aggregates.IAggregateRoot aggregateRoot)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(object aggregateRootId) where T : Aggregates.IAggregateRoot
        {
            throw new NotImplementedException();
        }
    }
}
