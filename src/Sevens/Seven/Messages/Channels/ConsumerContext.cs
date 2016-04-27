namespace Seven.Messages.Channels
{
    public class ConsumerContext
    {
        public readonly string QueueName;

        public readonly string ExChangeName;

        public readonly string RoutingKey;

        public readonly bool Durable;

        public readonly bool NoAck;

        public readonly string ExchangeType;

        public readonly string ResponseRoutingKey;

        public ConsumerContext(
            string exChangeName,
            string queueName,
            string routingKey,
            string responseRoutingKey = "",
            bool noAck = false,
            bool durable = false,
            string exchangeType = MessageExchangeType.Direct)
        {
            ExChangeName = exChangeName;
            QueueName = queueName;
            RoutingKey = routingKey;
            ResponseRoutingKey = responseRoutingKey;
            NoAck = noAck;
            Durable = durable;
            ExchangeType = exchangeType;
        }

        public string GetConsumerKey()
        {
            return string.Format("{0}_{1}_{2}", ExChangeName, QueueName, RoutingKey);
        }
    }
}