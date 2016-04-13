using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Messages
{
    [Serializable]
    public class MessageHandleResult : IMessage
    {
        public string MessageId { get; set; }

        public string Message { get; set; }

        public MessageStatus Status { get; set; }

        public MessageHandleResult(string messageId, string message, MessageStatus status)
        {
            MessageId = messageId;
            Message = message;
            Status = status;
        }

        public MessageHandleResult() { }
    }
}
