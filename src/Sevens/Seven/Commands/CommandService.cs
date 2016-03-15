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

        public Task<CommandExecutedResult> Send(ICommand command)
        {
            _messageProducer.Publish(command);

            return null;
        }

        public void SendAsync(ICommand command)
        {
            _messageProducer.PublishAsync(command);
        }
    }

    public class DefaultCommandService : ICommandService
    {
        public Task<CommandExecutedResult> Send(ICommand command)
        {
            throw new NotImplementedException();
        }

        public async void SendAsync(ICommand command)
        {
            throw new NotImplementedException();
        }
    }
}
