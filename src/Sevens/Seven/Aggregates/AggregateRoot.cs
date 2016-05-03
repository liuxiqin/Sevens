using System;
using System.Collections.Generic;
using System.Linq;
using Seven.Events;
using Seven.Infrastructure.Dependency;
using Seven.Initializer;

namespace Seven.Aggregates
{
    [Serializable]
    public abstract class AggregateRoot : IAggregateRoot
    {
        [NonSerialized]
        private EventHandleProvider _eventHandleProvider;

        [NonSerialized]
        private Queue<IEvent> _unCommitEvents;

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
        public void ApplyEvent(IEvent evnt)
        {
            HandleEvent(evnt);
            AppendUnCommitEvents(evnt);
        }

        public void AppendUnCommitEvents(IEvent evnt)
        {
            if (_unCommitEvents == null)
                _unCommitEvents = new Queue<IEvent>();

            _unCommitEvents.Enqueue(evnt);
        }

        public void ApplyEvents(IList<IEvent> events)
        {
            foreach (var evnt in events)
                ApplyEvent(evnt);
        }

        public IList<IEvent> GetChanges()
        {
            var unCommitEvents = new List<IEvent>();

            if (_unCommitEvents.Count > 0)
            {
                unCommitEvents = _unCommitEvents.ToList();
            }

            return unCommitEvents;
        }

        public void HandleEvent(IEvent evnt)
        {
            if (_eventHandleProvider == null)
            {
                _eventHandleProvider = DependencyResolver.Resolve<EventHandleProvider>();
            }

            var handler = _eventHandleProvider.GetInternalHandler(this.GetType(), evnt.GetType());

            if (handler == null)
            {
                throw new ApplicationException("can not find the handle with type of " + evnt.GetType());
            }

            handler(this, evnt);
        }


        public void Clear()
        {
            _unCommitEvents.Clear();
        }
    }
}