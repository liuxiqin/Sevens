namespace Seven.Messages
{
    public class ReceiveMessage
    {
        public byte[] ByteDatas { get; private set; }

        public ulong DeliveryTag { get; private set; }

        public bool Redelivered { get; private set; }

        public ReceiveMessage(byte[] datas, ulong deliveryTag, bool redelivered)
        {
            ByteDatas = datas;
            DeliveryTag = deliveryTag;
            Redelivered = redelivered;
        }
    }
}