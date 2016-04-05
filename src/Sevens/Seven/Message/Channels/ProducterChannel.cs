using RabbitMQ.Client;

namespace Seven.Message.Channels
{
    public class ProducterChannel : MessageChannelBase
    {
        private ChannelInfo _channelInfo;

        public ProducterChannel(ChannelInfo channelInfo) : base(channelInfo.Configuation)
        {
            _channelInfo = channelInfo;
        }

        public override void BindConsumer() { }

        public override void BindElements()
        {
            _channel.ExchangeDeclare(_channelInfo.ExChangeName, ExchangeType.Direct, true);
        }
    }
}