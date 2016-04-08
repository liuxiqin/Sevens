using RabbitMQ.Client;

namespace Seven.Messages.Channels
{
    public class ProducterChannel : MessageChannelBase
    {
        private RequestMessageContext _channelInfo;

        public ProducterChannel(RequestMessageContext channelInfo) : base(channelInfo.Configuation)
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