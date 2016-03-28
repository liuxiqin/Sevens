using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Message
{
    public class RabbitMqConfiguration
    {
        public string HostName = "12.0.0.1";

        public string VirtualName = "/";

        public string UserName = "guest";

        public string UserPaasword = "guest";

        public int Port = 5672;

        public RabbitMqConfiguration(
            string hostNamne,
            string virtualName,
            string userName,
            string userPassword,
            int port)
        {
            HostName = hostNamne;
            VirtualName = virtualName;
            UserPaasword = userPassword;
            UserName = userName;
            Port = port;
        }

        public RabbitMqConfiguration() { }
    }
}
