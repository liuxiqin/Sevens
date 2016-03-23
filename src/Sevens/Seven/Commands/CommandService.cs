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
        private readonly IMessageProducter _messageProducer;

        public CommandService(IMessageProducter messageProducer)
        {
            _messageProducer = messageProducer;
        }

        public CommandExecutedResult Send(ICommand command)
        {
            var senedTask = _messageProducer.Publish(command);

            var sendResult = senedTask.Result;

            return new CommandExecutedResult()
            {
                Message = sendResult.Message,
                Status = sendResult.Status == MessageStatus.Success ? CommandStatus.Success : CommandStatus.Error
            };
        }

        public async void SendAsync(ICommand command)
        {
            _messageProducer.PublishAsync(command);
        }

        public async Task<MessageHandleResult> AsyncSend(ICommand command)
        {
            return await Task.Run(() => { return new MessageHandleResult() {}; });
        }
    }
}
