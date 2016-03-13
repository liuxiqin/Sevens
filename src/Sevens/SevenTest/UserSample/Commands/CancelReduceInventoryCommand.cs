using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Commands;

namespace SevenTest.UserSample.Commands
{
    public class CancelReduceInventoryCommand : Command
    {
        public string AggregateRootId { get; set; }

        public int Quantity { get;  set; }
    }
}
