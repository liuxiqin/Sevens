using System;
using Newtonsoft.Json;
using Seven.Infrastructure.Dependency;
using Seven.Infrastructure.Serializer;
using Seven.Messages.Channels;
using Seven.Messages.QueueMessages;

namespace Seven.Messages.Pipelines
{
    public class ResponseMessageHandler : IMessageHandler
    {
        public void Handle(MessageContext context)
        {
            var channelPools = DependencyResolver.Resolve<CommunicateChannelFactoryPool>();

            var replyChannel = channelPools.GetChannel(new PublisherContext(context.MessageWrapper.ExchangeName,
               MessageExchangeType.Direct, false, true, true));

            var binarySerializer = DependencyResolver.Resolve<IBinarySerializer>();

            var responseMessage = new MessageWrapper()
            {
                AuotDelete = context.MessageWrapper.AuotDelete,
                Durable = context.MessageWrapper.Durable,
                ExchangeName = context.MessageWrapper.ExchangeName,
                IsRpcInvoke = context.MessageWrapper.IsRpcInvoke,
                Message = context.Response,
                MessageId = context.MessageWrapper.MessageId,
                MessageType = context.MessageWrapper.MessageType,
                ResponseRoutingKey = context.MessageWrapper.ResponseRoutingKey,
                RoutingKey = context.MessageWrapper.RoutingKey,
                TypeName = context.MessageWrapper.TypeName,
                TimeStamp = DateTime.Now
            };

            replyChannel.Send(new SendMessage(binarySerializer.Serialize(responseMessage),
                context.MessageWrapper.ResponseRoutingKey));
        }
    }
}
