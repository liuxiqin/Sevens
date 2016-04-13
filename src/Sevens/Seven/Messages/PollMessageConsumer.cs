using System.Threading;
using System.Threading.Tasks;
using Seven.Infrastructure.Serializer;
using Seven.Messages.Channels;
using Seven.Messages.Pipelines;
using Seven.Messages.QueueMessages;

namespace Seven.Messages
{
    /// <summary>
    /// RaabitMq拉取消息消费通道
    /// </summary>
    public class PollMessageConsumer : IMessageConsumer
    {
        private readonly MessageChannelBase _channel;

        private readonly RequestMessageContext _channelInfo;

        private readonly IQueueMessageHandler _messageHandler;

        private readonly CancellationTokenSource _cancellation;

        public PollMessageConsumer(RequestMessageContext channelInfo, IQueueMessageHandler messageHandler)
        {
            _channelInfo = channelInfo;
            _channel = new ConsumerChannel(_channelInfo);
            _messageHandler = messageHandler;
            _cancellation = new CancellationTokenSource();
        }

        public void Start()
        {
            var listenerTask = Task.Run(() =>
            {
                while (!_cancellation.IsCancellationRequested)
                {
                    var queueMessage = _channel.ReceiveMessage();

                    var context = new MessageContext(_channel, queueMessage, _channelInfo, queueMessage.DeliveryTag);

                    _messageHandler.Handle(context);

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