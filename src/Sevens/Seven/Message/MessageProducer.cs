using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing;
using Seven.Infrastructure.Serializer;

namespace Seven.Message
{
    /// <summary>
    /// 消息生产者
    /// </summary>
    public class MessageProducer : IMessageProducter
    {
        private IBinarySerializer _binarySerializer;

        private IJsonSerializer _jsonSerializer;

        private readonly IMessageBroker _messageBroker;

        public MessageProducer(
           IMessageBroker messageBroker,
            IBinarySerializer binarySerializer,
            IJsonSerializer jsonSerializer)
        {
            _messageBroker = messageBroker;
            _binarySerializer = binarySerializer;
            _jsonSerializer = jsonSerializer;
        }

        public Task<MessageHandleResult> Publish<TMessage>(TMessage message) where TMessage : IMessage
        {
            var task = new Task<MessageHandleResult>(() =>
             {
                 using (var channel = _messageBroker.GetConnection.CreateModel())
                 {
                     var tag = message.GetType().FullName;
                     var topic = message.GetType().FullName;

                     var data = _jsonSerializer.Serialize(message);

                     var queueMessage = new QueueMessage()
                     {
                         Tag = tag,
                         Topic = topic,
                         Datas = Encoding.UTF8.GetBytes(data),
                         TypeName = typeof(TMessage).FullName,
                         MessageType = MessageType.Reply
                     };

                     channel.ExchangeDeclare(queueMessage.Topic, ExchangeType.Direct, true);

                     channel.QueueDeclare("Rpc_Response", true, false, false, null);

                     channel.QueueBind(queueMessage.Topic, "Rpc_Response", message.MessageId);

                     var resultConsumer = new EventingBasicConsumer(channel);

                     var manualReset = new ManualResetEventSlim(false);

                     var handleResult = default(MessageHandleResult);

                     resultConsumer.Received += (sender, args) =>
                     {
                         handleResult = _binarySerializer.Deserialize<MessageHandleResult>(args.Body);

                         manualReset.Set();
                     };

                     channel.BasicConsume("Rpc_Response", true, resultConsumer);


                     channel.BasicPublish(topic, tag,
                         new BasicProperties()
                         {
                             DeliveryMode = 2,
                             ReplyTo = "Rpc_Response",
                             CorrelationId = message.MessageId
                         },
                         _binarySerializer.Serialize(queueMessage));


                     manualReset.Wait();

                     return handleResult;
                 }
             });

            task.Start();

            return task;
        }

        public void PublishAsync<TMessage>(TMessage message) where TMessage : IMessage
        {
            return;
        }


        public void Response(string responseQueueName, string correlationId, MessageHandleResult handleResult)
        {
            using (var channel = _messageBroker.GetConnection.CreateModel())
            {
                channel.ExchangeDeclare(responseQueueName, ExchangeType.Direct, true);

                channel.BasicPublish(responseQueueName, responseQueueName, new BasicProperties() { DeliveryMode = 2 },
                    _binarySerializer.Serialize(handleResult));
            }
        }
    }
}
