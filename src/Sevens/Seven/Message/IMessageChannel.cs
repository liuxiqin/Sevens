using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing;
using Seven.Infrastructure.Serializer;

namespace Seven.Message
{
    public interface IMessageChannel
    {
        QueueMessage ReceiveMessage();

        void SendMessage(QueueMessage message);
    }

    public class MessageExchangeType
    {
        public const string Direct = "direct";
    }

    public class ChannelInfo
    {
        public string ExChangeName { get; set; }

        public string RoutingKey { get; set; }

        public bool ShouldPersistent { get; set; }

        public bool NoAck { get; set; }

        public string ExchangeType { get; set; }

        public RabbitMqConfiguration Configuation { get; set; }

        public override string ToString()
        {
            return string.Format("{0}_{1}", ExChangeName, RoutingKey);
        }
    }


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

    /// <summary>
    /// rabbitmq
    /// </summary>
    public class DefaultMessageChannel : IMessageChannel
    {
        private IMessageBroker _messageBroker = null;

        private IModel _channel = null;

        private ChannelInfo _channelInfo = null;

        private IBinarySerializer _binarySerializer;


        private IModel GetChannel()
        {
            if (_messageBroker == null || !_messageBroker.GetConnection.IsOpen)
                _messageBroker = new MessageBroker(_channelInfo.Configuation);

            if (_channel == null || _channel.IsClosed)
                _channel = _messageBroker.GetConnection.CreateModel();

            return _channel;
        }

        public DefaultMessageChannel(ChannelInfo channelInfo, IBinarySerializer binarySerializer)
        {
            _channelInfo = channelInfo;
            _binarySerializer = binarySerializer;
        }

        public QueueMessage ReceiveMessage()
        {
            var channel = GetChannel();

            return new QueueMessage();
        }

        public void SendMessage(QueueMessage message)
        {
            var channel = GetChannel();

            channel.BasicPublish(message.Topic, message.RoutingKey, new BasicProperties() { DeliveryMode = 2 },
                _binarySerializer.Serialize(message));
        }
    }

    public class ProducterChannel : MessageChannelBase
    {
        private ChannelInfo _channelInfo;

        public ProducterChannel(ChannelInfo channelInfo) : base(channelInfo.Configuation)
        {
            _channelInfo = channelInfo;
        }

        public override void BindConsumer() { }

        public override void BindElements()
        {
            _channel.ExchangeDeclare(_channelInfo.ExChangeName, ExchangeType.Direct, true);
        }
    }

    public class MessageChannelPools
    {
        private static ConcurrentDictionary<string, MessageChannelBase> _channels = null;

        static MessageChannelPools()
        {
            _channels = new ConcurrentDictionary<string, MessageChannelBase>();
        }

        public static MessageChannelBase GetMessageChannel(ChannelInfo channelInfo)
        {
            if (!_channels.ContainsKey(channelInfo.ToString()))
            {
                _channels.TryAdd(channelInfo.ToString(), new ProducterChannel(channelInfo));
            }

            return _channels[channelInfo.ToString()];
        }
    }

    public abstract class ConsumerChannel : MessageChannelBase
    {
        private ChannelInfo _channelInfo = null;

        public ConsumerChannel(ChannelInfo channelInfo) : base(channelInfo.Configuation)
        {
            _channelInfo = channelInfo;
        }

        public override void BindConsumer()
        {
            _consumer = new QueueingBasicConsumer(_channel);

            _channel.BasicConsume(_channelInfo.RoutingKey, false, _consumer);
        }

        public override void BindElements()
        {
            _channel.ExchangeDeclare(_channelInfo.ExChangeName, ExchangeType.Direct, true);

            _channel.QueueDeclare(_channelInfo.RoutingKey, true, false, false, null);

            _channel.QueueBind(_channelInfo.RoutingKey, _channelInfo.ExChangeName, _channelInfo.RoutingKey);
        }
    }

    /// <summary>
    /// RaabitMq推送消息消费通道
    /// </summary>
    public class PushConsumerChannel : ConsumerChannel
    {
        public PushConsumerChannel(ChannelInfo channelInfo) : base(channelInfo)
        {

        }
    }

    /// <summary>
    /// RaabitMq拉取消息消费通道
    /// </summary>
    public class PollConsumerChannel : ConsumerChannel
    {
        public PollConsumerChannel(ChannelInfo channelInfo) : base(channelInfo)
        {
        }
    }
}
