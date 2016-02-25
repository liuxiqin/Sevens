using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Commands
{
    public class DefaultCommandExecuted : ICommandExecute
    {
      ////  private IDictionary<string, ICommandHanlder> _commandHanlders;

      //  public DefaultCommandExecuted(IDictionary<string, ICommandHanlder> commandHanlders)
      //  {
      //      _commandHanlders = commandHanlders;
      //  }

      //  public void Executed(ProcessCommand command)
      //  {
      //      var commandHandler = GetCommandHanlder(command);

      //      commandHandler.Handler(command.Command);
      //  }


      //  private ICommandHanlder GetCommandHanlder(ProcessCommand command)
      //  {
      //      var key = command.GetType().ToString();

      //      if (_commandHanlders.ContainsKey(key))
      //      {
      //          return _commandHanlders[key];
      //      }
      //      throw new ApplicationException("can not find the commandhandler from the dictionary of CommandHanlders");
      //  }
        public void Executed(ProcessCommand command)
        {
            throw new NotImplementedException();
        }
    }


}
