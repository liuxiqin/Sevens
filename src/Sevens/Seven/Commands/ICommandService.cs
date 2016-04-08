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
        CommandExecutedResult Send(ICommand command);

        void SendAsync(ICommand command);

        Task<MessageHandleResult> AsyncSend(ICommand command);
    }
}
