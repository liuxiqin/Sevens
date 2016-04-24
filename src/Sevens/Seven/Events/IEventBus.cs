using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Events
{
    public interface IEventBus
    {
        void Send<TEvent>(TEvent evnt) where TEvent : IEvent;

        void Dispatch<TEvent>(TEvent evnt) where TEvent : IEvent;
    }

    public class EventBus : IEventBus
    {
        private ConcurrentQueue<IEvent> _enventBus;

        private int _maxLength = 100;

        private IEventSubscription _eventObserver;

        private bool _started = false;

        public EventBus(IEventSubscription eventObserver)
        {
            _enventBus = new ConcurrentQueue<IEvent>();

            _eventObserver = eventObserver;
        }

        public void Send<TEvent>(TEvent evnt) where TEvent : IEvent
        {
            _enventBus.Enqueue(evnt);

            if (!_started)
            {
                _started = true;

                StartConsumer();
            }
        }

        private void StartConsumer()
        {
            var evnt = default(IEvent);
            if (_enventBus.TryDequeue(out evnt))
                Dispatch(evnt);
        }

        public void Dispatch<TEvent>(TEvent evnt) where TEvent : IEvent
        {
            var eventHandlers = _eventObserver.GetEventHandlers<TEvent>();

            if (eventHandlers == null)
                throw new ApplicationException("can not find the event handlers with type of " + evnt.GetType().FullName);

            foreach (var eventHandler in eventHandlers)
            {
                eventHandler.Handle(evnt);
            }
        }
    }
}
