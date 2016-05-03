using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Messages;

namespace Seven.Commands
{
    public interface ICommandTopicProvider
    {
        void Register(Type commandType, string topic);

        void Register<TMessage>(string topic) where TMessage : IMessage;

        string GetTopic(Type commanType);

        string GetTopic<TMessage>() where TMessage : IMessage;

        void Initialize();
    }
}
