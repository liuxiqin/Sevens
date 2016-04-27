using System;
using Newtonsoft.Json;
using Seven.Infrastructure.IocContainer;
using Seven.Infrastructure.Serializer;
using Seven.Messages.Channels;
using Seven.Messages.QueueMessages;

namespace Seven.Messages.Pipelines
{
    public class ResponseMessageHandler : IMessageHandler
    {
        public void Handle(MessageContext context)
        {
            var channelPools = ObjectContainer.Resolve<CommunicateChannelFactoryPool>();

            var replyChannel = channelPools.GetChannel(new PublisherContext(context.MessageWrapper.ExchangeName,
               MessageExchangeType.Direct, false, true, true));

            var binarySerializer = ObjectContainer.Resolve<IBinarySerializer>();

            context.SetMessage(new MessageHandleResult() { MessageId = context.MessageWrapper.MessageId, Status = MessageStatus.Success });

            replyChannel.Send(new SendMessage(binarySerializer.Serialize(context.MessageWrapper),
                context.MessageWrapper.ResponseRoutingKey));
        }
    }
}
