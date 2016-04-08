using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Events
{
    [Serializable]
    public abstract class IDomainEvent : IEvent
    {
        public readonly string Id;

        public IDomainEvent()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string MessageId
        {
            get { return Id; }
        }
    }
}
