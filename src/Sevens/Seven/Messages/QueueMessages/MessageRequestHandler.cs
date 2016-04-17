using System;
using System.Collections.Generic;
using Seven.Commands;
using Seven.Infrastructure.Exceptions;
using Seven.Infrastructure.Serializer;
using Seven.Messages.Channels;
using Seven.Messages.Pipelines;

namespace Seven.Messages.QueueMessages
{
    /// <summary>
    /// 消息请求处理
    /// </summary>

    public class MessageRequestHandler : IQueueMessageHandler
    {
        private List<IMessageHandler> _handlers = new List<IMessageHandler>();

        private ICommandProssor _commandProssor;


        public MessageRequestHandler(ICommandProssor commandProssor)
        {
            _commandProssor = commandProssor;
            InitHandlers();
        }


        private void InitHandlers()
        {
            _handlers.Add(new ReceiveMessageHandler());
            _handlers.Add(new ProcessMessageHandler(_commandProssor));
            _handlers.Add(new AckMessageHandler());
            _handlers.Add(new ResponseMessageHandler());
        }

        public void Handle(MessageContext context)
        {
            try
            {
                _handlers.ForEach(m => m.Handle(context));
            }

            catch (BusinessException ex)
            {

            }
            catch (FrameworkException ex)
            {

            }
            catch (Exception ex)
            {

            }
        }
    }
}