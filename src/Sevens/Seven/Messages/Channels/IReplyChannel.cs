using System.Threading.Tasks;

namespace Seven.Messages.Channels
{
    public interface IReplyChannel
    {
        MessageHandleResult GetResult();

        void SetResult(MessageHandleResult messageHandleResult);
    }
}