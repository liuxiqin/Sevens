using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing;
using Seven.Infrastructure.Serializer;

namespace Seven.Message
{
    /// <summary>
    /// 消息生产者
    /// </summary>
    public class MessageProducer : IMessageProducter
    {
        private IBinarySerializer _binarySerializer;

        private IJsonSerializer _jsonSerializer;

        private readonly IMessageBroker _messageBroker;

        private TimeSpan _timeout = TimeSpan.FromSeconds(5);

        private readonly string _responseQueueName = "PRCRESPONSE";

        public MessageProducer(
           IMessageBroker messageBroker,
            IBinarySerializer binarySerializer,
            IJsonSerializer jsonSerializer)
        {
            _messageBroker = messageBroker;
            _binarySerializer = binarySerializer;
            _jsonSerializer = jsonSerializer;
        }

        public Task<MessageHandleResult> Publish<TMessage>(TMessage message, TimeSpan timeout) where TMessage : IMessage
        {
            return null;
        }
        public Task<MessageHandleResult> Publish<TMessage>(TMessage message) where TMessage : IMessage
        {
            var task = new Task<MessageHandleResult>(() =>
             {
                 using (var productChannel = _messageBroker.GetConnection.CreateModel())
                 {
                     var queueMessage = BuildMessage(message);

                     var responseChannel = _messageBroker.GetConnection.CreateModel();

                     productChannel.ExchangeDeclare(queueMessage.Topic, ExchangeType.Direct, true);

                     responseChannel.ExchangeDeclare(queueMessage.Topic, ExchangeType.Direct, true);
                     responseChannel.QueueDeclare(_responseQueueName, true, false, false, null);
                     responseChannel.QueueBind(_responseQueueName, queueMessage.Topic, message.MessageId);

                     var manualReset = new ManualResetEventSlim(false);

                     var handleResult = default(MessageHandleResult);

                     var responseConsumer = new EventingBasicConsumer(responseChannel);

                     responseConsumer.Received += (sender, args) =>
                     {
                         handleResult = _binarySerializer.Deserialize<MessageHandleResult>(args.Body);

                         ((EventingBasicConsumer)sender).Model.BasicAck(args.DeliveryTag, true);

                         manualReset.Set();
                     };

                     responseChannel.BasicConsume(_responseQueueName, false, responseConsumer);

                     productChannel.BasicPublish(message.GetType().FullName, message.GetType().FullName,
                         new BasicProperties()
                         {
                             DeliveryMode = 2,
                             ReplyTo = _responseQueueName,
                             CorrelationId = message.MessageId
                         },
                         _binarySerializer.Serialize(queueMessage));

                     var hasTimeouted = manualReset.Wait(_timeout);

                     if (!hasTimeouted)
                     {
                         responseChannel.BasicCancel(responseConsumer.ConsumerTag);

                         return new MessageHandleResult() { Message = "超时", Status = MessageStatus.Fail };
                     }

                     return handleResult;
                 }
             });

            task.Start();

            return task;
        }

        private QueueMessage BuildMessage<TMessage>(TMessage message) where TMessage : IMessage
        {
            var tag = message.GetType().FullName;
            var topic = message.GetType().FullName;

            var data = _jsonSerializer.Serialize(message);

            var queueMessage = new QueueMessage()
            {
                Tag = tag,
                Topic = topic,
                Datas = Encoding.UTF8.GetBytes(data),
                TypeName = message.GetType().FullName,
                MessageType = MessageType.Reply
            };

            return queueMessage;
        }

        public void PublishAsync<TMessage>(TMessage message) where TMessage : IMessage
        {
            return;
        }


        public void Response(string responseQueueName, string correlationId, MessageHandleResult handleResult)
        {
            using (var channel = _messageBroker.GetConnection.CreateModel())
            {
                channel.ExchangeDeclare(responseQueueName, ExchangeType.Direct, true);

                channel.BasicPublish(responseQueueName, responseQueueName, new BasicProperties() { DeliveryMode = 2 },
                    _binarySerializer.Serialize(handleResult));
            }
        }
    }
}
