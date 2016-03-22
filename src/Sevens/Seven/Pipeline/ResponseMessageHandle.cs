using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Infrastructure.Serializer;
using Seven.Message;

namespace Seven.Pipeline
{
    public class ResponseMessageHandler : IMessageHandler
    {
        private readonly IBinarySerializer _binarySerializer;

        public ResponseMessageHandler(IBinarySerializer binarySerializer)
        {
            _binarySerializer = binarySerializer;
        }

        public void Execute(MessageContext message)
        {
            var responseChannel = message.Connection.CreateModel();

            var responseDatas = _binarySerializer.Serialize(message.Response);

            responseChannel.BasicPublish(message.Topic, message.Message.MessageId, null, responseDatas);
        }
    }
}
