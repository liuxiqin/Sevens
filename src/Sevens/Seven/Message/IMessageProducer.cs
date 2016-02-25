using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Seven.Message
{
    public interface IMessageProducer
    {
        void Publish<TMessage>(TMessage message) where TMessage : IMessage;

        void PublishAsync<TMessage>(TMessage message) where TMessage : IMessage;
    }
}
