using System;
using System.Linq;
using Seven.Infrastructure.Serializer;
using Seven.Commands;
using Seven.Events;
using Seven.Infrastructure.Ioc;
using Seven.Infrastructure.Repository;
using Seven.Initializer;

namespace Seven.Messages.Pipelines
{
    /// <summary>
    /// 处理消息
    /// </summary>
    public class ProcessMessageHandler : IMessageHandler
    {
        public void Handle(MessageContext context)
        {
            try
            {
                if (context.Message is ICommand)
                {
                    var commandProssor = new DefaultCommandProssor();

                    commandProssor.Execute(context.Message as ICommand);
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
