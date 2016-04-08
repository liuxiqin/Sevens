namespace Seven.Messages.Channels
{
    public class RequestMessageContext
    {
        public string ExChangeName { get; set; }

        public string RoutingKey { get; set; }

        public bool ShouldPersistent { get; set; }

        public bool NoAck { get; set; }

        public string ExchangeType { get; set; }

        public RabbitMqConfiguration Configuation { get; set; }

        public override string ToString()
        {
            return string.Format("{0}_{1}", ExChangeName, RoutingKey);
        }
    }
}