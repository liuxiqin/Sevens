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
            try
            {
                Console.WriteLine("receive the response send.{0}, and now is {1}", context.MessageWrapper.TimeStamp, DateTime.Now);

                var message = context.MessageWrapper;

                context.SetResponse(message.Message as MessageHandleResult);

                var replyChannel = ReplyChannelPools.GetReplyChannel(message.MessageId);

                if (replyChannel == null)
                {
                    Console.WriteLine("replyChannel is null.");
                    return;
                }

                replyChannel.SetResult(context.Response);
            }
            catch (Exception ex)
            {
                Console.WriteLine("replyChannel is waiting is error.");
            }
        }
    }
}