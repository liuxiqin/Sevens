using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommandHandlerTest;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Seven.Commands;
using Seven.Infrastructure.Ioc;
using Seven.Infrastructure.Serializer;
using Seven.Initializer;
using Seven.Message;
using Seven.Pipeline;

namespace RabbitMqClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var binarySerializer = new DefaultBinarySerializer();

            IJsonSerializer jsonSerializer = new DefaultJsonSerializer();

            ObjectContainer.SetContainer(new AutofacContainerObject());

            var eventHandleInitializer = new EventHandleProvider();
            eventHandleInitializer.Initialize(Assembly.GetExecutingAssembly(), Assembly.Load("CommandHandlerTest"));

            var commandInitializer = new CommandHandleProvider();
            commandInitializer.Initialize(Assembly.Load("CommandHandlerTest"));

            var messageTypeProvider = new MessageTypeProvider();
            messageTypeProvider.Initialize(Assembly.GetExecutingAssembly(), Assembly.Load("CommandHandlerTest"));

            ObjectContainer.RegisterInstance(eventHandleInitializer);
            ObjectContainer.RegisterInstance(commandInitializer);
            ObjectContainer.RegisterInstance(messageTypeProvider);

            var connectionInfo = new RabbitMqConnectionInfo("guest", "guest", "127.0.0.1", 5672);

            var broker = new MessageBroker(binarySerializer, connectionInfo);

            var messageHandler = new DefaultMessgaeExecute();

            var exchangeName = typeof(CreateUserCommand).ToString();

            var consumer = new MessageConsumer(broker, messageHandler, binarySerializer, jsonSerializer, exchangeName,
                exchangeName);

            Console.WriteLine("Begin to receive the message.");

            Task.Factory.StartNew(() =>
            {
                consumer.Start();
            });

            Console.ReadLine();
        }
    }
}

#region
//IBinarySerializer binarySerializer = new DefaultBinarySerializer();

//           var factory = new ConnectionFactory();
//           factory.HostName = "127.0.0.1";
//           factory.UserName = "guest";
//           factory.Password = "guest";
//           factory.Port = 5672;

//           using (var connection = factory.CreateConnection())
//           {
//               using (var channel = connection.CreateModel())
//               {
//                   channel.ExchangeDeclare("Command", ExchangeType.Direct, true);
//                   channel.QueueDeclare("UserCommand", true, false, false, null);
//                   channel.QueueBind("UserCommand", "Command", "Command");

//                   QueueingBasicConsumer consumer = new QueueingBasicConsumer(channel);

//                   channel.BasicConsume("UserCommand", false, consumer);

//                   while (true)
//                   {
//                       var queue = consumer.Queue.Dequeue();

//                       Console.WriteLine("waitting for get message from rabbitmq.");

//                       var applicationMessage = binarySerializer.Deserialize<ApplicationMessage>(queue.Body);

//                       var obj = binarySerializer.Deserialize(applicationMessage.Bodys);

//                       Console.WriteLine("receive the command {0}", obj.ToString());

//                       channel.BasicAck(queue.DeliveryTag, true);

//                       Thread.Sleep(1000);
//                   }
//               }
//           }
#endregion