using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;
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

        public void Execute(MessageContext context)
        {
          //  var responseChannel = context.Connection.CreateModel();

            //var responseDatas = _binarySerializer.Serialize(context.Response);

            //responseChannel.ExchangeDeclare(context.Topic, ExchangeType.Direct, //true);

            //responseChannel.BasicPublish(context.Topic, context.Message.MessageId,
              //  new BasicProperties() { DeliveryMode = 2 }, responseDatas);
        }
    }
}
