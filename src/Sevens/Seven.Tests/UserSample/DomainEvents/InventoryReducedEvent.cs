using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Events;

namespace Seven.Tests.UserSample.DomainEvents
{
    [Serializable]
    public class InventoryReducedEvent : IDomainEvent
    {
        public int Quantity { get; private set; }

        public InventoryReducedEvent(int quantity)
        {
            Quantity = quantity;
        }
    }
}
