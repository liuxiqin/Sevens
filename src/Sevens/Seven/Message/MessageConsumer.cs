using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing;
using Seven.Commands;
using Seven.Infrastructure.Exceptions;
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
            IBinarySerializer binarySerializer,
            IJsonSerializer jsonSerializer,
            string exchangeName,
            string routingKey)
        {
            _messageBroker = messageBroker;

            _exchangeName = exchangeName;
            _routingKey = routingKey;

            _binarySerializer = binarySerializer;

            _jsonSerializer = jsonSerializer;

            _messgaeExecute = new DefaultMessgaeExecute();
        }

        public void Start()
        {
            var consumerChannel = _messageBroker.GetConnection.CreateModel();

            consumerChannel.ExchangeDeclare(_exchangeName, ExchangeType.Direct, true);

            consumerChannel.QueueDeclare(_routingKey, true, false, false, null);

            consumerChannel.QueueBind(_routingKey, _exchangeName, _routingKey);

            consumerChannel.BasicQos(0, 1, false);

            var consumer = new QueueingBasicConsumer(consumerChannel);

            consumerChannel.BasicConsume(_routingKey, false, consumer);

            while (true)
            {
                var basicDeliverEventArgs = consumer.Queue.Dequeue();

                var bytes = basicDeliverEventArgs.Body;

                var queueMessage = _binarySerializer.Deserialize<QueueMessage>(bytes);

                var messageTypeProvider = ObjectContainer.Resolve<MessageTypeProvider>();

                var message = _jsonSerializer.DeSerialize(Encoding.UTF8.GetString(queueMessage.Datas),
                    messageTypeProvider.GetType(queueMessage.TypeName)) as IMessage;

                var messageContext = new MessageContext(consumerChannel, _messageBroker.GetConnection, message,
                    _routingKey, _exchangeName,
                    basicDeliverEventArgs.DeliveryTag);

                _messgaeExecute.Execute(messageContext);

                consumerChannel.BasicAck(basicDeliverEventArgs.DeliveryTag, true);

                Thread.Sleep(1);
            }
        }
        
        public void Stop()
        {

        }
    }
}
