using System;
using Seven.Infrastructure.Serializer;
using Seven.Messages.Channels;
using Seven.Messages.Pipelines;

namespace Seven.Messages.QueueMessages
{
    /// <summary>
    /// 消息响应结果处理
    /// </summary>
    public class MessageResponseHandler : IQueueMessageHandler
    {
        public void Handle(MessageContext context)
        {
            Console.WriteLine("receive the response message.");

            var message = context.QueueMessage;

            context.SetResponse(context.QueueMessage);

            var replyChannel = ReplyChannelPools.GetReplyChannel(message.MessageId);

            if (replyChannel == null) return;

            replyChannel.SetResult(context.Response);

        }
    }
}