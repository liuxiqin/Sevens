using System;
using System.Threading.Tasks;
using Seven.Messages.QueueMessages;

namespace Seven.Messages.Channels
{
    public interface IRequestChannel
    {
        Task<IReplyChannel> SendMessage(MessageWrapper message, int seconds = 10);

        Task SendMessageAsync(MessageWrapper message);
    }
}