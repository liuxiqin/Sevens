namespace Seven.Message
{
    public interface IReplyChannel : IRabbitMqChannel
    {
        MessageHandleResult GetResult();
    }
}