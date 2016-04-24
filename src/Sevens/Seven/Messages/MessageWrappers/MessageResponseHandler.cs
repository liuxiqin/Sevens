using System;
using Seven.Infrastructure.Serializer;
using Seven.Messages.Channels;
using Seven.Messages.Pipelines;

namespace Seven.Messages.QueueMessages
{
    /// <summary>
    /// ��Ϣ��Ӧ�������
    /// </summary>
    public class MessageResponseHandler : IQueueMessageHandler
    {
        public void Handle(MessageContext context)
        {
            Console.WriteLine("receive the response message.");

            var message = context.MessageWrapper;

            context.SetResponse(message.Message as MessageHandleResult);

            var replyChannel = ReplyChannelPools.GetReplyChannel(message.MessageId);

            if (replyChannel == null) return;

            replyChannel.SetResult(context.Response);

        }
    }
}