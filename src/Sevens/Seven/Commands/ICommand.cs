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
        private string _id;

        public int RetryCount { get; set; }

    

        public string AggerateRootId
        {
            get { return _id; }
        }

        public Command()
        {
            _id = ObjectId.NewObjectId();
            RetryCount = 5;
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }

        public string CommandId
        {
            get { return _id; }
        }

        public string MessageId
        {
            get { return _id; }
        }

        protected long Version { get; set; }
    }



    public interface ICommand : IMessage
    {
        string CommandId { get; }

        string AggerateRootId { get; }

        void Execute();

        int RetryCount { get; set; }
    }
}
