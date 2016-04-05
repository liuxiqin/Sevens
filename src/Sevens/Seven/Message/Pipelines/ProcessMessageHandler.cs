using System;
using System.Linq;
using Seven.Infrastructure.Serializer;
using Seven.Commands;
using Seven.Infrastructure.Ioc;
using Seven.Infrastructure.Repository;
using Seven.Initializer;

namespace Seven.Message.Pipelines
{
    /// <summary>
    /// 处理消息
    /// </summary>
    public class ProcessMessageHandler : IMessageHandler
    {
        private IMessageHandler _nextHandler = new ResponseMessageHandler(new DefaultBinarySerializer());

        public void Handle(MessageContext context)
        {
            try
            {
                var command = context.Message as ICommand;

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

                context.SetResponse(new MessageHandleResult() { Message = "成功", Status = MessageStatus.Success });
            }
            catch (Exception)
            {
                context.SetResponse(new MessageHandleResult() { Message = "失败", Status = MessageStatus.Fail });
            }

            _nextHandler.Handle(context);
        }
    }
}
