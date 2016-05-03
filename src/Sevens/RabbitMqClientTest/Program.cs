using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
using Seven.Infrastructure.Repository;
using Seven.Infrastructure.Serializer;
using Seven.Initializer;
using Seven.Messages;
using Seven.Messages.Channels;
using Seven.Messages.QueueMessages;
using Seven.Tests.UserSample.Commands;
using Seven.Events;
using Seven.Infrastructure.Dependency;
using Seven.Infrastructure.Snapshoting;
using Seven.Tests;

namespace RabbitMqClientTest
{
    internal class Program
    {
        private const string _mysqlConnection = "Database = sevens; Data Source = 127.0.0.1; User Id = root; Password = 123456; port = 3306";


        private static void Main(string[] args)
        {

            var containerBuilder = new AutofacContainerBuilder();

            var binarySerializer = new DefaultBinarySerializer();
            
            var eventHandleInitializer = new EventHandleProvider();
            eventHandleInitializer.Initialize(Assembly.GetExecutingAssembly(), Assembly.Load("Seven.Tests"));

            var commandInitializer = new CommandHandleProvider();
            commandInitializer.Initialize(Assembly.Load("Seven.Tests"));

            var messageTypeProvider = new MessageTypeProvider();
            messageTypeProvider.Initialize(Assembly.GetExecutingAssembly(), Assembly.Load("Seven.Tests"));

            containerBuilder.RegisterInstance(eventHandleInitializer);
            containerBuilder.RegisterInstance(commandInitializer);
            containerBuilder.RegisterInstance(messageTypeProvider);

            var mysqlEventStore = new MySqlEventStore(_mysqlConnection);

            var snapshotStorage = new MysqlSnapshotStorage(_mysqlConnection);

            var aggregateRootStorage = new MysqlAggregateRootStorage(_mysqlConnection);

            var aggregateRootMemory = new AggregateRootMemoryCache();

            var nonEventSouringRepository = new NonEventSouringRepository(aggregateRootStorage, binarySerializer);

            var eventSouringRepository = new EventSouringRepository(mysqlEventStore, snapshotStorage, binarySerializer,
                aggregateRootMemory);


            var endPoint = new RemoteEndpoint("127.0.0.1", "/", "guest", "guest", 5672);

            var exChangeName = typeof(CreateUserCommand).Assembly.GetName().Name; ;

            var responseRoutingKey = MessageUtils.CurrentResponseRoutingKey;

            var channelPools = new CommunicateChannelFactoryPool(endPoint);

            containerBuilder.RegisterInstance(channelPools);
            containerBuilder.RegisterInterface<IBinarySerializer, DefaultBinarySerializer>();

            var requestChannelPools = new RequestChannelPools();

            var commandTopicProvider = new UserTopicProvider();

            var eventTopicProvider = new UserEEventTopicProvider();

            var eventPublisher = new EventPublisher(requestChannelPools, eventTopicProvider);

            var commandProssor = new DefaultCommandProssor(mysqlEventStore, eventSouringRepository, commandInitializer, eventPublisher, snapshotStorage, binarySerializer);

            var messageHandler = new MessageRequestHandler(commandProssor);

            for (var i = 0; i < 5; i++)
            {
                var routingKey = string.Format("{0}_{1}_{2}", exChangeName, "command", i);

                var consumerContext = new ConsumerContext(exChangeName, routingKey, routingKey, responseRoutingKey, false, true);

                var consumer = new PushMessageConsumer(channelPools, binarySerializer, consumerContext, messageHandler);

                consumer.Start();

                Console.WriteLine("Started.");
            }


            for (var i = 0; i < 5; i++)
            {
                var routingKey = string.Format("{0}_{1}_{2}", exChangeName, "event", i);

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
