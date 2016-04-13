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

        public QueueMessage QueueMessage { get; private set; }

        public ulong DeliveryTag { get; private set; }

        /// <summary>
        /// 消息响应结果
        /// </summary>
        public MessageHandleResult Response { get; private set; }

        public RequestMessageContext ChannelInfo { get; private set; }

        public MessageChannelBase Channel { get; private set; }

        public MessageContext(MessageChannelBase channel, QueueMessage queueMessage, RequestMessageContext channelInfo, ulong deliveryTag)
        {
            QueueMessage = queueMessage;
            ChannelInfo = channelInfo;
            DeliveryTag = deliveryTag;
            Channel = channel;
        }


        /// <summary>
        /// 消息处理结果
        /// </summary>
        /// <param name="response"></param>
        public void SetResponse(MessageHandleResult response)
        {
            Response = response;
        }


        public QueueMessage GetResponse()
        {
            var queueMessage = new QueueMessage()
            {
                Message = Response,
                DeliveryTag = DeliveryTag,
                IsRpcInvoke = true,
                MessageId = QueueMessage.MessageId,
                Topic = QueueMessage.Topic,
                TypeName = typeof(MessageHandleResult).FullName,
                RoutingKey = QueueMessage.ResponseRoutingKey
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
