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

        public Queue<IEvent> _unCommitEvents;

        private string _aggregateRootId;

        public int Version { get; set; }

        protected AggregateRoot(string aggregateRootId)
        {
            _unCommitEvents = new Queue<IEvent>();

            _aggregateRootId = aggregateRootId;
        }

        public string AggregateRootId
        {
            get { return _aggregateRootId; }
            protected set { _aggregateRootId = value; }

        }

        /// <summary>
        /// 此处专门处理领域事件
        /// </summary>
        /// <param name="evnt"></param>
        public void ApplyEvent(IDomainEvent evnt)
        {
            HandleEvent(evnt);
            AppendUnCommitEvents(evnt);
        }

        public void AppendUnCommitEvents(IEvent evnt)
        {
            if (_unCommitEvents == null)
            {
                _unCommitEvents = new Queue<IEvent>();
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

        public IList<IEvent> GetChanges()
        {
            var unCommitEvents = new List<IEvent>();

            while (_unCommitEvents.Count > 0)
            {
                unCommitEvents.Add(_unCommitEvents.Dequeue());
            }

            return unCommitEvents;
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

            ((dynamic)this).Handle(evnt);

            handler(this, evnt);
        }


        public IList<IEvent> Commit()
        {
            var unCommitEvents = new List<IEvent>();

            while (_unCommitEvents.Count > 0)
            {
                unCommitEvents.Add(_unCommitEvents.Dequeue());
            }

            return unCommitEvents;
        }



    }
}