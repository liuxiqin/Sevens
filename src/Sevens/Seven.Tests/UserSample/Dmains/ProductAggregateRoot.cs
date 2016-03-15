using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Seven.Aggregates;
using Seven.Tests.UserSample.ApplicationEvents;
using Seven.Tests.UserSample.DomainEvents;

namespace Seven.Tests.UserSample.Dmains
{
    public class ProductAggregateRoot : AggregateRoot
    {
        public string ProductName { get; private set; }

        public int Inventory { get; private set; }

        public decimal Price { get; private set; }

        public decimal Discount { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public ProductAggregateRoot(string aggregateRootId) : base(aggregateRootId)
        {

        }

        public void ReduceInventory(int quantity)
        {
            if (Inventory < quantity)//库存不足
                ApplyEvent(new InvertoryOutCheckoutFailed(AggregateRootId));

            ApplyEvent(new InventoryReducedEvent(quantity));
        }

        public void CancelInventoryReduced(int quantity)
        {

        }

        private void Handle(InventoryReducedEvent evnt)
        {
            Inventory = Inventory - evnt.Quantity;
        }

        private void Handle()
        {
            
        }
    }
}
