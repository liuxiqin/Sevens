using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Seven.Messages;
using Seven.Messages.Channels;
using Seven.Messages.QueueMessages;
using Seven.Configuration;
namespace Seven.Events
{
    public class EventPublisher : IEventPublisher
    {
        public Task<AsyncHandleResult> Publish(IEvent evnt)
        {
            var publishTask = Task.Run(() =>
            {
                var queueMessage = BuildMessage(evnt);

                var requestContext = new RequestMessageContext()
                {
                    RoutingKey = queueMessage.RoutingKey,
                    ResponseRoutingKey = null,
                    ShouldPersistent = true,
                    ExChangeName = queueMessage.Topic,
                    NoAck = false,
                    Configuation = SevensConfiguretion.RabbitMqConfiguration,
                    ExchangeType = MessageExchangeType.Direct
                };

                var requestChannel = RequestChannelPools.GetRequestChannel(requestContext);

                requestChannel.SendMessageAsync(queueMessage);

                return new AsyncHandleResult(HandleStatus.Success);
            });

            return publishTask;
        }

        public Task<AsyncHandleResult> PublishAsync(IEvent evnt)
        {
            throw new NotImplementedException();
        }

        public Task<AsyncHandleResult> PublishAsync(IList<IEvent> events)
        {
            var publishTask = Task.Run(() =>
            {
                foreach (var evnt in events)
                {
                    var queueMessage = BuildMessage(evnt);


                    var requestContext = new RequestMessageContext()
                    {
                        RoutingKey = queueMessage.RoutingKey,
                        ResponseRoutingKey = null,
                        ShouldPersistent = true,
                        ExChangeName = queueMessage.Topic,
                        NoAck = false,
                        Configuation = SevensConfiguretion.RabbitMqConfiguration,
                        ExchangeType = MessageExchangeType.Direct
                    };

                    var requestChannel = RequestChannelPools.GetRequestChannel(requestContext);

                    requestChannel.SendMessageAsync(queueMessage);
                }

                return new AsyncHandleResult(HandleStatus.Success);
            });

            return publishTask;
        }

        private QueueMessage BuildMessage(IEvent evnt)
        {
            return new QueueMessage()
            {
                IsRpcInvoke = false,
                Message = evnt,
                MessageId = evnt.MessageId,
                MessageType = MessageType.OneWay,
                ResponseRoutingKey = string.Empty,
                RoutingKey = string.Format("{0}_{1}", "event", evnt.MessageId.GetHashCode() % 5),
                Topic = evnt.GetType().Assembly.FullName,
                TypeName = evnt.GetType().FullName
            };
        }
    }
}