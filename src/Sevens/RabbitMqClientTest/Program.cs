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
using RabbitMQ.Client.Events;

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

            var commandProssor = new DefaultCommandProssor(new MySqlEventStore(""), null, commandInitializer, null,
                binarySerializer);

            var endPoint = new RemoteEndpoint("127.0.0.1", "/", "guest", "guest", 5672);

            var exChangeName = typeof(CreateUserCommand).Namespace;

            var responseRoutingKey = MessageUtils.CurrentResponseRoutingKey;

            var channelPools = new CommunicateChannelFactoryPool(endPoint);

            var messageHandler = new MessageRequestHandler(commandProssor);

            ObjectContainer.RegisterInstance(channelPools);
            ObjectContainer.RegisterInterface<IBinarySerializer, DefaultBinarySerializer>();

            for (var i = 0; i < 2; i++)
            {
                var routingKey = string.Format("{0}_{1}_{2}", exChangeName, "command", i);

                var consumerContext = new ConsumerContext(exChangeName, routingKey, routingKey, responseRoutingKey, false, true);

                var consumer = new PushMessageConsumer(channelPools, binarySerializer, consumerContext, messageHandler);

                consumer.Start();

                Console.WriteLine("Started.");
            }

            Console.WriteLine("begin to consumer the message.");

            Console.ReadLine();
        }
    }

    public static class ChannelFactory
    {
        public static CommunicateChannelFactoryPool ChannelPools;

        public static void SetChannelPools(RemoteEndpoint endpoint)
        {
            ChannelPools = new CommunicateChannelFactoryPool(endpoint);
        }
    }
}
