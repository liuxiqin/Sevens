using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Messages
{
    [Serializable]
    public class MessageHandleResult
    {
        public string MessageId { get; set; }

        public string Message { get; set; }

        public MessageStatus Status { get; set; }

    }
}
