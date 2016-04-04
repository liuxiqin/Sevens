using RabbitMQ.Client;
using RabbitMQ.Client.Framing;
using Seven.Infrastructure.Serializer;

namespace Seven.Message
{
    public abstract class MessageChannelBase
    {
        protected IMessageBroker _messageBroker = null;

        protected IModel _channel = null;

        protected IBinarySerializer _binarySerializer;

        protected QueueingBasicConsumer _consumer = null;

        public MessageChannelBase(RabbitMqConfiguration configution)
        {
            _messageBroker = new MessageBroker(configution);

            _binarySerializer = new DefaultBinarySerializer();
        }

        public virtual QueueMessage ReceiveMessage()
        {
            var deliverEvent = _consumer.Queue.Dequeue();

            var queueMessage = _binarySerializer.Deserialize<QueueMessage>(deliverEvent.Body);

            return queueMessage;
        }

        public void SendMessage(QueueMessage message)
        {
            var channel = GetChannel();

            channel.BasicPublish(message.Topic, message.RoutingKey, new BasicProperties() { DeliveryMode = 2 },
                _binarySerializer.Serialize(message));
        }

        public abstract void BindElements();

        public abstract void BindConsumer();

        public IModel GetChannel()
        {
            if (_channel == null || _channel.IsClosed)
            {
                _channel = _messageBroker.GetConnection.CreateModel();

                BindElements();

                BindConsumer();
            }

            return _channel;
        }
    }
}