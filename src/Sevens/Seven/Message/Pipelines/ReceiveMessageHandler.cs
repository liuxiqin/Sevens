using System;
using Seven.Infrastructure.Serializer;

namespace Seven.Message.Pipelines
{
    /// <summary>
    /// 接收消息
    /// </summary>
    public class ReceiveMessageHandler : IMessageHandler
    {
        private IBinarySerializer _binarySerializer;

        public ReceiveMessageHandler(IBinarySerializer binarySerializer)
        {
            _binarySerializer = binarySerializer;
        }

        public void Handle(MessageContext context)
        {
            var message = _binarySerializer.Deserialize<IMessage>(context.QueueMessage.Datas);

            context.SetMessage(message);
        }
    }
}
