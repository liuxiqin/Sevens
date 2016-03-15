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
            _messageProducer.Publish(command);

            return null;
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
        private IMessageProducter _producter;

        /// <summary>
        /// 同步阻塞处理，返回结果
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public CommandExecutedResult Send(ICommand command)
        {
            var sendTask = _producter.Publish(command);

            var taskResult = sendTask.Result;

            return new CommandExecutedResult()
            {
                Message = taskResult.Message,
                Status = (CommandStatus)(int)taskResult.Status
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
