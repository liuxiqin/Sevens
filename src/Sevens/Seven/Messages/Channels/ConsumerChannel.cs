using RabbitMQ.Client;

namespace Seven.Messages.Channels
{
    public class ConsumerChannel : MessageChannelBase
    {
        private RequestMessageContext _channelInfo = null;

        public ConsumerChannel(RequestMessageContext channelInfo) : base(channelInfo.Configuation)
        {
            _channelInfo = channelInfo;
        }

        public override void BindConsumer()
        {
            _consumer = new QueueingBasicConsumer(_channel);

            _channel.BasicConsume(_channelInfo.RoutingKey, false, _consumer);
        }

        public override void BindElements()
        {
            _channel.ExchangeDeclare(_channelInfo.ExChangeName, ExchangeType.Direct, true);

            _channel.QueueDeclare(_channelInfo.RoutingKey, true, false, false, null);

            _channel.QueueBind(_channelInfo.RoutingKey, _channelInfo.ExChangeName, _channelInfo.RoutingKey);
        }
    }
}