using System.Collections.Concurrent;
using System.Collections.Generic;
using Seven.Messages.Channels;

namespace Seven.Messages
{
    public class CommunicateChannelFactory
    {
        private IMessageConnection _connection;

        private ConcurrentDictionary<string, ICommunicateChannel> _channelPools;

        private readonly int _channelPoolLength = 16;

        private readonly RemoteEndpoint _endpoint;

        private readonly object _lockObj = new object();
         
        public CommunicateChannelFactory(RemoteEndpoint endpoint)
        {
            _endpoint = endpoint;

            _channelPools = new ConcurrentDictionary<string, ICommunicateChannel>();
        }

        public ICommunicateChannel GetChannel(PublisherContext publisherContext)
        {
            CheckConnection();

            if (!_channelPools.ContainsKey(publisherContext.ExchangeName))
            {
                var channel = new CommunicateChannel(_connection, publisherContext);

                _channelPools.TryAdd(publisherContext.ExchangeName, channel);
            }
            return _channelPools[publisherContext.ExchangeName];
        }

        public ICommunicateChannel GetChannel(ConsumerContext consumerContext)
        {
            CheckConnection();

            if (!_channelPools.ContainsKey(consumerContext.GetConsumerKey()))
            {
                var channel = new CommunicateChannel(_connection, consumerContext);

                _channelPools.TryAdd(consumerContext.GetConsumerKey(), channel);

            }
            return _channelPools[consumerContext.GetConsumerKey()];
        }

        public bool ContainsChannel(PublisherContext publisherContext)
        {
            return _channelPools.ContainsKey(publisherContext.ExchangeName);
        }

        public bool ContainsChannel(ConsumerContext consumerContext)
        {
            return _channelPools.ContainsKey(consumerContext.GetConsumerKey());
        }

        public bool IsFull()
        {
            return _channelPools.Count >= _channelPoolLength;
        }

        private void CheckConnection()
        {

            if (_connection == null || !_connection.IsOpen())
            {
                lock (_lockObj)
                {
                    if (_connection == null || !_connection.IsOpen())
                        _connection = new MessageConnection(_endpoint);
                }
            }
        }
    }
}