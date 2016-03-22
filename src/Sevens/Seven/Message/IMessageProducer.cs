using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Message
{
    public interface IMessageProducter
    {
        Task<MessageHandleResult> Publish<TMessage>(TMessage message) where TMessage : IMessage;

        Task<MessageHandleResult> Publish<TMessage>(TMessage message, TimeSpan timeout) where TMessage : IMessage;

        void PublishAsync<TMessage>(TMessage message) where TMessage : IMessage;

        void Response(string responseQueueName, string correlationId, MessageHandleResult handleResult);
    }
}
