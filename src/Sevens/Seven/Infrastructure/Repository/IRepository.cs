using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Aggregates;

namespace Seven.Infrastructure.Repository
{
    public interface IRepository
    {
        void Add(IAggregateRoot aggregateRoot);

        TAggregateRoot Get<TAggregateRoot>(string aggregateRootId) where TAggregateRoot : IAggregateRoot;
    }
}
