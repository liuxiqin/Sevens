using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Message;

namespace Seven.Commands
{
    /// <summary>
    /// Command Send
    /// </summary>
    public class CommandService : ICommandService
    {
        private readonly IMessageProducer _messageProducer;

        public CommandService(IMessageProducer messageProducer)
        {
            _messageProducer = messageProducer;
        }

        public void Send(ICommand command)
        {
            _messageProducer.Publish(command);
        }

        public void SendAsync(ICommand command)
        {
            _messageProducer.PublishAsync(command);
        }
    }

    public class DefaultCommandService : ICommandService
    {
        public void Send(ICommand command)
        {
            throw new NotImplementedException();
        }

        public void SendAsync(ICommand command)
        {
            throw new NotImplementedException();
        }
    }
}
