using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Seven.Commands;
using Seven.Infrastructure.Ioc;
using Seven.Infrastructure.Serializer;
using Seven.Initializer;
using Seven.Pipeline;

namespace Seven.Message
{
    /// <summary>
    /// 消息消费者
    /// </summary>
    public class MessageConsumer : IMessageConsumer
    {
        private readonly IMessageBroker _messageBroker;

        private readonly string _exchangeName;
        private readonly string _routingKey;

        private readonly IMessgaeExecute _messgaeExecute;

        private readonly IBinarySerializer _binarySerializer;

        private readonly IJsonSerializer _jsonSerializer;

        public MessageConsumer(IMessageBroker messageBroker,
            IMessgaeExecute messgaeExecute,
            IBinarySerializer binarySerializer,
            IJsonSerializer jsonSerializer,
            string exchangeName,
            string routingKey)
        {
            _messageBroker = messageBroker;

            _messgaeExecute = messgaeExecute;

            _exchangeName = exchangeName;
            _routingKey = routingKey;

            _binarySerializer = binarySerializer;

            _jsonSerializer = jsonSerializer;
        }

        public void Start()
        {
            ulong deliveryTag = 0;

            var connection = _messageBroker.GetConnection;

            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(_exchangeName, ExchangeType.Direct, true);

                channel.QueueDeclare(_routingKey, true, false, false, null);

                channel.QueueBind(_routingKey, _exchangeName, _routingKey);

                var consumer = new QueueingBasicConsumer(channel);

                channel.BasicConsume(_routingKey, false, consumer);

                var basicDeliverEventArgs = consumer.Queue.Dequeue();
                var bytes = basicDeliverEventArgs.Body;

                var queueMessage = _binarySerializer.Deserialize<QueueMessage>(bytes);

                var messageTypeProvider = ObjectContainer.Resolve<MessageTypeProvider>();

                var message = _jsonSerializer.DeSerialize(Encoding.UTF8.GetString(queueMessage.Datas),
                        messageTypeProvider.GetType(queueMessage.TypeName)) as IMessage;

                var messageContext = new MessageContext(channel, message, _routingKey, _exchangeName,
                    basicDeliverEventArgs.DeliveryTag);

                _messgaeExecute.Execute(messageContext);
            }
        }

        public void BasicAck(ulong deliveryTag)
        {
            using (var connection = _messageBroker.GetConnection)
            {

                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(_exchangeName, ExchangeType.Direct, true);

                    channel.QueueDeclare(_routingKey, true, false, false, null);

                    channel.QueueBind(_routingKey, _exchangeName, _routingKey);

                    var consumer = new QueueingBasicConsumer(channel);

                    channel.BasicConsume(_routingKey, false, consumer);

                    channel.BasicAck(deliveryTag, true);
                }
            }
        }
        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
