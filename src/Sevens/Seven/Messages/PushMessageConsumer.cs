using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Seven.Configuration;
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
        private readonly IQueueMessageHandler _messageHandler;

        private readonly CancellationTokenSource _cancellation;
         
        private readonly ConsumerContext _consumerContext;

        private CommunicateChannelFactoryPool _channelPools;

        private IBinarySerializer _binarySerializer;

        public PushMessageConsumer(
            CommunicateChannelFactoryPool channelPools,
            IBinarySerializer binarySerializer,
            ConsumerContext consumerContext,
            IQueueMessageHandler messageHandler)
        {
            _channelPools = channelPools;
             
            _consumerContext = consumerContext;

            _messageHandler = messageHandler;

            _binarySerializer = binarySerializer;

            _cancellation = new CancellationTokenSource();
        }

        public void Start()
        {
            var channel = _channelPools.GetChannel(_consumerContext);

            if (channel == null)
                throw new ApplicationException("can not start the message of consumer.");

            var listenerTask = new Task(() =>
            {
                while (!_cancellation.IsCancellationRequested)
                {
                    var receiveMessage = channel.Receive();

                    if (receiveMessage == null)
                    {
                        Thread.Sleep(1);
                        continue;
                    }

                    var messageWrapper = _binarySerializer.Deserialize<MessageWrapper>(receiveMessage.ByteDatas);

                    var messageContext = new MessageContext(messageWrapper, _consumerContext, receiveMessage.DeliveryTag);

                    _messageHandler.Handle(messageContext);
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