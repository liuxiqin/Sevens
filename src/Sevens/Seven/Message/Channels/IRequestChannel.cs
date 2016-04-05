using System;
using Seven.Message.QueueMessages;

namespace Seven.Message.Channels
{
    public interface IRequestChannel 
    {
        IReplyChannel SendMessage(QueueMessage message,TimeSpan timeout);

        void SendMessageAsync(QueueMessage message);
    }
}