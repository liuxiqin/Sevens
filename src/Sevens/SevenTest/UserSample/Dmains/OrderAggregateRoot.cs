using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Aggregates;
using Seven.Infrastructure.Persistence;
using SevenTest.UserSample.DomainEvents;

namespace SevenTest.UserSample.Dmains
{
    public class OrderAggregateRoot : AggregateRoot
    {
        public string OrderNumber { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public int TotalQuantity { get; private set; }

        public decimal TotalAmount { get; private set; }

        public decimal TotalShippingFee { get; private set; }

        public decimal TotalDiscountAmount { get; private set; }

        public Address Address { get; private set; }

        public IList<OrderItem> OrderItems { get; private set; }

        public bool IsEnabled { get; private set; }
        
        public OrderAggregateRoot(string aggregateRootId) : base(aggregateRootId)
        {

        }

        public void DestroyOrder()
        {

        }

        private void Handle(OrderDestroyedEvent evnt)
        {
            IsEnabled = true;
        }
    }

    public class Address
    {
        public string StreeName { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public string DetailAdress { get; set; }

        public string ReceivePerson { get; set; }

        public string ReceivePhone { get; set; }
    }

    public class OrderItem
    {
        public string OrderId { get; set; }

        public string OrderItemId { get; set; }

        public string ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Discount { get; set; }

        public string ProductName { get; set; }
    }
}
