using System;
using System.Collections.Generic;

namespace Seven.Messages.QueueMessages
{
    [Serializable]
    public class QueueMessage
    {
        public string MessageId { get; set; }

        public string RoutingKey { get; set; }

        public string Topic { get; set; }

        public string TypeName { get; set; }

        public MessageType MessageType { get; set; }

        public bool IsRpcInvoke { get; set; }

        public ulong DeliveryTag { get; set; }

        public IMessage Message { get; set; }

    }
}
