using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Message
{
    [Serializable]
    public class MessageHandleResult
    {
        public string Message { get; set; }

        public MessageStatus Status { get; set; }

    }
}
