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
using Seven.Infrastructure.UniqueIds;
using Seven.Messages.Channels;
using Seven.Messages.QueueMessages;

namespace Seven.Messages
{
    /// <summary>
    /// 消息生产者
    /// </summary>
    public class MessageProducer : IMessageProducter
    {
        private IBinarySerializer _binarySerializer;

        private TimeSpan _timeout = TimeSpan.FromSeconds(10);

        private readonly string _responseQueueName = "PRCRESPONSE";

        private RabbitMqConfiguration _configuration = null;

        public MessageProducer(IBinarySerializer binarySerializer, RabbitMqConfiguration configuration)
        {
            _binarySerializer = binarySerializer;
            _configuration = configuration;
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

                var requestMessageContext = new RequestMessageContext()
                {
                    Configuation = _configuration,
                    ExChangeName = queueMessage.Topic,
                    ExchangeType = MessageExchangeType.Direct,
                    NoAck = false,
                    RoutingKey = queueMessage.RoutingKey,
                    ShouldPersistent = true
                };
 
                var requestChannel = RequestChannelPools.GetRequestChannel(requestMessageContext);

                var replyChannel = requestChannel.SendMessage(queueMessage, _timeout);

                return replyChannel.GetResult();

            });

            task.Start();

            return task;
        }

        private QueueMessage BuildMessage<TMessage>(TMessage message) where TMessage : IMessage
        {
            var routingKey = message.GetType().FullName;
            var topic = message.GetType().FullName;
             
            var queueMessage = new QueueMessage()
            {
                MessageId = ObjectId.NewObjectId(),
                RoutingKey = routingKey,
                Topic = topic,
                Message = message,
                TypeName = message.GetType().FullName,
                MessageType = MessageType.Reply,
                ResponseRoutingKey = MessageUtils.CurrentResponseRoutingKey
            };

            return queueMessage;
        }

        public void PublishAsync<TMessage>(TMessage message) where TMessage : IMessage
        {
            return;
        }

        public void PublishAsync(QueueMessage message)
        {
            var channel = MessageChannelPools.GetMessageChannel(new RequestMessageContext());

            channel.SendMessage(message);
        }
    }
}
