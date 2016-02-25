using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Aggregates;
using Seven.Infrastructure.Repository;

namespace Seven.Commands
{
    /// <summary>
    /// 命令执行上下文
    /// </summary>
    public class CommandContext : ICommandContext
    {
        private IRepository _repository;

        public ICommand CurrentCommand { get; private set; }

        public IDictionary<string, IAggregateRoot> AggregateRoots { get; private set; }

        public CommandContext(IRepository repository)
        {
            this._repository = repository;

            AggregateRoots = new Dictionary<string, IAggregateRoot>();
        }

        public Type GetCommandType
        {
            get { return this.CurrentCommand.GetType(); }
        }

        public CommandExecutionState ExecuteState { get; private set; }
        

        public void Add(IAggregateRoot aggregate)
        {
            AggregateRoots.Add(aggregate.AggregateRootId, aggregate);
        }

        public T Get<T>(object aggregateRootId) where T : IAggregateRoot
        {
            if (!AggregateRoots.ContainsKey(aggregateRootId.ToString()))
            {
                var aggregateRoot = _repository.Get<IAggregateRoot>(aggregateRootId);

                AggregateRoots.Add(aggregateRootId.ToString(), aggregateRoot);
            }
            return (T)AggregateRoots[aggregateRootId.ToString()];
        }
    }
}
