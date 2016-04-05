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
using Seven.Message.Channels;
using Seven.Message.QueueMessages;

namespace Seven.Message
{
    /// <summary>
    /// 消息生产者
    /// </summary>
    public class MessageProducer : IMessageProducter
    {
        private IBinarySerializer _binarySerializer;

        private TimeSpan _timeout = TimeSpan.FromSeconds(10);

        private readonly string _responseQueueName = "PRCRESPONSE";

        public MessageProducer(IBinarySerializer binarySerializer)
        {
            _binarySerializer = binarySerializer;
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

                var channel = MessageChannelPools.GetMessageChannel(new ChannelInfo());

                channel.SendMessage(queueMessage);

                var channelInfo = new ChannelInfo()
                {
                    ExChangeName = message.GetType().FullName,
                    ExchangeType = MessageExchangeType.Direct,
                    NoAck = false,
                    RoutingKey = message.GetType().FullName,
                    ShouldPersistent = true
                };

                var requestChannel = RequestChannelPools.GetRequestChannel(channelInfo);

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

            var data = _binarySerializer.Serialize(message);

            var queueMessage = new QueueMessage()
            {
                RoutingKey = tag,
                Topic = topic,
                Datas = data,
                TypeName = message.GetType().FullName,
                MessageType = MessageType.Reply
            };

            return queueMessage;
        }

        public void PublishAsync<TMessage>(TMessage message) where TMessage : IMessage
        {
            return;
        }
    }
}
