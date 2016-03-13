using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Commands;

namespace SevenTest.UserSample.Commands
{
    public class ReduceInventoryCommand : Command
    {
        public string ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
