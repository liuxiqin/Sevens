namespace Seven.Message.Channels
{
    public interface IReplyChannel
    {
        MessageHandleResult GetResult();

        void SetResult(MessageHandleResult messageHandleResult);
    }
}