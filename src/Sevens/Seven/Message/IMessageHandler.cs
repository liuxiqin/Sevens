using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Pipeline;

namespace Seven.Message
{
    public interface IMessageHandler
    {
        void Execute(MessageContext message);
    }
}
