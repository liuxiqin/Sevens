using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.AccessControl;
using Seven.Aggregates;
using Seven.Infrastructure.Serializer;

namespace Seven.Infrastructure.Repository
{
    /// <summary>
    /// 不采用ES
    /// </summary>
    public class NonEventSouringRepository : IRepository
    {
        private IAggregateRootStorage _aggregateRootStorage;

        private IBinarySerializer _binarySerializer;
        public NonEventSouringRepository(
            IAggregateRootStorage aggregateRootStorage,
            IBinarySerializer binarySerializer)
        {
            _aggregateRootStorage = aggregateRootStorage;
            _binarySerializer = binarySerializer;
        }

        public void Add(IAggregateRoot aggregateRoot)
        {
            if (aggregateRoot == null) return;

            _aggregateRootStorage.Add(new AggregateRootRecord()
            {
                AggregateRootId = aggregateRoot.AggregateRootId,
                Version = aggregateRoot.Version,
                TimeStamp = DateTime.Now,
                ByteDatas = _binarySerializer.Serialize(aggregateRoot)
            });
        }

        public TAggregateRoot Get<TAggregateRoot>(string aggregateRootId) where TAggregateRoot : IAggregateRoot
        {
            var aggaregateRecord = _aggregateRootStorage.Get(aggregateRootId);

            if (aggaregateRecord == null) return default(TAggregateRoot);

            var aggaregateRoot = _binarySerializer.Deserialize<IAggregateRoot>(aggaregateRecord.ByteDatas);

            if (aggaregateRoot == null) return default(TAggregateRoot);

            return (TAggregateRoot)aggaregateRoot;
        }
    }
}