using System;
using Seven.Infrastructure.Exceptions;
using Seven.Infrastructure.Serializer;
using Seven.Messages.Channels;
using Seven.Messages.QueueMessages;

namespace Seven.Messages.Pipelines
{
    /// <summary>
    /// 消息处理上下文
    /// </summary>
    public class MessageContext
    {
        public IMessage Message { get; private set; }

        public MessageWrapper MessageWrapper { get; private set; }

        public ulong DeliveryTag { get; private set; }

        /// <summary>
        /// 消息响应结果
        /// </summary>
        public MessageHandleResult Response { get; private set; }
          
        public ConsumerContext ConsumerContext { get; private set; }
        
        public MessageContext(
            MessageWrapper queueMessage,
            ConsumerContext consuemrContext,
            ulong deliveryTag)
        {
            MessageWrapper = queueMessage;
            ConsumerContext = consuemrContext;
            DeliveryTag = deliveryTag;
        }


        /// <summary>
        /// 消息处理结果
        /// </summary>
        /// <param name="response"></param>
        public void SetResponse(MessageHandleResult response)
        {
            Response = response;
        }


        public MessageWrapper GetResponse()
        {
            var queueMessage = new MessageWrapper()
            {
                Message = Response, 
                IsRpcInvoke = true,
                MessageId = MessageWrapper.MessageId,
                ExchangeName = MessageWrapper.ExchangeName,
                TypeName = typeof(MessageHandleResult).FullName,
                RoutingKey = MessageWrapper.ResponseRoutingKey
            };

            return queueMessage;
        }

        public SevenException Exception { get; private set; }

        public void SetException(SevenException exception)
        {
            Exception = exception;
        }

        public void SetMessage(IMessage message)
        {
            Message = message;
        }
    }
}
