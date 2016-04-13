using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Messages.Channels;

namespace Seven.Messages.Pipelines
{
    public class AckMessageHandler : IMessageHandler
    {
        public void Handle(MessageContext context)
        {
            context.Channel.GetChannel().BasicAck(context.DeliveryTag, false);

          //  var replyChannel = MessageChannelPools.GetMessageChannel(context.ChannelInfo);

           // replyChannel.GetChannel().BasicAck(context.DeliveryTag, false);
        }
    }
}
