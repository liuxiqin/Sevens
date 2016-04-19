using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Commands
{
    public class CommandExecutedResult
    {
        public string Message { get; set; }

        public CommandExecutedStatus Status { get; set; }
    }
}
