
namespace Seven.Messages.Pipelines
{
    public interface IMessageHandler
    {
        void Handle(MessageContext message);
    }
}
