﻿using System;
using Newtonsoft.Json;
using Seven.Infrastructure.Serializer;
using Seven.Messages.Channels;
using Seven.Messages.QueueMessages;

namespace Seven.Messages.Pipelines
{
    public class ResponseMessageHandler : IMessageHandler
    {
        public void Handle(MessageContext context)
        {
            var routingKey = "RpcResponseQueue";

            var channelInfo = new RequestMessageContext()
            {
                Configuation = context.ChannelInfo.Configuation,
                ExChangeName = context.ChannelInfo.ExChangeName,
                ExchangeType = MessageExchangeType.Direct,
                NoAck = true,
                RoutingKey = routingKey,
                ShouldPersistent = false,
            };


            var reply = MessageChannelPools.GetMessageChannel(channelInfo);

            reply.SendMessage(context.GetResponse());

            Console.WriteLine("response the message {0}.",JsonConvert.SerializeObject(context.Response));
        }
    }
}
