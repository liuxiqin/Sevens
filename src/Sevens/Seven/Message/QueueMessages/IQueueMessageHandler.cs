namespace Seven.Message.QueueMessages
{
    public interface IQueueMessageHandler
    {
        void Handle(QueueMessage message);
    }
}