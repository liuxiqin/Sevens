using System;
using Seven.Infrastructure.Serializer;

namespace Seven.Messages.Pipelines
{
    /// <summary>
    /// 接收消息
    /// </summary>
    public class ReceiveMessageHandler : IMessageHandler
    {   
        public void Handle(MessageContext context)
        { 
            context.SetMessage(context.QueueMessage.Message);
        }
    }
}
