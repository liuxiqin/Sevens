using Seven.Messages.Channels;
using Seven.Messages.Pipelines;

namespace Seven.Messages.QueueMessages
{
    public interface IQueueMessageHandler
    {
        void Handle(MessageContext context);
    }
}