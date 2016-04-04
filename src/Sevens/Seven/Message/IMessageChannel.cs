using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;

namespace Seven.Message
{
    public interface IMessageChannel
    {
        QueueMessage ReceiveMessage();

        void SendMessage(QueueMessage message);
    }

    /// <summary>
    /// RaabitMq推送消息消费通道
    /// </summary>
    public class PushConsumerChannel : ConsumerChannel
    {
        public PushConsumerChannel(ChannelInfo channelInfo) : base(channelInfo)
        {

        }
    }

    /// <summary>
    /// RaabitMq拉取消息消费通道
    /// </summary>
    public class PollConsumerChannel : ConsumerChannel
    {
        public PollConsumerChannel(ChannelInfo channelInfo) : base(channelInfo)
        {
        }
    }
}
