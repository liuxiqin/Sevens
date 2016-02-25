using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Seven.Aggregates;
using Seven.Events;
using Seven.Initializer;

namespace Seven.Initializer
{
    public class EventHandleProvider : IApplictionInitializer
    {
        private IDictionary<Type, IDictionary<Type, Action<IAggregateRoot, IDomainEvent>>>
            _eventHandlerProvider =
                new ConcurrentDictionary<Type, IDictionary<Type, Action<IAggregateRoot, IDomainEvent>>>();

        private readonly BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance |
                                                     BindingFlags.DeclaredOnly;

        public void Initialize(params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                foreach (var aggregateRootType in assembly.GetTypes()
                    .Where(m => !m.IsAbstract
                                && m.IsClass
                                && typeof(IAggregateRoot).IsAssignableFrom(m)))
                {
                    var entries = from method in aggregateRootType.GetMethods(bindingFlags)
                        let parameters = method.GetParameters()
                        where method.Name == "Handle"
                              && parameters.Length == 1
                              && typeof(IDomainEvent).IsAssignableFrom(parameters.Single().ParameterType)
                        select new { Method = method, EventType = parameters.Single().ParameterType };

                    foreach (var entry in entries)
                    {
                        RegisterInternalHandler(aggregateRootType, entry.EventType, entry.Method);
                    }
                }
            }
        }

        public Action<IAggregateRoot, IDomainEvent> GetInternalHandler(Type aggregateRootType, Type eventType)
        {
            IDictionary<Type, Action<IAggregateRoot, IDomainEvent>> eventHandlerDic;

            if (!_eventHandlerProvider.TryGetValue(aggregateRootType, out eventHandlerDic)) return null;

            Action<IAggregateRoot, IDomainEvent> eventHandler;

            return eventHandlerDic.TryGetValue(eventType, out eventHandler) ? eventHandler : null;
        }

        private void RegisterInternalHandler(Type aggregateRootType, Type eventType, MethodInfo eventHandler)
        {
            IDictionary<Type, Action<IAggregateRoot, IDomainEvent>> eventHandlerDic;

            if (!_eventHandlerProvider.TryGetValue(aggregateRootType, out eventHandlerDic))
            {
                eventHandlerDic = new Dictionary<Type, Action<IAggregateRoot, IDomainEvent>>();

                _eventHandlerProvider.Add(aggregateRootType, eventHandlerDic);
            }

            if (eventHandlerDic.ContainsKey(eventType))
            {
                throw new Exception(
                    string.Format("Found duplicated event handler on aggregate, aggregate type:{0}, event type:{1}",
                        aggregateRootType.FullName, eventType.FullName));
            }

            eventHandlerDic.Add(eventType, (aggregateRoot, domainEvent) =>
            {
                eventHandler.Invoke(aggregateRoot, new object[] { domainEvent });
            });
        }

        public void Dispose()
        {
            _eventHandlerProvider = null;
        }
    }
}