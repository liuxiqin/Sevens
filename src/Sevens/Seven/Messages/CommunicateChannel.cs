using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;
using Seven.Messages.Channels;

namespace Seven.Messages
{
    public class CommunicateChannel : ICommunicateChannel
    {
        private IMessageConnection _connection;

        private readonly bool _isConsumer;

        private IModel _channel;

        private PublisherContext _publisherContext;

        private ConsumerContext _consumerContext;

        private QueueingBasicConsumer _basicConsumer;

        public CommunicateChannel(IMessageConnection connection, PublisherContext publisherContext)
        {
            _connection = connection;

            _publisherContext = publisherContext;

            PublisherBind(publisherContext);
        }

        public CommunicateChannel(IMessageConnection connection, ConsumerContext consumerContext)
        {
            _connection = connection;

            _consumerContext = consumerContext;

            ConsumerBind(consumerContext);
        }

        private void PublisherBind(PublisherContext publisherContext)
        {
            _channel = _connection.CreateChannel();

            _channel.ExchangeDeclare(publisherContext.ExchangeName, publisherContext.ExchangeType, true);
        }

        private void ConsumerBind(ConsumerContext consumerContext)
        {
            _channel = _connection.CreateChannel();

            _channel.ExchangeDeclare(consumerContext.ExChangeName, consumerContext.ExchangeType, true);

            _channel.QueueDeclare(consumerContext.QueueName, consumerContext.Durable,
                !consumerContext.Durable, !consumerContext.Durable, null);

            _channel.QueueBind(_consumerContext.QueueName, _consumerContext.ExChangeName, consumerContext.RoutingKey);

            _basicConsumer = new QueueingBasicConsumer(_channel);

            _channel.BasicQos(0, 100, true);

            _channel.BasicConsume(consumerContext.QueueName, consumerContext.NoAck, _basicConsumer);
        }

        public void Send(SendMessage messsage)
        {
            if (_publisherContext == null)
                throw new ApplicationException("The current channel can not publish  message.");

            var properties = new BasicProperties() { DeliveryMode = 2 };

            if (!_publisherContext.Durable)
                properties.DeliveryMode = 1;

            _channel.BasicPublish(_publisherContext.ExchangeName, messsage.RoutingKey, properties, messsage.ByteDatas);
        }

        public ReceiveMessage Receive()
        {
            if (_consumerContext == null)
                throw new ApplicationException("The current channel can not receive message.");

            if (_basicConsumer == null)
                throw new ApplicationException("basicConsumer can not be null.");

            var deliverEventArgs = _basicConsumer.Queue.Dequeue();

            if (deliverEventArgs == null)
                return null;

            return new ReceiveMessage(deliverEventArgs.Body, deliverEventArgs.DeliveryTag, deliverEventArgs.Redelivered);
        }

        public void BasicAck(ulong deliveryTag)
        {
            _channel.BasicAck(deliveryTag, true);
        }
    }
}