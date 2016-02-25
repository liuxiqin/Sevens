using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Pipeline;
using Seven.Commands;

namespace Seven.Message
{
    public interface IMessgaeExecute
    {
        void Execute(IMessageContext messageContext);
    }

    public class DefaultMessgaeExecute : IMessgaeExecute
    {
        private IMessageHandler _messageHandler = new ProcessMessageHandler();

        public void Execute(IMessageContext messageContext)
        {
            var context = (MessageContext)messageContext;

            _messageHandler.Execute(context);
        }
    }
}
