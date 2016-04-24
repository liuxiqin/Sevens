using System;
using System.Collections.Generic;
using System.Linq;

namespace Seven.Events
{
    public class EventSubscription : IEventSubscription
    {
        private IDictionary<Type, IList<IEventHandler<IEvent>>> _eventHandlers = null;

        public EventSubscription()
        {
            _eventHandlers = new Dictionary<Type, IList<IEventHandler<IEvent>>>();
        }

        public void RegisterHandler<TEvent>(IEventHandler<IEvent> eventHandler) where TEvent : IEvent
        {
            if (!_eventHandlers.ContainsKey(typeof(TEvent)))
                _eventHandlers.Add(typeof(TEvent), new List<IEventHandler<IEvent>>());

            var eventhandlers = _eventHandlers[typeof(TEvent)];

            var hasExist = eventhandlers.Any(m => m.GetType().FullName.Equals(eventHandler.GetType().FullName));

            if (!hasExist)
                eventhandlers.Add(eventHandler);
        }

        public void UnRegisterRegisterHandler<TEvent>() where TEvent : IEvent
        {
            if (!_eventHandlers.ContainsKey(typeof(TEvent))) return;

            _eventHandlers.Remove(typeof(TEvent));
        }

        public void UnRegisterRegisterHandler<TEvent>(IEventHandler<IEvent> eventHandler) where TEvent : IEvent
        {
            if (!_eventHandlers.ContainsKey(typeof(TEvent))) return;

            var eventHandlers =
                _eventHandlers[typeof(TEvent)].Where(m => !m.GetType().FullName.Equals(eventHandler.GetType().FullName))
                    .ToList();

            _eventHandlers[typeof(TEvent)] = eventHandlers;

        }


        public IList<IEventHandler<IEvent>> GetEventHandlers<TEvent>() where TEvent : IEvent
        {
            if (_eventHandlers.ContainsKey(typeof(TEvent))) return null;

            return _eventHandlers[typeof(TEvent)];
        }
    }
}