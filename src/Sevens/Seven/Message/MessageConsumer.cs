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
        private readonly IMessageBroker _messageBroker = null;

        private readonly string _exchangeName;

        private readonly string _routingKey;

        private readonly IMessgaeExecute _messgaeExecute;

        private readonly IBinarySerializer _binarySerializer;

        private readonly IJsonSerializer _jsonSerializer;

        private readonly IModel _consumerChannel = null;

        private readonly CancellationTokenSource _cancellationTokenSource = null;

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

            _consumerChannel = _messageBroker.GetConnection.CreateModel();

            _consumerChannel.ExchangeDeclare(_exchangeName, ExchangeType.Direct, true);

            _consumerChannel.QueueDeclare(_routingKey, true, false, false, null);

            _consumerChannel.QueueBind(_routingKey, _exchangeName, _routingKey);

            _consumerChannel.BasicQos(0, 1, false);

            _cancellationTokenSource = new CancellationTokenSource();

        }

        public void Start()
        {
            var comsumerTask = new Task(() => { }, _cancellationTokenSource.Token);

            var consumer = new QueueingBasicConsumer(_consumerChannel);

            _consumerChannel.BasicConsume(_routingKey, false, consumer);

            var basicDeliverEventArgs = consumer.Queue.Dequeue();

            var bytes = basicDeliverEventArgs.Body;

            var queueMessage = _binarySerializer.Deserialize<QueueMessage>(bytes);

        }

        public void Execute(QueueMessage queueMessage)
        {
            var messageTypeProvider = ObjectContainer.Resolve<MessageTypeProvider>();

            var message = _jsonSerializer.DeSerialize(Encoding.UTF8.GetString(queueMessage.Datas),
                messageTypeProvider.GetType(queueMessage.TypeName)) as IMessage;

            var messageContext = new MessageContext(_consumerChannel, _messageBroker.GetConnection, message,
                _routingKey, _exchangeName, 1);

            _messgaeExecute.Execute(messageContext);

            _consumerChannel.BasicAck(1, true);

            Thread.Sleep(1);
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }
    }

    public class MessageResultConsumer : IMessageConsumer
    {
        private readonly IMessageBroker _messageBroker = null;

        private readonly string _exchangeName;

        private readonly string _routingKey;

        private readonly IMessgaeExecute _messgaeExecute;

        private readonly IBinarySerializer _binarySerializer;

        private readonly IModel _consumerChannel = null;

        private readonly CancellationTokenSource _cancellationTokenSource = null;

        public MessageResultConsumer(IMessageBroker messageBroker,
            IBinarySerializer binarySerializer,
            string exchangeName,
            string routingKey)
        {
            _messageBroker = messageBroker;

            _exchangeName = exchangeName;
            _routingKey = routingKey;

            _binarySerializer = binarySerializer;

            _consumerChannel = _messageBroker.GetConnection.CreateModel();

            _consumerChannel.ExchangeDeclare(_exchangeName, ExchangeType.Direct, true);

            _consumerChannel.QueueDeclare(_routingKey, true, false, false, null);

            _consumerChannel.QueueBind(_routingKey, _exchangeName, _routingKey);

            _consumerChannel.BasicQos(0, 1, false);

            _cancellationTokenSource = new CancellationTokenSource();

        }

        public void Start()
        {
            var comsumerTask = new Task(() => { }, _cancellationTokenSource.Token);

            var consumer = new QueueingBasicConsumer(_consumerChannel);

            _consumerChannel.BasicConsume(_routingKey, false, consumer);

            var basicDeliverEventArgs = consumer.Queue.Dequeue();

            var bytes = basicDeliverEventArgs.Body;

            var queueMessage = _binarySerializer.Deserialize<QueueMessage>(bytes);

        }

        public void Execute(QueueMessage queueMessage)
        {
            var messageResult = new MessageHandleResult();

            var replyChannel = ReplyChannelPools.GetReplyChannel(messageResult.MessageId);

            if (replyChannel != null)
            {
                replyChannel.SetResult(messageResult);
            }

            //var message = _jsonSerializer.DeSerialize(Encoding.UTF8.GetString(queueMessage.Datas),
            // messageTypeProvider.GetType(queueMessage.TypeName)) as IMessage;

            //var messageContext = new MessageContext(_consumerChannel, _messageBroker.GetConnection, message,
            //    _routingKey, _exchangeName, 1);

            //_messgaeExecute.Execute(messageContext);

            //_consumerChannel.BasicAck(1, true);

            //Thread.Sleep(1);
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
