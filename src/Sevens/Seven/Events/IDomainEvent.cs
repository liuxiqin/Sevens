using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Infrastructure.UniqueIds;

namespace Seven.Events
{
    [Serializable]
    public abstract class IDomainEvent : IEvent
    {
        public readonly string Id;

        public IDomainEvent()
        {
            Id = ObjectId.NewObjectId();
        }

        public string MessageId
        {
            get { return Id; }
        }
    }
}
