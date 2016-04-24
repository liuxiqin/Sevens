using System.Collections.Generic;
using System.Linq;
using Seven.Messages.Channels;

namespace Seven.Messages
{
    public class CommunicateChannelFactoryPool
    {
        private readonly IList<CommunicateChannelFactory> _channelPools;

        private readonly RemoteEndpoint _endpoint;

        public CommunicateChannelFactoryPool(RemoteEndpoint endpoint)
        {
            _endpoint = endpoint;

            _channelPools = new List<CommunicateChannelFactory>();
        }

        public ICommunicateChannel GetChannel(PublisherContext publisherContext)
        {
            var communicateChannel = default(ICommunicateChannel);

            for (var i = 0; i < _channelPools.Count; i++)
            {
                if (_channelPools[i].ContainsChannel(publisherContext))
                {
                    communicateChannel = _channelPools[i].GetChannel(publisherContext);
                    break;
                }
            }

            if (communicateChannel != null) return communicateChannel;

            return CreateChannel(publisherContext);
        }

        public ICommunicateChannel GetChannel(ConsumerContext consumerContext)
        {
            var communicateChannel = default(ICommunicateChannel);

            for (var i = 0; i < _channelPools.Count; i++)
            {
                if (_channelPools[i].ContainsChannel(consumerContext))
                {
                    communicateChannel = _channelPools[i].GetChannel(consumerContext);
                    break;
                }
            }

            if (communicateChannel != null) return communicateChannel;

            return CreateChannel(consumerContext);
        }

        private ICommunicateChannel CreateChannel(PublisherContext publisherContext)
        {
            if(!_channelPools.Any() || _channelPools.Last().IsFull())
                _channelPools.Add(new CommunicateChannelFactory(_endpoint));

            var channel = _channelPools.Last().GetChannel(publisherContext);

            return channel;
        }

        private ICommunicateChannel CreateChannel(ConsumerContext consumerContext)
        {
            if (!_channelPools.Any() || _channelPools.Last().IsFull())
                _channelPools.Add(new CommunicateChannelFactory(_endpoint));

            var channel = _channelPools.Last().GetChannel(consumerContext);

            return channel;
        }
    }
}