using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Events;
using Seven.Tests.UserSample.ApplicationEvents;
using Seven.Tests.UserSample.DomainEvents;

namespace Seven.Tests.UserSample.EventHandlers
{
    public class OrderEventHandlers
        : IEventHandler<OrderCreated>,
        IEventHandler<OrderDestroyedEvent>,
        IEventHandler<InventoryReducedCancelEvent>,
        IEventHandler<InventoryReducedEvent>
    {
        public void Handle(OrderCreated evnt)
        {
            throw new NotImplementedException();
        }

        public void Handle(OrderDestroyedEvent evnt)
        {
            throw new NotImplementedException();
        }

        public void Handle(InventoryReducedCancelEvent evnt)
        {
            throw new NotImplementedException();
        }

        public void Handle(InventoryReducedEvent evnt)
        {
            throw new NotImplementedException();
        }
    }
}
