using System.Runtime.Remoting.Messaging;
using Seven.Messages.QueueMessages;

namespace Seven.Messages.Channels
{
    public class PublisherContext
    {
        public readonly string ExchangeName;

        public readonly string ExchangeType;

        public readonly bool NoAck;

        public readonly bool Durable;

        public readonly bool AutoDelete;

        public PublisherContext(
            string exchangeName,
            string exchangeType,
            bool durable,
            bool autoDelete,
            bool noAck)
        {
            ExchangeName = exchangeName;
            ExchangeType = exchangeType;
            Durable = durable;
            AutoDelete = autoDelete;
            NoAck = noAck;
        }
    }
}