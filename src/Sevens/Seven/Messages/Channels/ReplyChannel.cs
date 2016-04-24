using System;
using System.Threading;
using Seven.Infrastructure.Exceptions;
using System.Threading.Tasks;

namespace Seven.Messages.Channels
{
    public class ReplyChannel : IReplyChannel
    {
        private ManualResetEventSlim _manualReset;

        private readonly TimeSpan _timeout;

        private readonly string _messageId;
        
        private MessageHandleResult _result;

        public ReplyChannel(string messageId, TimeSpan timeout)
        {
            _messageId = messageId;
            _timeout = timeout;
            _manualReset = new ManualResetEventSlim(false);

        }

        public MessageHandleResult GetResult()
        {
            var noTimeout = _manualReset.Wait(_timeout);

            if (!noTimeout)
                return new MessageHandleResult(_messageId, "has timeout.", MessageStatus.Timeout);

            if (_result == null)
                return new MessageHandleResult(_messageId, "no answer.", MessageStatus.Timeout);

            return _result;
        }

        public void SetResult(MessageHandleResult result)
        {
            _result = result;
            _manualReset.Set();

        }
    }
}