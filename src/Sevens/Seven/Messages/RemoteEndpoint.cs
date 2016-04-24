using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Messages
{
    public class RemoteEndpoint
    {
        public string HostName { get; private set; }

        public string VirtualName { get; private set; }

        public string UserName { get; private set; }

        public string Password { get; private set; }

        public int Port { get; private set; }

        public RemoteEndpoint(
            string hostName,
            string virtualName,
            string useName,
            string password,
            int port)
        {
            HostName = hostName;
            VirtualName = virtualName;
            UserName = useName;
            Password = password;
            Port = port;
        }
    }
}
