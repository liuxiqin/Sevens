using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using Seven.Infrastructure.Serializer;

namespace Seven.Message
{
    public class MessageResultSubscribe : MessageConsumer
    {
        public MessageResultSubscribe(IMessageBroker broker, IBinarySerializer binarySerializer,
            IJsonSerializer jsonSerializer, string exChangeName, string routingKey)
            : base(broker, binarySerializer, jsonSerializer, exChangeName, routingKey)
        {

        }
    }
}
