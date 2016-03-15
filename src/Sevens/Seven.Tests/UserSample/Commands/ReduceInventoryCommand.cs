using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Commands;

namespace Seven.Tests.UserSample.Commands
{
    public class ReduceInventoryCommand : Command
    {
        public string ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
