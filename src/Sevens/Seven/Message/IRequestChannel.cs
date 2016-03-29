using System;

namespace Seven.Message
{
    public interface IRequestChannel : IRabbitMqChannel
    {
        IReplyChannel SendMessage(QueueMessage message,TimeSpan timeout);

        void SendMessageAsync(QueueMessage message);
    }
}