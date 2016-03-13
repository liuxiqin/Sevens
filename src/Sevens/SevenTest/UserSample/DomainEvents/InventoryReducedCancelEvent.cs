using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Events;

namespace SevenTest.UserSample.DomainEvents
{
    [Serializable]
    public class InventoryReducedCancelEvent : IDomainEvent
    {
        public int Quantity { get; private set; }

        public InventoryReducedCancelEvent(int quantity)
        {
            Quantity = quantity;
        }
    }
}
