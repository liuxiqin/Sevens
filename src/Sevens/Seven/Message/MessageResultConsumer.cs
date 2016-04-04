using System;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Seven.Infrastructure.Serializer;

namespace Seven.Message
{
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

            _consumerChannel.QueueDeclare(_routingKey, false, true, true, null);

            _consumerChannel.QueueBind(_routingKey, _exchangeName, _routingKey);

            _consumerChannel.BasicQos(0, 1, false);

            _cancellationTokenSource = new CancellationTokenSource();

        }

        public void Start()
        {
            var comsumerTask = new Task(() =>
            {
                var consumer = new QueueingBasicConsumer(_consumerChannel);

                _consumerChannel.BasicConsume(_routingKey, false, consumer);

                var basicDeliverEventArgs = consumer.Queue.Dequeue();

                var bytes = basicDeliverEventArgs.Body;

                var queueMessage = _binarySerializer.Deserialize<QueueMessage>(bytes);

                Execute(queueMessage);

            }, _cancellationTokenSource.Token);

            comsumerTask.Start();
        }

        public void Execute(QueueMessage queueMessage)
        {
            var messageResult = _binarySerializer.Deserialize<MessageHandleResult>(queueMessage.Datas);

            var replyChannel = ReplyChannelPools.GetReplyChannel(messageResult.MessageId);

            if (replyChannel == null) return;

            replyChannel.SetResult(messageResult);
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}