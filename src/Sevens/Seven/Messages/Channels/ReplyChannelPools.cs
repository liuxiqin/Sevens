﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Seven.Infrastructure.Exceptions;

namespace Seven.Messages.Channels
{
    public class ReplyChannelPools
    {
        private static readonly ConcurrentDictionary<string, IReplyChannel> _channelPools =
            new ConcurrentDictionary<string, IReplyChannel>();

        public static IReplyChannel GetReplyChannel(string messageId)
        {
            if (_channelPools.ContainsKey(messageId))
            {
                var replyChannel = default(IReplyChannel);

                if (_channelPools.TryRemove(messageId, out replyChannel))
                    return replyChannel;
            }

            return null;
        }


        public static IReplyChannel TryAddReplyChannel(string messageId, TimeSpan timeout)
        {
            var replyChannel = new ReplyChannel(messageId, timeout);

            if (_channelPools.TryAdd(messageId, replyChannel))
            {
                return replyChannel;
            }

            throw new FrameworkException("can not add the ReplyChannel in the ReplyChannelPools");
        }
    }
}
