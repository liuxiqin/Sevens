using System;
using System.Diagnostics;
using Seven.Infrastructure.Dependency;
using Seven.Infrastructure.Serializer;
using Seven.Messages.QueueMessages;
using Seven.Tests.UserSample.Commands;

namespace Seven.Tests
{
    public class Program
    {
        private const string _mysqlConnection = "Database = sevens; Data Source = 127.0.0.1; User Id = root; Password = 123456; port = 3306";

        private static void Main(string[] args)
        {

           // ObjectContainer.SetContainer(new AutofacContainerObject());

            //var applictionInitializer = new EventHandleProvider();

            //applictionInitializer.Initialize(Assembly.GetExecutingAssembly());

            //var commandInitializer = new CommandHandleProvider();

            //commandInitializer.Initialize(Assembly.GetExecutingAssembly());

            //ObjectContainer.RegisterInstance(applictionInitializer);
            //ObjectContainer.RegisterInstance(commandInitializer);


            //var dbConnection = new MySqlConnection(_mysqlConnection);

            //var persistence = new SnapshotPersistence(dbConnection);

            //var binarySerializer = new DefaultBinarySerializer();

            //var snapshotRepository = new SnapshotRepository(persistence, new SnapshotFactory(binarySerializer));

            //var eventPersistence = new EventStorePersistence(dbConnection);

            //var eventFactory = new EventStreamFactory(binarySerializer);

            //var eventStore = new EventStore(eventPersistence, eventFactory);


            //IRepository repository = new EventSouringRepository(eventStore, snapshotRepository);

            //var comamndHandler = ObjectContainer.Resolve<CommandHandleProvider>();

            var changePasswordCommand = new ChangePasswordCommand("90ca0d59-65e6-403b-82c5-8df967cc8e22", "2222222",
                "11111");

            //var commandContext = new CommandContext(repository);

            //var commandHanldeAction = comamndHandler.GetInternalCommandHandle(typeof(ChangePasswordCommand));
            //commandHanldeAction(commandContext, changePasswordCommand);

            //var aggregateRoots = commandContext.AggregateRoots;

            //IList<IEvent> unCommitEvents = null;

            //foreach (var item in aggregateRoots)
            //{
            //    unCommitEvents = item.Value.Commit();
            //}

            //var aggregateRoot = aggregateRoots.FirstOrDefault().Value;

            //eventStore.AppendToStream(aggregateRoot.AggregateRootId, new EventStream(aggregateRoot.Version, unCommitEvents));

            //snapshotRepository.Create(aggregateRoot);

            //Console.WriteLine("改方法执行完毕...");

            // TestBinSer();

            //var messageHandleResult = new MessageHandleResult()
            //{
            //    Message = "订单那等你给当地发给你",
            //    Status = MessageStatus.Success,
            //    MessageId = Guid.NewGuid().ToString(),
            //};

            IBinarySerializer binarySerializer = new DefaultBinarySerializer();

            var queueMessage = new MessageWrapper()
            {
                Message = changePasswordCommand,
            };

            var queues = binarySerializer.Serialize(queueMessage);

            var queueMessage1 = binarySerializer.Deserialize<MessageWrapper>(queues);


            for (var i = 0; i < 1000000; i++)
            {
                // Console.WriteLine(ObjectId.NewObjectId());
            }

            Console.Write("MachineName:{0},ProcessId:{1}", Environment.MachineName, Process.GetCurrentProcess().Id);

        }


        public static void TestBinSer()
        {
            var changePasswordCommand = new ChangePasswordCommand("90ca0d59-65e6-403b-82c5-8df967cc8e22", "2222222", "11111");

            var message = new MessageWrapper();

            message.ExchangeName = changePasswordCommand.GetType().FullName;
            message.TypeName = changePasswordCommand.GetType().FullName;

            var binarySerializer = new DefaultBinarySerializer();

        }
    }
}