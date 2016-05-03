using System;
using System.Collections.Generic;
using Seven.Messages;

namespace Seven.Commands
{
    public abstract class CommandTopicProvider : ICommandTopicProvider
    {
        protected Dictionary<Type, string> _commandTopics;

        protected CommandTopicProvider()
        {
            _commandTopics = new Dictionary<Type, string>();

            Initialize();
        }

        public abstract void Initialize();

        public virtual void Register(Type commandType, string topic)
        {
            if (!_commandTopics.ContainsKey(commandType))
                _commandTopics.Add(commandType, topic);
        }


        public virtual void Register<TMessage>(string topic) where TMessage : IMessage
        {
            if (!_commandTopics.ContainsKey(typeof(TMessage)))
                _commandTopics.Add(typeof(TMessage), topic);
        }


        public virtual string GetTopic(Type commanType)
        {
            if (_commandTopics.ContainsKey(commanType))
                throw new ApplicationException("can not find the topic.");

            return _commandTopics[commanType];
        }

        public virtual string GetTopic<TMessage>() where TMessage : IMessage
        {
            if (!_commandTopics.ContainsKey(typeof(TMessage)))
                throw new ApplicationException("can not find the topic.");

            return _commandTopics[typeof(TMessage)];
        }
    }
}