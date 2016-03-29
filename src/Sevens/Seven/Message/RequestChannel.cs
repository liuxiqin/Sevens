using System;
using Microsoft.SqlServer.Server;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;
using Seven.Infrastructure.Serializer;

namespace Seven.Message
{
    public class RequestChannel : IRequestChannel
    {
        private IModel _channel = null;

        private string _exChangeName = null;

        private IMessageBroker _broker { get; set; }

        private IBinarySerializer _binarySerializer = null;

        public RequestChannel(
            string exchangeName,
            IMessageBroker broker,
            IBinarySerializer binarySerializer)
        {
            _exChangeName = exchangeName;
            _broker = broker;
            _binarySerializer = binarySerializer;
        }

        public IReplyChannel SendMessage(QueueMessage message, TimeSpan timeout)
        {
            if (_channel == null || _channel.IsClosed)
            {
                _channel = _broker.GetConnection.CreateModel();

                _channel.ExchangeDeclare(_exChangeName, ExchangeType.Direct, true);
            }

            message.IsRpcInvoke = true;

            _channel.BasicPublish(_exChangeName, message.Tag, new BasicProperties() { DeliveryMode = 2 },
                _binarySerializer.Serialize(message));

            return ReplyChannelPools.TryAddReplyChannel(message.MessageId, timeout);
        }

        public void SendMessageAsync(QueueMessage message)
        {
            if (_channel == null || _channel.IsClosed)
            {
                _channel = _broker.GetConnection.CreateModel();

                _channel.ExchangeDeclare(_exChangeName, ExchangeType.Direct, true);
            }

            message.IsRpcInvoke = false;

            _channel.BasicPublish(_exChangeName, message.Tag, new BasicProperties() { DeliveryMode = 2 },
                _binarySerializer.Serialize(message));
        }
    }
}