using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Seven.Messages;
using Seven.Messages.Channels;

namespace Seven.Events
{
    public class EventPublisher : IEventPublisher
    {

        public Task<AsyncHandleResult> Publish(IEvent evnt)
        {
            var publishTask = new Task<AsyncHandleResult>(() =>
            {
                var requestContext = new RequestMessageContext();

                var requestChannel = RequestChannelPools.GetRequestChannel(requestContext);

                requestChannel.SendMessage(null, TimeSpan.FromSeconds(10));

                return new AsyncHandleResult(HandleStatus.Success);
            });


            publishTask.Start();

            return publishTask;
        }

        public Task<AsyncHandleResult> PublishAsync(IEvent evnt)
        {
            throw new NotImplementedException();
        }

        public Task<AsyncHandleResult> PublishAsync(IList<IEvent> events)
        {
            var publishTask = new Task<AsyncHandleResult>(() =>
            {
                foreach (var evnt in events)
                {
                    var requestContext = new RequestMessageContext();

                    var requestChannel = RequestChannelPools.GetRequestChannel(requestContext);

                    requestChannel.SendMessage(null, TimeSpan.FromSeconds(10));
                }

                return new AsyncHandleResult(HandleStatus.Success);
            });


            publishTask.Start();

            return publishTask;
        }
    }
}