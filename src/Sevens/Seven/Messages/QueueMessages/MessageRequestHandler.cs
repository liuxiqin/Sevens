using System;
using System.Collections.Generic;
using Seven.Infrastructure.Exceptions;
using Seven.Infrastructure.Serializer;
using Seven.Messages.Channels;
using Seven.Messages.Pipelines;

namespace Seven.Messages.QueueMessages
{
    /// <summary>
    /// ��Ϣ������
    /// </summary>

    public class MessageRequestHandler : IQueueMessageHandler
    {
        private IBinarySerializer _binarySerializer;

        private List<IMessageHandler> _handlers = new List<IMessageHandler>();

        public MessageRequestHandler(IBinarySerializer binarySerializer)
        {
            _binarySerializer = binarySerializer;

            InitHandlers();
        }


        private void InitHandlers()
        {
            // _handlers.Add(new ReceiveMessageHandler(_binarySerializer));
            // _handlers.Add(new ProcessMessageHandler());
            //  _handlers.Add(new AckMessageHandler());
            _handlers.Add(new ResponseMessageHandler());
        }

        public void Handle(MessageContext context)
        {
            try
            {
                context.SetResponse(new MessageHandleResult()
                {
                    Message = "����ͨ��",
                    MessageId = context.QueueMessage.MessageId,
                    Status = MessageStatus.Success
                });

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