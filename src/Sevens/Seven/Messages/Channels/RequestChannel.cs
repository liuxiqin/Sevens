using System;
using Microsoft.SqlServer.Server;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;
using Seven.Infrastructure.Serializer;
using Seven.Messages.QueueMessages;
using System.Threading.Tasks;

namespace Seven.Messages.Channels
{
    public class RequestChannel : IRequestChannel
    {
        private readonly RequestMessageContext _requestMessageContext;

        private readonly CommunicateChannelFactoryPool _channelFactoryPool;

        private readonly IBinarySerializer _binarySerializer;

        public RequestChannel(
            CommunicateChannelFactoryPool channelFactoryPool,
            RequestMessageContext requestMessageContext,
            IBinarySerializer binarySerializer)
        {
            _requestMessageContext = requestMessageContext;

            _channelFactoryPool = channelFactoryPool;

            _binarySerializer = binarySerializer;
        }


        public Task<IReplyChannel> SendMessage(MessageWrapper message, int timeout)
        {
            return Task.Run(() =>
            {
                var channel =
                    _channelFactoryPool.GetChannel(
                        new PublisherContext(
                            _requestMessageContext.ExChangeName,
                            _requestMessageContext.ExchangeType,
                            _requestMessageContext.Durable,
                            _requestMessageContext.NoAck,
                            _requestMessageContext.NoAck));

                var byteDatas = _binarySerializer.Serialize(message);

                channel.Send(new SendMessage(byteDatas, message.RoutingKey));

                return ReplyChannelPools.TryAddReplyChannel(message.MessageId, TimeSpan.FromSeconds(timeout));
            });
        }

        public Task SendMessageAsync(MessageWrapper message)
        {
            return Task.Run(() =>
            {
                var channel =
                    _channelFactoryPool.GetChannel(
                        new PublisherContext(
                            _requestMessageContext.ExChangeName,
                            _requestMessageContext.ExchangeType,
                            _requestMessageContext.Durable,
                            _requestMessageContext.NoAck,
                            _requestMessageContext.NoAck));

                var byteDatas = _binarySerializer.Serialize(message);

                channel.Send(new SendMessage(byteDatas, message.RoutingKey));
            });
        }
    }
}