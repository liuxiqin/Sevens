using System;
using Seven.Messages.QueueMessages;

namespace Seven.Messages.Channels
{
    public interface IRequestChannel 
    {
        IReplyChannel SendMessage(QueueMessage message,TimeSpan timeout);

        void SendMessageAsync(QueueMessage message);
    }
}