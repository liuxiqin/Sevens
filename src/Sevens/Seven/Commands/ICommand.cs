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
        public string Id;

        public Command()
        {
            Id = Guid.NewGuid().ToString();
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }

        public string CommandId
        {
            get { return Id; }
        }

        public string MessageId
        {
            get { return Id; }
        }

        protected long Version { get; set; }
    }



    public interface ICommand : IMessage
    {
        string CommandId { get; }

        void Execute();
    }
}
