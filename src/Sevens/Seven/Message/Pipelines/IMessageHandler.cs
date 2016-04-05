
namespace Seven.Message.Pipelines
{
    public interface IMessageHandler
    {
        void Handle(MessageContext message);
    }
}
