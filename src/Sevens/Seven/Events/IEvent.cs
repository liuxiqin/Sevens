using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Messages;

namespace Seven.Events
{
    public interface IEvent : IMessage
    {

    }

    /// <summary>
    /// 非领域事件继承此类；
    /// 此事件专门用与SAGAS（ProcessManager）
    /// </summary>
    public interface IMessageEvent : IEvent
    {

    }
}
