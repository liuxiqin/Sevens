using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Seven.Commands;
using Seven.Extension.Persistence;
using Seven.Infrastructure.Ioc;
using Seven.Infrastructure.Repository;
using Seven.Infrastructure.Snapshoting;
using Seven.Initializer;
using MySql.Data.MySqlClient;
using Seven.Events;
using Seven.Infrastructure.EventStore;
using Seven.Infrastructure.Serializer;
using SevenTest.UserSample.Commands;

namespace SevenTest
{
    public class Program
    {
        private const string _mysqlConnection = "Database = sevens; Data Source = 127.0.0.1; User Id = root; Password = 123456; port = 3306";

        private static void Main(string[] args)
        {

            ObjectContainer.SetContainer(new AutofacContainerObject());

            var applictionInitializer = new EventHandleProvider();

            applictionInitializer.Initialize(Assembly.GetExecutingAssembly());

            var commandInitializer = new CommandHandleProvider();

            commandInitializer.Initialize(Assembly.GetExecutingAssembly());

            ObjectContainer.RegisterInstance(applictionInitializer);
            ObjectContainer.RegisterInstance(commandInitializer);


            var dbConnection = new MySqlConnection(_mysqlConnection);

            var persistence = new SnapshotPersistence(dbConnection);

            var binarySerializer = new DefaultBinarySerializer();

            var snapshotRepository = new SnapshotRepository(persistence, new SnapshotFactory(binarySerializer));

            var eventPersistence = new EventStorePersistence(dbConnection);

            var eventFactory = new EventStreamFactory(binarySerializer);

            var eventStore = new EventStore(eventPersistence, eventFactory);


            IRepository repository = new EventSouringRepository(eventStore, snapshotRepository);

            var comamndHandler = ObjectContainer.Resolve<CommandHandleProvider>();
   
            var changePasswordCommand = new ChangePasswordCommand("90ca0d59-65e6-403b-82c5-8df967cc8e22", "2222222", "11111");

            var commandContext = new CommandContext(repository);

            var commandHanldeAction = comamndHandler.GetInternalCommandHandle(typeof(ChangePasswordCommand));
            commandHanldeAction(commandContext, changePasswordCommand);

            var aggregateRoots = commandContext.AggregateRoots;

            IList<IEvent> unCommitEvents = null;

            foreach (var item in aggregateRoots)
            {
                unCommitEvents = item.Value.Commit();
            }

            var aggregateRoot = aggregateRoots.FirstOrDefault().Value;

            eventStore.AppendToStream(aggregateRoot.AggregateRootId, new EventStream(aggregateRoot.Version, unCommitEvents));

            snapshotRepository.Create(aggregateRoot);
            
            Console.WriteLine("改方法执行完毕...");
        }
    }
}