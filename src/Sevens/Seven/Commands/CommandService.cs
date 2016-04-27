using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Infrastructure.Serializer;
using Seven.Infrastructure.UniqueIds;
using Seven.Messages;
using Seven.Messages.Channels;
using Seven.Messages.QueueMessages;

namespace Seven.Commands
{
    /// <summary>
    /// Command Send
    /// </summary>
    public class CommandService : ICommandService
    {
        private int _queueCount = 20;

        private RequestChannelPools _requestChannelPools;

        public CommandService(RequestChannelPools requestChannelPools)
        {
            _requestChannelPools = requestChannelPools;
        }

        public MessageHandleResult Send(ICommand command, int timeoutSeconds = 10)
        {
            var messageWrapper = BuildMessage(command);

            var rquestChannel =
                _requestChannelPools.GetRequestChannel(new RequestMessageContext(messageWrapper.ExchangeName,
                    messageWrapper.RoutingKey, messageWrapper.ResponseRoutingKey));

            var replyTask = rquestChannel.SendMessage(messageWrapper, timeoutSeconds);

            return replyTask.Result.GetResult();
        }

        public async Task SendAsync(ICommand command)
        {
            var messageWrapper = BuildMessage(command, false);

            var rquestChannel =
                _requestChannelPools.GetRequestChannel(new RequestMessageContext(messageWrapper.ExchangeName,
                    messageWrapper.RoutingKey, messageWrapper.ResponseRoutingKey));

            await rquestChannel.SendMessageAsync(messageWrapper);
        }

        private MessageWrapper BuildMessage(ICommand message, bool isRpcInvoke = true)
        {

            var exchangeName = message.GetType().Assembly.GetName().Name;

            var routingKey = string.Format("{0}_{1}_{2}", exchangeName, "command", message.MessageId.GetHashCode() % _queueCount);

            return new MessageWrapper()
            {
                MessageId = ObjectId.NewObjectId(),
                RoutingKey = routingKey,
                ExchangeName = exchangeName,
                Message = message,
                TypeName = message.GetType().FullName,
                MessageType = MessageType.Reply,
                ResponseRoutingKey = MessageUtils.CurrentResponseRoutingKey,
                IsRpcInvoke = isRpcInvoke
            };
        }
    }
}
