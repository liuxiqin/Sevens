using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Infrastructure.Serializer;

namespace Seven.Message
{
    public class RequestChannelPools
    {
        private static readonly ConcurrentDictionary<string, IRequestChannel> _channelPools =
            new ConcurrentDictionary<string, IRequestChannel>();

        private static object _lockObj = new object();

        public static IRequestChannel GetRequestChannel(
            string exChangeName,
            IMessageBroker messageBroker,
            IBinarySerializer binarySerializer)
        {
            if (!_channelPools.ContainsKey(exChangeName))
            {
                lock (_lockObj)
                {
                    if (!_channelPools.ContainsKey(exChangeName))
                    {
                        _channelPools.TryAdd(exChangeName,
                            new RequestChannel(exChangeName, messageBroker, binarySerializer));
                    }
                }
            }

            return _channelPools[exChangeName];
        }
    }
}