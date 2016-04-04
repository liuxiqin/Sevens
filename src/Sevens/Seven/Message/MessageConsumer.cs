using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing;
using Seven.Commands;
using Seven.Infrastructure.Exceptions;
using Seven.Infrastructure.Ioc;
using Seven.Infrastructure.Serializer;
using Seven.Initializer;
using Seven.Pipeline;

namespace Seven.Message
{
    /// <summary>
    /// 消息消费者
    /// </summary>
    public class MessageConsumer : IMessageConsumer
    {
        private readonly IMessgaeExecute _messgaeExecute;

        private readonly IBinarySerializer _binarySerializer;

        private readonly CancellationTokenSource _cancellationTokenSource = null;

        private ChannelInfo _channelInfo = null;

        private MessageChannelBase _consumerChannel = null;
         
        public MessageConsumer(IBinarySerializer binarySerializer, ChannelInfo channelInfo)
        {

            _binarySerializer = binarySerializer;

            _messgaeExecute = new DefaultMessgaeExecute();

            _cancellationTokenSource = new CancellationTokenSource();

            _channelInfo = channelInfo;

            _consumerChannel=new PollConsumerChannel(channelInfo);

        }

        public void Start()
        {
            var comsumerTask = new Task(() =>
            {
                 

            }, _cancellationTokenSource.Token);
        }

      
 


        public void Execute(QueueMessage queueMessage, ulong deliveryTag)
        {
            var messageTypeProvider = ObjectContainer.Resolve<MessageTypeProvider>();

            var message = _binarySerializer.Deserialize<IMessage>(queueMessage.Datas);

           // var messageContext = new MessageContext(message, _routingKey, _exchangeName, deliveryTag);

          //  _messgaeExecute.Execute(messageContext);

           // _consumerChannel.BasicAck(deliveryTag, true);

           // Thread.Sleep(1);
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
