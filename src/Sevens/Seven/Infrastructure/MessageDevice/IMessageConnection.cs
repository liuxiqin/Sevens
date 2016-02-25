using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Infrastructure.MessageDevice
{
    public interface IMessageConnection:IDisposable
    {

        MessageConnectionState State { get; }
        void Open();
        void Close();
    }
}
