using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Message
{
    [Serializable]
    public class QueueMessage
    {
        public string Tag { get; set; }

        public string Topic { get; set; }

        public string TypeName { get; set; }

        public MessageType MessageType { get; set; }

        public byte[] Datas;
    }
}
