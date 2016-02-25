using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;

namespace Seven.Message
{
    /// <summary>
    /// 消息Broker
    /// </summary>
    public interface IMessageBroker : IDisposable
    {
        

        void Stop();

        IConnection GetConnection { get; }

        void SendMessage(QueueMessage message);

        void ReceiveMessage(string exchangeName, string routingKey, string exchangeType);
    }
}
