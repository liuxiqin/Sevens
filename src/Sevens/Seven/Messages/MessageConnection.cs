using RabbitMQ.Client;

namespace Seven.Messages
{
    /// <summary>
    /// message connection
    /// </summary>
    public class MessageConnection : IMessageConnection
    {
        private ConnectionFactory _connectionFactory;

        private IConnection _connection;

        public readonly RemoteEndpoint _endpoint;

        private object _lockObj = new object();
         
        public IModel CreateChannel()
        {
            if (_connection == null)
            {
                lock (_lockObj)
                {
                    _connection = _connectionFactory.CreateConnection();

                    _connection = BindEvent(_connection);
                }
            }

            return _connection.CreateModel();
        }

        public MessageConnection(RemoteEndpoint endpoint)
        {
            _endpoint = endpoint;

            Initializer();
        }

        public void Initializer()
        {
            _connectionFactory = new ConnectionFactory();
            _connectionFactory.HostName = _endpoint.HostName;
            _connectionFactory.Port = _endpoint.Port;
            _connectionFactory.UserName = _endpoint.UserName;
            _connectionFactory.Password = _endpoint.Password;
            _connectionFactory.VirtualHost = _endpoint.VirtualName;

        }

        public void Stop()
        {
            if (_connection.IsOpen)
            {
                _connection.Close();
            }
        }

        public bool IsOpen()
        {
            return _connection.IsOpen;
        }

        public void Dispose()
        {
            Stop();
        }

        private IConnection BindEvent(IConnection connection)
        {
            connection.ConnectionShutdown += (sender, args) => { };

            connection.CallbackException += (sender, args) => { };

            connection.ConnectionUnblocked += (sender, args) => { };

            connection.ConnectionBlocked += (sender, args) => { };

            return connection;
        }
    }
}
