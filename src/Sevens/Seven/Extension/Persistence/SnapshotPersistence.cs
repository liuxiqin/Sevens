using System.Data;
using System.Data.SqlClient;
using Dapper;
using Seven.Infrastructure.Persistence;
using Seven.Infrastructure.Snapshoting;

namespace Seven.Extension.Persistence
{
    public class SnapshotPersistence : IPersistence<SnapshotEntity>
    {
        private IDbConnection _dbConnection;

        public SnapshotPersistence(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void Save(SnapshotEntity entity)
        {
            _dbConnection.Execute(
                @"insert into SnapshotEntity(AggregateRootId,Versions,Datas) values(@AggregateRootId,@Versions,@Datas)",
                entity);
        }

        public SnapshotEntity GetById(string aggregateRootId)
        {
            return default(SnapshotEntity);
        }

        public SnapshotEntity Get(ISpecification<SnapshotEntity> specification)
        {
            return default(SnapshotEntity);
        }

        public SnapshotEntity Get(string aggregateRootId, int version)
        {
            return default(SnapshotEntity);
        }
    }
}
