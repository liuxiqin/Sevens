using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Seven.Messages
{
    public class RabbitMqConnectionPool
    {
        private Dictionary<string, IConnection> _connections;

        public RabbitMqConnectionPool()
        {
            _connections = new Dictionary<string, IConnection>();
        }
    }
}
