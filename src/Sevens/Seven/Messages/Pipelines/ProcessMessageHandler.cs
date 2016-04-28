using System;
using System.Diagnostics;
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
                Stopwatch watch = new Stopwatch();
                watch.Start();

                if (context.Message is ICommand)
                {
                    _commandProssor.Execute(context.Message as ICommand);
                }

                if (context.Message is IEvent)
                {
                    var eventProssor = new DefaultEventProssor();

                    eventProssor.Execute(context.Message as IEvent);
                }

                context.SetResponse(new MessageHandleResult() { Message = "成功", Status = MessageStatus.Success, MessageId = context.Message.MessageId });

                watch.Stop();

                Console.WriteLine("message proccess time:{0}", watch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                context.SetResponse(new MessageHandleResult() { Message = ex.Message, Status = MessageStatus.Failure, MessageId = context.Message.MessageId });
            }
        }
    }
}
