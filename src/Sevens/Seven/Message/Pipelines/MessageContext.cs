using Seven.Infrastructure.Exceptions;
using Seven.Message.QueueMessages;

namespace Seven.Message.Pipelines
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

      
        public MessageContext(QueueMessage queueMessage, ulong deliveryTag)
        {
            QueueMessage = queueMessage;
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
