using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Seven.Commands;
using Seven.Messages;
using Seven.Messages.Channels;
using Seven.Messages.QueueMessages;
using Seven.Configuration;
namespace Seven.Events
{
    public class EventPublisher : IEventPublisher
    {
        private readonly RequestChannelPools _requestChannelPools;

        private readonly IEventTopicProvider _eventTopicProvider;

        public EventPublisher(
            RequestChannelPools requestChannelPools,
            IEventTopicProvider eventTopicProvider)
        {
            _requestChannelPools = requestChannelPools;

            _eventTopicProvider = eventTopicProvider;
        }

        public Task<AsyncHandleResult> Publish(IEvent evnt)
        {
            var publishTask = Task.Run(() =>
            {
                var queueMessage = BuildMessage(evnt);

                var requestContext = new RequestMessageContext(queueMessage.ExchangeName, queueMessage.RoutingKey, null);

                var requestChannel = _requestChannelPools.GetRequestChannel(requestContext);

                requestChannel.SendMessageAsync(queueMessage);

                return new AsyncHandleResult(HandleStatus.Success);
            });

            return publishTask;
        }

        public Task<AsyncHandleResult> PublishAsync(IEvent evnt)
        {
            var publishTask = Task.Run(() =>
            {
                var queueMessage = BuildMessage(evnt);

                var requestContext = new RequestMessageContext(queueMessage.ExchangeName, queueMessage.RoutingKey, null);

                var requestChannel = _requestChannelPools.GetRequestChannel(requestContext);

                requestChannel.SendMessageAsync(queueMessage);

                return new AsyncHandleResult(HandleStatus.Success);
            });

            return publishTask;
        }

        public Task<AsyncHandleResult> PublishAsync(IList<IEvent> events)
        {
            var publishTask = Task.Run(() =>
            {
                foreach (var evnt in events)
                {
                    var queueMessage = BuildMessage(evnt);

                    var requestContext = new RequestMessageContext(
                        queueMessage.ExchangeName,
                        queueMessage.RoutingKey,
                        null);

                    var requestChannel = _requestChannelPools.GetRequestChannel(requestContext);

                    requestChannel.SendMessageAsync(queueMessage);
                }

                return new AsyncHandleResult(HandleStatus.Success);
            });

            return publishTask;
        }

        private MessageWrapper BuildMessage(IEvent evnt)
        {
            var exchangeName = _eventTopicProvider.GetTopic(evnt.GetType());

            return new MessageWrapper()
            {
                IsRpcInvoke = false,
                Message = evnt,
                MessageId = evnt.MessageId,
                MessageType = MessageType.OneWay,
                ResponseRoutingKey = string.Empty,
                RoutingKey = string.Format("{0}_{1}_{2}", exchangeName, "event", evnt.MessageId.GetHashCode() & 0x7FFFFFFF % 5),
                ExchangeName = exchangeName,
                TypeName = evnt.GetType().FullName
            };
        }
    }
}