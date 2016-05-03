using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Infrastructure.Dependency;
using Seven.Messages.Channels;

namespace Seven.Messages.Pipelines
{
    public class AckMessageHandler : IMessageHandler
    {
        public void Handle(MessageContext context)
        {
            var channelPools = DependencyResolver.Resolve<CommunicateChannelFactoryPool>();

            var channel = channelPools.GetChannel(context.ConsumerContext);

            channel.BasicAck(context.DeliveryTag);
        }
    }
}
