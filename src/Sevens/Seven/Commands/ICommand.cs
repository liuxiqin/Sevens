using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Infrastructure.UniqueIds;
using Seven.Messages;

namespace Seven.Commands
{
    [Serializable]
    public class Command : ICommand
    {
        public string Id;

        public string AggerateRootId
        {
            get { return Id; }
        }

        public Command()
        {
            Id = ObjectId.NewObjectId();
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

        string AggerateRootId { get; }

        void Execute();
    }
}
