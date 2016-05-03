using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Infrastructure.Dependency;
using Seven.Infrastructure.Serializer;

namespace Seven.Messages.Channels
{
    public class RequestChannelPools
    {
        private readonly ConcurrentDictionary<string, IRequestChannel> _channelPools =
            new ConcurrentDictionary<string, IRequestChannel>();

        private object _lockObj = new object();

        private IBinarySerializer _binarySerializer;

        private CommunicateChannelFactoryPool _channelFactoryPools;

        public RequestChannelPools()
        {
            _channelFactoryPools = ObjectContainer.Resolve<CommunicateChannelFactoryPool>();
            _binarySerializer = ObjectContainer.Resolve<IBinarySerializer>();
        }

        public IRequestChannel GetRequestChannel(RequestMessageContext messageContext)
        {
            if (!_channelPools.ContainsKey(messageContext.ToString()))
            {
                lock (_lockObj)
                {
                    if (!_channelPools.ContainsKey(messageContext.ToString()))
                    {
                        _channelPools.TryAdd(messageContext.ToString(), new RequestChannel(_channelFactoryPools, messageContext, _binarySerializer));
                    }
                }
            }

            return _channelPools[messageContext.ToString()];
        }
    }
}