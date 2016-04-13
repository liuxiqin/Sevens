using System;
using System.Collections.Generic;
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

        public MessageRequestHandler()
        {
            InitHandlers();
        }


        private void InitHandlers()
        {
            // _handlers.Add(new ReceiveMessageHandler(_binarySerializer));
            // _handlers.Add(new ProcessMessageHandler());
            _handlers.Add(new AckMessageHandler());
            _handlers.Add(new ResponseMessageHandler());
        }

        public void Handle(MessageContext context)
        {
            try
            {
                var messageresult = @"成功";

                var result = new MessageHandleResult(context.QueueMessage.MessageId, messageresult,
                    MessageStatus.Success);


                context.SetResponse(result);

                Console.WriteLine(result.Message);

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