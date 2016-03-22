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

        private readonly IMessageConsumer _messageConsumer;

        public CommandService(IMessageProducter messageProducer, MessageConsumer messageConsumer)
        {
            _messageProducer = messageProducer;

            _messageConsumer = messageConsumer;
        }

        public CommandExecutedResult Send(ICommand command)
        {
            var task = _messageProducer.Publish(command);

            var sendResult = task.Result;

            return new CommandExecutedResult()
            {
                Message = task.Result.Message,
                Status = task.Result.Status == MessageStatus.Success ? CommandStatus.Success : CommandStatus.Error
            };
        }

        public async void SendAsync(ICommand command)
        {
            _messageProducer.PublishAsync(command);
        }

        public async Task<MessageHandleResult> AsyncSend(ICommand command)
        {
            return await Task.Run(() => { return new MessageHandleResult() { }; });
        }
    }

    public class DefaultCommandService : ICommandService
    {
        private readonly IMessageProducter _messageProducer;

        private readonly IMessageConsumer _messageConsumer;

        public DefaultCommandService(IMessageProducter messageProducer, MessageConsumer messageConsumer)
        {
            _messageProducer = messageProducer;

            _messageConsumer = messageConsumer;
        }

        public CommandExecutedResult Send(ICommand command)
        {
            _messageProducer.Publish(command);

            var timeout = TimeSpan.FromSeconds(20);

            var handleResult = _messageConsumer.SubscribeResult("Rpc_Response", command.MessageId, timeout);

            return new CommandExecutedResult()
            {
                Message = handleResult.Message,
                Status = handleResult.Status == MessageStatus.Success ? CommandStatus.Success : CommandStatus.Error
            };
        }

        /// <summary>
        /// 异步处理返回结果，/配合MVC/WebApi异步处理方式
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<MessageHandleResult> AsyncSend(ICommand command)
        {
            return await Task.Run(() => { return new MessageHandleResult() { }; });
        }

        /// <summary>
        /// 异步无阻塞无返回结果
        /// </summary>
        /// <param name="command"></param>
        public async void SendAsync(ICommand command)
        {

        }
    }
}
