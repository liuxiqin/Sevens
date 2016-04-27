using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using Seven.Aggregates;

namespace Seven.Infrastructure.Repository
{
    public interface IAggregateRootStorage
    {
        void Add(AggregateRootRecord aggregateRoot);

        AggregateRootRecord Get(string aggregateRootSId);
    }

    public class MysqlAggregateRootStorage : IAggregateRootStorage
    {
        private IDbConnection _dbConnection;

        public MysqlAggregateRootStorage(string connectionString)
        {
            _dbConnection = new MySqlConnection(connectionString);
        }

        public void Add(AggregateRootRecord aggregateRoot)
        {
            _dbConnection.Execute(
             @"insert into aggregaterecord(AggregateRootId,Version,TimeStamp,ByteDatas) values(@AggregateRootId,@Version,@TimeStamp,@ByteDatas)",
             aggregateRoot);
        }

        public AggregateRootRecord Get(string aggregateRootId)
        {
            var resultTask = _dbConnection.QueryAsync<AggregateRootRecord>(
                "select * from aggregaterecord where AggregateRootId=@AggregateRootId ORDER BY Version DESC LIMIT 1",
                new {AggregateRootId = aggregateRootId});

            return resultTask.Result.FirstOrDefault();
        }
    }

    public class AggregateRootRecord
    {
        public string AggregateRootId { get; set; }

        public int Version { get; set; }

        public byte[] ByteDatas { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
