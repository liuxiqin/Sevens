using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Infrastructure.UniqueIds;

namespace Seven.Events
{
    [Serializable]
    public abstract class ApplicationEvent : IEvent
    {
        public readonly string Id;

        public ApplicationEvent()
        {
            Id = ObjectId.NewObjectId();
        }

        public string MessageId
        {
            get { return Id; }
        }
    }
}
