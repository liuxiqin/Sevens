using System;
using System.Collections.Generic;

namespace Seven.Messages.QueueMessages
{
    [Serializable]
    public class MessageWrapper
    {
        public string MessageId { get; set; }

        public string RoutingKey { get; set; }
         
        public string ExchangeName { get; set; }

        public string TypeName { get; set; }

        public MessageType MessageType { get; set; }

        public bool IsRpcInvoke { get; set; }

        public IMessage Message { get; set; }

        public string ResponseRoutingKey { get; set; }

        public bool ShouldPersist { get; set; }

    }
}
