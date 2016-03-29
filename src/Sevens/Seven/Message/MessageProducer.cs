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

        private TimeSpan _timeout = TimeSpan.FromSeconds(10);

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
                var queueMessage = BuildMessage(message);

                var requestChannel = RequestChannelPools.GetRequestChannel(queueMessage.Topic, _messageBroker, _binarySerializer);

                var replyChannel = requestChannel.SendMessage(queueMessage, _timeout);

                return replyChannel.GetResult();

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
