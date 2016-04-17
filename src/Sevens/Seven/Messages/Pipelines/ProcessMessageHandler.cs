using System;
using Seven.Commands;
using Seven.Events;

namespace Seven.Messages.Pipelines
{
    /// <summary>
    /// 处理消息
    /// </summary>
    public class ProcessMessageHandler : IMessageHandler
    {
        private ICommandProssor _commandProssor;

        public ProcessMessageHandler(ICommandProssor commandProssor)
        {
            _commandProssor = commandProssor;
        }

        public void Handle(MessageContext context)
        {
            try
            {
                if (context.Message is ICommand)
                {
                    _commandProssor.Execute(context.Message as ICommand);
                }

                if (context.Message is IEvent)
                {
                    var eventProssor = new DefaultEventProssor();

                    eventProssor.Execute(context.Message as IEvent);
                }


                context.SetResponse(new MessageHandleResult() { Message = "成功", Status = MessageStatus.Success });
            }
            catch (Exception)
            {
                context.SetResponse(new MessageHandleResult() { Message = "失败", Status = MessageStatus.Fail });
            }
        }
    }
}
