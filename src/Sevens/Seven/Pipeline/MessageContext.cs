using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Seven.Message;

namespace Seven.Pipeline
{
    /// <summary>
    /// 消息处理上下文
    /// </summary>
    public class MessageContext : IMessageContext
    {
        public IModel Channel { get; private set; }

        public IMessage Message { get; private set; }

        public string RoutingKey { get; private set; }

        public string Topic { get; private set; }

        public ulong DeliveryTag { get; private set; }

        public MessageContext(IModel channel, IMessage message, string routingKey, string topic, ulong deliveryTag)
        {
            Channel = channel;
            Message = message;
            Topic = topic;
            DeliveryTag = deliveryTag;
            RoutingKey = routingKey;
        }
    }
}
