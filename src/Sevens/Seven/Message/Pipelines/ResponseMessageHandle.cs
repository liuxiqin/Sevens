using Seven.Infrastructure.Serializer; 

namespace Seven.Message.Pipelines
{
    public class ResponseMessageHandler : IMessageHandler
    {
        private readonly IBinarySerializer _binarySerializer;

        public ResponseMessageHandler(IBinarySerializer binarySerializer)
        {
            _binarySerializer = binarySerializer;
        }

        public void Handle(MessageContext context)
        {
          //  var responseChannel = context.Connection.CreateModel();

            //var responseDatas = _binarySerializer.Serialize(context.Response);

            //responseChannel.ExchangeDeclare(context.Topic, ExchangeType.Direct, //true);

            //responseChannel.BasicPublish(context.Topic, context.Message.MessageId,
              //  new BasicProperties() { DeliveryMode = 2 }, responseDatas);
        }
    }
}
