using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Infrastructure.Serializer;
using Seven.Message;
using Seven.Commands;
using Seven.Infrastructure.Ioc;
using Seven.Infrastructure.Repository;
using Seven.Initializer;

namespace Seven.Pipeline
{
    /// <summary>
    /// 处理消息
    /// </summary>
    public class ProcessMessageHandler : IMessageHandler
    {

        public void Execute(MessageContext context)
        {
            var command = context.Message as ICommand;

            IRepository repository = new EventSouringRepository();

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
