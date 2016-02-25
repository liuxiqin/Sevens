using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Message
{
    public class RabbitMqConnectionInfo
    {
        public string UserName { get; private set; }

        public string UserPaasword { get; private set; }

        public string HostName { get; private set; }

        public int Port { get; private set; }

        public RabbitMqConnectionInfo(
            string userName,
            string userPassword,
            string hostName,
            int port)
        {
            UserName = userName;
            UserPaasword = userPassword;
            HostName = hostName;
            Port = port;
        }
    }
}
