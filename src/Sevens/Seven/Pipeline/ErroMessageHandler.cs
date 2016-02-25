using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Message;

namespace Seven.Pipeline
{
    /// <summary>
    /// 消息错误处理
    /// </summary>
    public class ErrorMessageHandler : IMessageHandler
    {
        public void Execute(MessageContext message)
        {
            throw new NotImplementedException();
        }
    }
}
