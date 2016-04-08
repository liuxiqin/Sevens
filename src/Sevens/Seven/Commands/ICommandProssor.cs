using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Seven.Infrastructure.Ioc;
using Seven.Infrastructure.Repository;
using Seven.Initializer;

namespace Seven.Commands
{
    public interface ICommandProssor
    {
        void Execute(ICommand command);
    }

    public class DefaultCommandProssor : ICommandProssor
    {
        public void Execute(ICommand command)
        {
            IRepository repository = new EventSouringRepository(null, null);

            var comamndHandler = ObjectContainer.Resolve<CommandHandleProvider>();

            var action = comamndHandler.GetInternalCommandHandle(command.GetType());

            var commandContext = new CommandContext(repository);

            action(commandContext, command);

            var aggregateRoots = commandContext.AggregateRoots;

            var aggregateRoot = aggregateRoots.First();

            var domainEvents = aggregateRoot.Value.Commit();

            Console.WriteLine(aggregateRoot.ToString());

            Console.WriteLine("DomainEvents count is {0}", domainEvents.Count);
        }
    }
}
