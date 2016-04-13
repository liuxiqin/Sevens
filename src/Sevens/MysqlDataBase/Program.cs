using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using DapperExtensions.Mapper;
using MySql.Data.MySqlClient;
using Seven.Extension.Persistence;
using Seven.Infrastructure.Snapshoting;
using Seven.Infrastructure.UniqueIds;

namespace MysqlDataBase
{
    public class TestEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }

    public class SnapshotEntityMapper : ClassMapper<SnapshotEntity>
    {

    }

    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("connect to mysql....");

            var datas = "connect to mysql test";

            var bytes = Encoding.UTF8.GetBytes(datas);

            var snapshot = new SnapshotEntity(ObjectId.NewObjectId(), 1, bytes);

            var testEntity = new TestEntity() { Id = "13566558", Name = "Dapper Insert Test" };

           

            using (IDbConnection connection = new MySqlConnection("Database = sevens; Data Source = 127.0.0.1; User Id = root; Password = 123456; port = 3306"))
            {
                var snapshotPersistence = new SnapshotPersistence(connection);

                connection.Open();

                snapshotPersistence.Save(snapshot);

                var result = connection.Query<SnapshotEntity>(@"select * from SnapshotEntity");

                Console.ReadLine();
            }
        }


    }
}
