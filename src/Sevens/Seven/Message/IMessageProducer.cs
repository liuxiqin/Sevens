using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Message
{
    public interface IMessageProducer
    {
        Task<MessageHandleResult> Publish<TMessage>(TMessage message) where TMessage : IMessage;

        Task PublishAsync<TMessage>(TMessage message) where TMessage : IMessage;
    }
}
