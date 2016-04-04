using System;
using Microsoft.SqlServer.Server;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;
using Seven.Infrastructure.Serializer;

namespace Seven.Message
{
    public class RequestChannel : IRequestChannel
    {
        private MessageChannelBase _messageChannel;

        private string _exChangeName = null;

        private IBinarySerializer _binarySerializer = null;

        public RequestChannel(string exchangeName, IBinarySerializer binarySerializer)
        {
            _exChangeName = exchangeName;

            _binarySerializer = binarySerializer;

            _messageChannel = new ProducterChannel(new ChannelInfo());
        }

        public RequestChannel(ChannelInfo channelInfo)
        {
            _messageChannel = new ProducterChannel(channelInfo);
        }

        public IReplyChannel SendMessage(QueueMessage message, TimeSpan timeout)
        {
            _messageChannel.SendMessage(message);

            return ReplyChannelPools.TryAddReplyChannel(message.MessageId, timeout);
        }

        public void SendMessageAsync(QueueMessage message)
        {
            _messageChannel.SendMessage(message);
        }
    }
}