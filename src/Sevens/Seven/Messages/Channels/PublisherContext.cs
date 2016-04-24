using System.Runtime.Remoting.Messaging;
using Seven.Messages.QueueMessages;

namespace Seven.Messages.Channels
{
    public class PublisherContext
    {
        public readonly string ExchangeName;

        public readonly string ExchangeType;

        public readonly bool ShouldPersistent;

        public readonly bool NoAck;

        public PublisherContext(
            string exchangeName,
            string exchangeType,
            bool shouldPersistent,
            bool noAck)
        {
            ExchangeName = exchangeName;
            ExchangeType = exchangeType;
            ShouldPersistent = shouldPersistent;
            NoAck = noAck;
        }
    }
}