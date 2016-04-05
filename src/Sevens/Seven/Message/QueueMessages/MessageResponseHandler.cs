using Seven.Infrastructure.Serializer;
using Seven.Message.Channels;

namespace Seven.Message.QueueMessages
{
    /// <summary>
    /// 消息响应结果处理
    /// </summary>
    public class MessageResponseHandler : IQueueMessageHandler
    {
        private IBinarySerializer _binarySerializer;

        private MessageResponseHandler(IBinarySerializer binarySerializer)
        {
            _binarySerializer = binarySerializer;
        }

        public void Handle(QueueMessage message)
        {
            var messageResult = _binarySerializer.Deserialize<MessageHandleResult>(message.Datas);

            var replyChannel = ReplyChannelPools.GetReplyChannel(messageResult.MessageId);

            if (replyChannel == null) return;

            replyChannel.SetResult(messageResult);
        }
    }
}