using System.Security.Permissions;

namespace Seven.Messages.Channels
{
    public class RequestMessageContext
    {
        public readonly string ExChangeName;

        public readonly string RoutingKey;

        public readonly bool ShouldPersistent;

        public readonly bool NoAck;

        public readonly string ExchangeType;

        public readonly string ResponseRoutingKey;

        public override string ToString()
        {
            return ExChangeName;
        }

        public RequestMessageContext(
            string exChangeName,
            string routingKey,
            string responseRoutingKey,
            string exchangeType = MessageExchangeType.Direct,
            bool noAck = false,
            bool shouldPersistent = true)
        {
            ExChangeName = exChangeName;
            RoutingKey = routingKey;
            ResponseRoutingKey = responseRoutingKey;
            ExchangeType = exchangeType;
            NoAck = noAck;
            ShouldPersistent = shouldPersistent;
        }
    }
}