using System.Data;
using System.Linq;
using Dapper;
using DapperExtensions;
using Seven.Infrastructure.EventStore;
using Seven.Infrastructure.Persistence;
using Seven.Infrastructure.Snapshoting;

namespace Seven.Extension.Persistence
{
    public class EventStorePersistence : IPersistence<EventStreamEntity>
    {
        private IDbConnection _dbConnection;

        public EventStorePersistence(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void Save(EventStreamEntity entity)
        {
            _dbConnection.Execute(
                @"insert into EventStreamEntity(AggregateRootId,Version,EventDatas) values (@AggregateRootId,@Version,@EventDatas)",
                entity);
        }

        public EventStreamEntity GetById(string aggregateRootId)
        {
            return _dbConnection.Get<EventStreamEntity>(aggregateRootId);
        }

        public EventStreamEntity Get(ISpecification<EventStreamEntity> specification)
        {
            return default(EventStreamEntity);
        }

        public EventStreamEntity Get(string aggregateRootId, int version)
        {
            return
                _dbConnection.Query<EventStreamEntity>(
                    "select * from EventStreamEntity where AggregateRootId=@aggregateRootId and Version=@version",
                    new { aggregateRootId = aggregateRootId, version = version }).FirstOrDefault();
        }
    }
}