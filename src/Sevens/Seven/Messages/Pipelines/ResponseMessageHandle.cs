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
            var channelInfo = new RequestMessageContext(context.MessageWrapper.ExchangeName,
                context.MessageWrapper.ResponseRoutingKey, null, MessageExchangeType.Direct, true, false);

            var channelPools = ObjectContainer.Resolve<CommunicateChannelFactoryPool>();

            var replyChannel = channelPools.GetChannel(new PublisherContext(context.MessageWrapper.ExchangeName,
               MessageExchangeType.Direct, false, true));

            var binarySerializer = ObjectContainer.Resolve<IBinarySerializer>();

            replyChannel.Send(new SendMessage(binarySerializer.Serialize(context.Response),
                context.MessageWrapper.ResponseRoutingKey));
        }
    }
}
