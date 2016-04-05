using System;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing;
using RabbitMQ.Client.Framing.Impl;
using RabbitMQ.Client.MessagePatterns;
using Seven.Infrastructure.MessageDevice;
using Seven.Infrastructure.Serializer;

namespace Seven.Message
{
    /// <summary>
    /// 消息Broker
    /// </summary>
    public class MessageBroker : IMessageBroker
    {
        private readonly ConnectionFactory _connectionFactory;

        private EventHandler connectioneSuccessed;
        private EventHandler connectionFailed;
        private IConnection connection;
        public IConnection GetConnection
        {
            get
            {
                if (connection == null)
                {
                    connection = _connectionFactory.CreateConnection();
                }

                return connection;
            }
        }

        public MessageBroker(RabbitMqConfiguration config)
        {
            _connectionFactory = new ConnectionFactory();
            _connectionFactory.HostName = config.HostName;
            _connectionFactory.Port = config.Port;
            _connectionFactory.UserName = config.UserName;
            _connectionFactory.Password = config.UserPaasword;
            _connectionFactory.VirtualHost = config.VirtualName;

        }

        private IConnection Connection()
        {
            if (connection == null)
            {
                connection = _connectionFactory.CreateConnection();
            }
            return connection;
        }
         
        public void Stop()
        {
            if (connection.IsOpen)
            {
                connection.Close();
            }
        }
         

        public void Dispose()
        {
            Stop();
        }
    }
}
