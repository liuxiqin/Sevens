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
    /// 业务处理，返回结果，
    /// 系统错误，进行重试操作
    /// </summary>
    public class ErrorMessageHandler : IMessageHandler
    {
        public void Execute(MessageContext message)
        {
            throw new NotImplementedException();
        }
    }
}
