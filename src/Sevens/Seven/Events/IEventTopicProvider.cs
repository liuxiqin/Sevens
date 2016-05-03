using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Events
{
    public interface IEventTopicProvider
    {
        void Register(Type commandType, string topic);

        void Register<TEvent>(string topic) where TEvent : IEvent;

        string GetTopic(Type commanType);

        string GetTopic<TEvent>() where TEvent : IEvent;

        void Initialize();
    }
}
