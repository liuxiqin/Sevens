using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Commands;

namespace SevenTest.UserSample.Commands
{
    public class CreatedOrderCancelCommand : Command
    {
        public string AggregateRootId { get; set; }
    }

}
