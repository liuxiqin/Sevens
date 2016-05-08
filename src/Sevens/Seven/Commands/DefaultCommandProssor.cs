using System;
using System.Linq;
using Seven.Aggregates;
using Seven.Events;
using Seven.Infrastructure.EventStore;
using Seven.Infrastructure.Repository;
using Seven.Infrastructure.Serializer;
using Seven.Infrastructure.Snapshoting;
using Seven.Initializer;

namespace Seven.Commands
{
    public class DefaultCommandProssor : ICommandProssor
    {
        private readonly IEventStore _eventStore;

        private readonly IRepository _repository;

        private readonly CommandHandleProvider _commandHandleProvider;

        private readonly IEventPublisher _eventPublisher;

        private readonly IBinarySerializer _binarySerializer;

        private readonly ISnapshotStorage _snapshotStorage;

        public DefaultCommandProssor(
            IEventStore eventStore,
            IRepository repository,
            CommandHandleProvider commandHandleProvider,
            IEventPublisher eventPublisher,
            ISnapshotStorage snapshotStorage,
            IBinarySerializer binarySerializer)
        {
            _eventStore = eventStore;
            _repository = repository;
            _commandHandleProvider = commandHandleProvider;
            _eventPublisher = eventPublisher;
            _binarySerializer = binarySerializer;
            _snapshotStorage = snapshotStorage;
        }


        public void Execute(ICommand command)
        {
            var commandHandler = _commandHandleProvider.GetInternalCommandHandle(command.GetType());

            var commandContext = new CommandContext(_repository);

            commandHandler(commandContext, command);

            var aggregateRoots = commandContext.AggregateRoots;

            if (aggregateRoots.Count > 1)
                throw new Exception("one command handler can change just only one aggregateRoot.");

            var aggregateRoot = aggregateRoots.First().Value;

            var domainEvents = aggregateRoot.GetUnCommitEvents();

            var eventStream = BuildEventStream(aggregateRoot, command.CommandId);

            _eventStore.AppendAsync(eventStream);

            if (aggregateRoot.Version % 3 == 0)
                _snapshotStorage.Create(new SnapshotRecord(aggregateRoot.AggregateRootId, aggregateRoot.Version,
                    _binarySerializer.Serialize(aggregateRoot)));

            _eventPublisher.PublishAsync(domainEvents);

            aggregateRoot.Clear();

            Console.WriteLine(aggregateRoot.ToString());

            Console.WriteLine("DomainEvents count is {0}", domainEvents.Count);
        }

        private EventStreamRecord BuildEventStream(IAggregateRoot aggregateRoot, string commandId)
        {
            return new EventStreamRecord()
            {
                AggregateRootId = aggregateRoot.AggregateRootId,
                CommandId = commandId,
                Version = aggregateRoot.Version,
                EventDatas = _binarySerializer.Serialize(aggregateRoot.GetUnCommitEvents())
            };
        }
    }
}