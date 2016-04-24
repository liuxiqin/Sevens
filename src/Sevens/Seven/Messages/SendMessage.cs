namespace Seven.Messages
{
    public class SendMessage
    {
        public byte[] ByteDatas { get; private set; }

        public string RoutingKey { get; private set; }

        public SendMessage(byte[] datas, string routingKey)
        {
            ByteDatas = datas;

            RoutingKey = routingKey;
        }
    }
}