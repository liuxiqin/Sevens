using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Seven.Message
{
    public static class ReplyChannelPools
    {
        private static readonly ConcurrentDictionary<string, IReplyChannel> _channelPools = new ConcurrentDictionary<string, IReplyChannel>();

        public static IReplyChannel GetReplyChannel(string messageId)
        {
            if (_channelPools.ContainsKey(messageId))
            {
                var replyChannel = default(IReplyChannel);

                _channelPools.TryRemove(messageId, out replyChannel);
            }

            return null;
        }


        public static bool TryAddReplyChannel(string messageId)
        {
            return _channelPools.TryAdd(messageId, new ReplyChannel(messageId, TimeSpan.FromSeconds(10)));
        }
    }
}
