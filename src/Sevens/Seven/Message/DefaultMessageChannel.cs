using RabbitMQ.Client;
using RabbitMQ.Client.Framing;
using Seven.Infrastructure.Serializer;

namespace Seven.Message
{
    /// <summary>
    /// rabbitmq
    /// </summary>
    public class DefaultMessageChannel : IMessageChannel
    {
        private IMessageBroker _messageBroker = null;

        private IModel _channel = null;

        private ChannelInfo _channelInfo = null;

        private IBinarySerializer _binarySerializer;


        private IModel GetChannel()
        {
            if (_messageBroker == null || !_messageBroker.GetConnection.IsOpen)
                _messageBroker = new MessageBroker(_channelInfo.Configuation);

            if (_channel == null || _channel.IsClosed)
                _channel = _messageBroker.GetConnection.CreateModel();

            return _channel;
        }

        public DefaultMessageChannel(ChannelInfo channelInfo, IBinarySerializer binarySerializer)
        {
            _channelInfo = channelInfo;
            _binarySerializer = binarySerializer;
        }

        public QueueMessage ReceiveMessage()
        {
            var channel = GetChannel();

            return new QueueMessage();
        }

        public void SendMessage(QueueMessage message)
        {
            var channel = GetChannel();

            channel.BasicPublish(message.Topic, message.RoutingKey, new BasicProperties() { DeliveryMode = 2 },
                _binarySerializer.Serialize(message));
        }
    }
}