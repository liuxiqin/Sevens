using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Infrastructure.Serializer;

namespace Seven.Messages.Channels
{
    public class RequestChannelPools
    {
        private static readonly ConcurrentDictionary<string, IRequestChannel> _channelPools =
            new ConcurrentDictionary<string, IRequestChannel>();

        private static object _lockObj = new object();

        public static IRequestChannel GetRequestChannel(RequestMessageContext channelInfo)
        {
            if (!_channelPools.ContainsKey(channelInfo.ToString()))
            {
                lock (_lockObj)
                {
                    if (!_channelPools.ContainsKey(channelInfo.ToString()))
                    {
                        _channelPools.TryAdd(channelInfo.ToString(), new RequestChannel(channelInfo));
                    }
                }
            }

            return _channelPools[channelInfo.ToString()];
        }
    }
}