using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Aggregates;

namespace Seven.Commands
{
    public interface ICommandContext
    {
        void Add(IAggregateRoot aggregateRoot);

        T Get<T>(object aggregateRootId) where T : IAggregateRoot;
    }
}
