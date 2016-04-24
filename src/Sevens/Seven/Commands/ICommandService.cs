using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Messages;

namespace Seven.Commands
{
    public interface ICommandService
    {
        MessageHandleResult Send(ICommand command, int timeoutSeconds = 10);

        Task SendAsync(ICommand command);
    }
}
