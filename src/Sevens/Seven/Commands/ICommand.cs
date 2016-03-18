using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Message;

namespace Seven.Commands
{
    [Serializable]
    public class Command : ICommand
    {
        public void Execute()
        {
            throw new NotImplementedException();
        }

        public string CommandId
        {
            get { return Guid.NewGuid().ToString(); }
        }

        public string MessageId
        {
            get { return CommandId; }
        }

        public long Version
        {
            get { return 1; }
        }
    }



    public interface ICommand : IMessage
    {
        string CommandId { get; }

        void Execute();
    }
}
