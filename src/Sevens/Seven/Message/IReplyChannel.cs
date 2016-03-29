namespace Seven.Message
{
    public interface IReplyChannel : IRabbitMqChannel
    {
        MessageHandleResult GetResult();

        void SetResult(MessageHandleResult messageHandleResult);
    }
}