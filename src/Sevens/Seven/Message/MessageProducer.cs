using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;
using Seven.Infrastructure.Serializer;

namespace Seven.Message
{
    /// <summary>
    /// 消息生产者
    /// </summary>
    public class MessageProducer : IMessageProducer
    {
        private IBinarySerializer _binarySerializer;

        private IJsonSerializer _jsonSerializer;

        private readonly MessageBroker _messageBroker;

        public MessageProducer(
            MessageBroker messageBroker,
            IBinarySerializer binarySerializer,
            IJsonSerializer jsonSerializer)
        {
            _messageBroker = messageBroker;
            _binarySerializer = binarySerializer;
            _jsonSerializer = jsonSerializer;
        }

        public Task<MessageHandleResult> Publish<TMessage>(TMessage message) where TMessage : IMessage
        {
            using (var channel = _messageBroker.GetConnection.CreateModel())
            {
                var tag = typeof(TMessage).ToString();
                var topic = typeof(TMessage).ToString();

                var data = _jsonSerializer.Serialize(message);

                var queueMessage = new QueueMessage()
                {
                    Tag = tag,
                    Topic = topic,
                    Datas = Encoding.UTF8.GetBytes(data),
                    TypeName = typeof(TMessage).FullName
                };

                channel.ExchangeDeclare(queueMessage.Topic, ExchangeType.Direct, true);

                channel.BasicPublish(topic, tag, new BasicProperties() { DeliveryMode = 2 },
                    _binarySerializer.Serialize(queueMessage));
            }

            return null;
        }

        public Task PublishAsync<TMessage>(TMessage message) where TMessage : IMessage
        {
            return null;
        }
    }
}
