using System.Threading;
using System.Threading.Tasks;
using Seven.Message.Channels;
using Seven.Message.QueueMessages;

namespace Seven.Message
{
    /// <summary>
    /// RaabitMq推送消息消费通道
    /// </summary>
    public class PushMessageConsumer : IMessageConsumer
    {
        private readonly MessageChannelBase _channel;

        private readonly ChannelInfo _channelInfo;

        private readonly IQueueMessageHandler _messageHandler;

        private readonly CancellationTokenSource _cancellation;

        public PushMessageConsumer(ChannelInfo channelInfo, IQueueMessageHandler messageHandler)
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

                    _messageHandler.Handle(queueMessage);

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