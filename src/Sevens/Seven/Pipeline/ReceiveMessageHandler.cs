using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Message;

namespace Seven.Pipeline
{
    /// <summary>
    /// 接收消息
    /// </summary>
    public class ReceiveMessageHandler : IMessageHandler
    {
        public void Execute(MessageContext message)
        {
            Console.WriteLine(message.ToString());
        }
    }
}
