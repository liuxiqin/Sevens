using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Seven.Messages.QueueMessages;

namespace Seven.Messages
{
    public interface IMessageProducter
    {
        Task<MessageHandleResult> Publish<TMessage>(TMessage message) where TMessage : IMessage;

        Task<MessageHandleResult> Publish<TMessage>(TMessage message, TimeSpan timeout) where TMessage : IMessage;

        void PublishAsync<TMessage>(TMessage message) where TMessage : IMessage;

        void PublishAsync(QueueMessage message);

    }
}
