using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Commands
{
    public class ProcessCommand
    {
        public ICommandContext CommandContext { get; private set; }

        public ICommand Command { get; private set; }

        public ProcessCommand(ICommandContext commandContext, ICommand command)
        {
            CommandContext = commandContext;
            Command = command;
        }

        public Type GetCommandType
        {
            get { return Command.GetType(); }
        }
    }
}
