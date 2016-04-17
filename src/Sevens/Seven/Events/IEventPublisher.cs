using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Messages;

namespace Seven.Events
{
    public interface IEventPublisher
    {
        Task<AsyncHandleResult> Publish(IEvent evnt);

        Task<AsyncHandleResult> PublishAsync(IEvent evnt);

        Task<AsyncHandleResult> PublishAsync(IList<IEvent> events);
    }
}
