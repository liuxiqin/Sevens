using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;

namespace Seven.Infrastructure.Snapshoting
{
    public class MysqlSnapshotStorage : ISnapshotStorage
    {
        private IDbConnection _dbConnection;

        private readonly string _connectionString;

        public MysqlSnapshotStorage(string connectionString)
        {
            _connectionString = connectionString;

            _dbConnection = new MySqlConnection(_connectionString);
        }

        public bool Create(SnapshotRecord snapshot)
        {
            var resultTask = _dbConnection.ExecuteAsync(
                  @"insert into SnapshotEntity(AggregateRootId,Versions,TimeStamp,Datas) values(@AggregateRootId,@Versions,@TimeStamp,@Datas)",
                  snapshot);

            return resultTask.Result == 1;
        }

        public async Task<SnapshotRecord> GetLastestSnapshot(string aggregateRootId)
        {
            var resultTask = _dbConnection.QueryAsync<SnapshotRecord>(
                "select * from SnapshotEntity where AggregateRootId=@AggregateRootId ORDER BY versions DESC LIMIT 1",
                new { AggregateRootId = aggregateRootId });

            await resultTask;

            return resultTask.Result.FirstOrDefault();
        }
    }
}