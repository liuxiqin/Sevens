using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Messages
{
    public interface IMessage
    {
        string MessageId { get; }

    }

    [Serializable]
    public class ApplicationMessage
    {
        public string Tag { get; set; }

        public string Topic { get; set; }

        public string Body { get; set; }

        public byte[] Bodys { get; set; }
    }
}
