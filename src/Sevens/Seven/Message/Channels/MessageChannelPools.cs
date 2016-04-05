using System.Collections.Concurrent;

namespace Seven.Message.Channels
{
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
}