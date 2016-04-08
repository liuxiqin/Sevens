using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seven.Messages
{
    /// <summary>
    /// 消息消费者
    /// </summary>
    public interface IMessageConsumer
    {
        void Start();
         
        void Stop();

    }
     
}
