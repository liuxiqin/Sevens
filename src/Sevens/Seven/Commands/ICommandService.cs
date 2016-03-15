using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Commands
{
    public interface ICommandService
    {
        Task<CommandExecutedResult> Send(ICommand command);

        void SendAsync(ICommand command);
    }
}
