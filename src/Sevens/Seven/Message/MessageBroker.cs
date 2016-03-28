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
    public class MessageBroker : IMessageBroker, IDisposable
    {
        private readonly ConnectionFactory _connectionFactory;

        private readonly IBinarySerializer _binarySerializer;

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

        public MessageBroker(IBinarySerializer binarySerializer, RabbitMqConnectionInfo connectionInfo)
        {
            _connectionFactory = new ConnectionFactory();
            _connectionFactory.HostName = connectionInfo.HostName;
            _connectionFactory.Port = connectionInfo.Port;
            _connectionFactory.UserName = connectionInfo.UserName;
            _connectionFactory.Password = connectionInfo.UserPaasword;

            this._binarySerializer = binarySerializer;
        }

        private IConnection Connection()
        {
            if (connection == null)
            {
                connection = _connectionFactory.CreateConnection();
            }
            return connection;
        }


        /// <summary>
        /// send message to queue
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(QueueMessage message)
        {
            if (!connection.IsOpen)
            {
                return;
            }
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(message.Topic, ExchangeType.Direct, true);

                var basicProperties = channel.CreateBasicProperties();
                basicProperties.DeliveryMode = 2; //消息持久化

                channel.BasicPublish(message.Topic, message.Tag, false, false, basicProperties, null);
            }
        }


        public void ReceiveMessage(string exchangeName, string routingKey, string exchangeType)
        {
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, true);
                channel.QueueDeclare(exchangeName, true, false, false, null);
                channel.QueueBind(exchangeName, exchangeName, exchangeName);

                var consumer = new QueueingBasicConsumer(channel);

                channel.BasicConsume(exchangeName, false, consumer);

                while (true)
                {
                    var queue = consumer.Queue.Dequeue();
                    var queueMessage = _binarySerializer.Deserialize<QueueMessage>(queue.Body);

                    Thread.Sleep(1);
                }
            }
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
