using System;
using System.Collections.Generic;

namespace Seven.Events
{
    public abstract class EventTopicProvider : IEventTopicProvider
    {
        protected Dictionary<Type, string> _eventTopics;

        protected EventTopicProvider()
        {
            _eventTopics = new Dictionary<Type, string>();

            Initialize();
        }

        public abstract void Initialize();

        public virtual void Register(Type commandType, string topic)
        {
            if (!_eventTopics.ContainsKey(commandType))
                _eventTopics.Add(commandType, topic);
        }


        public virtual void Register<TEvent>(string topic) where TEvent : IEvent
        {
            if (!_eventTopics.ContainsKey(typeof (TEvent)))
                _eventTopics.Add(typeof (TEvent), topic);
        }

        public virtual string GetTopic(Type evenType)
        {
            if (_eventTopics.ContainsKey(evenType))
                throw new ApplicationException("can not find the topic.");

            return _eventTopics[evenType];
        }

        public virtual string GetTopic<TEvent>() where TEvent : IEvent
        {
            if (!_eventTopics.ContainsKey(typeof(TEvent)))
                throw new ApplicationException("can not find the topic.");

            return _eventTopics[typeof(TEvent)];
        }
    }
}