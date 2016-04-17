using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using MySql.Data.MySqlClient;
using Seven.Events;
using Seven.Infrastructure.Persistence;

namespace Seven.Infrastructure.EventStore
{
    public class MySqlEventStore : IEventStore
    {

        private IDbConnection _dbConnection;

        private string _connectionString;

        public MySqlEventStore(string connectionString)
        {
            _connectionString = connectionString;

            _dbConnection = new MySqlConnection(connectionString);
        }

        public EventStreamRecord LoadEventStream(string aggregateRootId)
        {
            return _dbConnection.Get<EventStreamRecord>(aggregateRootId);
        }

        /// <summary>
        /// 加载版本后面所有的事件
        /// </summary>
        /// <param name="aggregateRootId"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public EventStreamRecord LoadEventStream(string aggregateRootId, int version)
        {
            var entity = _dbConnection.Query<EventStreamRecord>(
                "select * from EventStreamEntity where AggregateRootId=@aggregateRootId and Version=@version",
                new { aggregateRootId = aggregateRootId, version = version }).FirstOrDefault();

            return entity;
        }

        public void AppendAsync(EventStreamRecord eventStream)
        {
            var result = _dbConnection.Execute(
                @"insert into EventStreamEntity(AggregateRootId,Version,EventDatas) values (@AggregateRootId,@Version,@EventDatas)",
                eventStream);
        }
    }
}
