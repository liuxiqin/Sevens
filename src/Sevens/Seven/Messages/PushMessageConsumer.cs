using System.Threading;
using System.Threading.Tasks;
using Seven.Infrastructure.Serializer;
using Seven.Messages.Channels;
using Seven.Messages.Pipelines;
using Seven.Messages.QueueMessages;

namespace Seven.Messages
{
    /// <summary>
    /// RaabitMq推送消息消费通道
    /// </summary>
    public class PushMessageConsumer : IMessageConsumer
    {
        private readonly MessageChannelBase _channel;

        private readonly RequestMessageContext _channelInfo;

        private readonly IQueueMessageHandler _messageHandler;

        private readonly CancellationTokenSource _cancellation;

        public PushMessageConsumer(RequestMessageContext channelInfo, IQueueMessageHandler messageHandler)
        {
            _channelInfo = channelInfo;
            _channel = new ConsumerChannel(_channelInfo);
            _messageHandler = messageHandler;
            _cancellation = new CancellationTokenSource();
        }

        public void Start()
        {
            var listenerTask = new Task(() =>
            {
                while (!_cancellation.IsCancellationRequested)
                {
                    var queueMessage = _channel.ReceiveMessage();

                    var messageContext = new MessageContext(_channel, queueMessage, _channelInfo, queueMessage.DeliveryTag);

                    _messageHandler.Handle(messageContext);

                    Thread.Sleep(1);
                }
            });

            listenerTask.Start();
        }


        public void Stop()
        {
            _cancellation.Cancel();
        }

    }
}