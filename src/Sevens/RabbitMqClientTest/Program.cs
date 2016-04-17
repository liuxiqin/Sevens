using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Seven.Commands;
using Seven.Infrastructure.EventStore;
using Seven.Infrastructure.IocContainer;
using Seven.Infrastructure.Repository;
using Seven.Infrastructure.Serializer;
using Seven.Initializer;
using Seven.Messages;
using Seven.Messages.Channels;
using Seven.Messages.QueueMessages;
using Seven.Tests.UserSample.Commands;

namespace RabbitMqClientTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var binarySerializer = new DefaultBinarySerializer();

            ObjectContainer.SetContainer(new AutofacContainerObject());

            var eventHandleInitializer = new EventHandleProvider();
            eventHandleInitializer.Initialize(Assembly.GetExecutingAssembly(), Assembly.Load("Seven.Tests"));

            var commandInitializer = new CommandHandleProvider();
            commandInitializer.Initialize(Assembly.Load("Seven.Tests"));

            var messageTypeProvider = new MessageTypeProvider();
            messageTypeProvider.Initialize(Assembly.GetExecutingAssembly(), Assembly.Load("Seven.Tests"));

            ObjectContainer.RegisterInstance(eventHandleInitializer);
            ObjectContainer.RegisterInstance(commandInitializer);
            ObjectContainer.RegisterInstance(messageTypeProvider);

            var commandProssor = new DefaultCommandProssor(new MySqlEventStore(""), null, commandInitializer, null, binarySerializer);

            var configuration = new RabbitMqConfiguration()
            {
                HostName = "127.0.0.1",
                Port = 5672,
                UserName = "guest",
                UserPaasword = "guest",
                VirtualName = "/"
            };

            Console.WriteLine("begin to receive the message.");

            var consumer = new PushMessageConsumer(new RequestMessageContext()
            {
                Configuation = configuration,
                ExChangeName = typeof(CreateUserCommand).Namespace,
                ExchangeType = MessageExchangeType.Direct,
                NoAck = false,
                RoutingKey = typeof(CreateUserCommand).FullName,
                ShouldPersistent = true
            }, new MessageRequestHandler(commandProssor));

            consumer.Start();

            Console.WriteLine("begin to consumer the message.");

            Console.ReadLine();
        }
    }
}
