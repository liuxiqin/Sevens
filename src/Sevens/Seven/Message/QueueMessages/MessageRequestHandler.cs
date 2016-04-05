using System;
using System.Collections.Generic;
using Seven.Infrastructure.Exceptions;
using Seven.Infrastructure.Serializer;
using Seven.Message.Pipelines;

namespace Seven.Message.QueueMessages
{
    /// <summary>
    /// 消息请求处理
    /// </summary>

    public class MessageRequestHandler : IQueueMessageHandler
    {
        private IBinarySerializer _binarySerializer;

        private List<IMessageHandler> _handlers = new List<IMessageHandler>();

        public MessageRequestHandler(IBinarySerializer binarySerializer)
        {
            _binarySerializer = binarySerializer;
        }


        private void InitHandlers()
        {
            _handlers.Add(new ReceiveMessageHandler(_binarySerializer));
            _handlers.Add(new ProcessMessageHandler());
            _handlers.Add(new ResponseMessageHandler(_binarySerializer));
        }

        public void Handle(QueueMessage message)
        {
            try
            {
                var context = new MessageContext(message, message.DeliveryTag);
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