using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Events
{
    public interface IEventSubscription
    {
        void RegisterHandler<TEvent>(IEventHandler<IEvent> eventHandler) where TEvent : IEvent;

        void UnRegisterRegisterHandler<TEvent>() where TEvent : IEvent;

        void UnRegisterRegisterHandler<TEvent>(IEventHandler<IEvent> eventHandler) where TEvent : IEvent;

        IList<IEventHandler<IEvent>> GetEventHandlers<TEvent>() where TEvent : IEvent;
    }
}
