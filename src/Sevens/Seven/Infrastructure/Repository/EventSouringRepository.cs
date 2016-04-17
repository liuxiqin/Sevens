using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Seven.Aggregates;
using Seven.Events;
using Seven.Infrastructure.EventStore;
using Seven.Infrastructure.Serializer;
using Seven.Infrastructure.Snapshoting;

namespace Seven.Infrastructure.Repository
{
    public class EventSouringRepository : IRepository
    {
        private readonly IEventStore _eventStore;

        private ISnapshotStorage _snapshotStorage;

        private readonly IBinarySerializer _binarySerializer;

        public EventSouringRepository(IEventStore eventStore, ISnapshotStorage snapshotStorage,
            IBinarySerializer binarySerializer)
        {
            _eventStore = eventStore;
            _snapshotStorage = snapshotStorage;
            _binarySerializer = binarySerializer;
        }

        public void Add(IAggregateRoot aggregateRoot)
        {

        }

        public TAggregateRoot Get<TAggregateRoot>(string aggregateRootId) where TAggregateRoot : IAggregateRoot
        {
            var snapshot = _snapshotStorage.GetLastestSnapshot(aggregateRootId).Result;

            var aggregateRoot = default(TAggregateRoot);

            if (snapshot != null)
            {
                aggregateRoot = (TAggregateRoot)ConvertTo(snapshot.Datas);
            }

            if (aggregateRoot == null)
            {
                aggregateRoot = (TAggregateRoot)FormatterServices.GetUninitializedObject(typeof(TAggregateRoot));
            }

            var eventStreamRecord = _eventStore.LoadEventStream(aggregateRootId, aggregateRoot.Version);

            var changgEvents = ConvertTo(eventStreamRecord);

            aggregateRoot.ApplyEvents(changgEvents.Events);

            return aggregateRoot;
        }

        private DomainEventStream ConvertTo(EventStreamRecord streamRecord)
        {
            var changeEvents = _binarySerializer.Deserialize<IList<IEvent>>(streamRecord.EventDatas);

            return new DomainEventStream(streamRecord.AggregateRootId, streamRecord.Version, streamRecord.CommandId,
                changeEvents);
        }


        private IAggregateRoot ConvertTo(byte[] aggregateRootDatas)
        {
            return _binarySerializer.Deserialize<IAggregateRoot>(aggregateRootDatas);
        }


    }
}
