using System;
using System.Collections.Generic;
using Seven.Events;
using Seven.Infrastructure.Ioc;
using Seven.Initializer;

namespace Seven.Aggregates
{
    public abstract class AggregateRoot : IAggregateRoot
    {
        private EventHandleProvider _eventHandleProvider;

        public Queue<IDomainEvent> _unCommitEvents = null;

        private string _aggregateRootId;

        protected AggregateRoot(string aggregateRootId)
        {
            _unCommitEvents = new Queue<IDomainEvent>();

            _aggregateRootId = aggregateRootId;
        }

        public string AggregateRootId
        {
            get { return _aggregateRootId; }
            protected set { _aggregateRootId = value; }

        }

        public void ApplyEvent(IDomainEvent evnt)
        {
            HandleEvent(evnt);
            AppendUnCommitEvents(evnt);
        }

        public void AppendUnCommitEvents(IDomainEvent evnt)
        {
            if (_unCommitEvents == null)
            {
                _unCommitEvents = new Queue<IDomainEvent>();
            }

            _unCommitEvents.Enqueue(evnt);
        }

        public void ApplyEvents(IList<IDomainEvent> events)
        {
            foreach (var evnt in events)
            {
                HandleEvent(evnt);
            }
        }

        public void HandleEvent(IDomainEvent evnt)
        {
            if (_eventHandleProvider == null)
            {
                _eventHandleProvider = ObjectContainer.Resolve<EventHandleProvider>();
            }

            var handler = _eventHandleProvider.GetInternalHandler(this.GetType(), evnt.GetType());

            if (handler == null)
            {
                throw new ApplicationException("can not find the handle with type of " + evnt.GetType());
            }

            handler(this, evnt);
        }

        public IList<IDomainEvent> Commit()
        {
            var unCommitEvents = new List<IDomainEvent>();

            while (_unCommitEvents.Count > 0)
            {
                unCommitEvents.Add(_unCommitEvents.Dequeue());
            }

            return unCommitEvents;
        }


    }
}