using System;
using System.Threading;
using Seven.Infrastructure.Exceptions;

namespace Seven.Message
{
    public class ReplyChannel : IReplyChannel
    {
        private ManualResetEventSlim _manualReset;

        private TimeSpan _timeout;
        private string _messageId { get; set; }

        public ReplyChannel(string messageId, TimeSpan timeout)
        {
            _messageId = messageId;
            _timeout = timeout;
            _manualReset = new ManualResetEventSlim(false);
        }


        private MessageHandleResult _result;

        public MessageHandleResult GetResult()
        {
            var hasTimeout = _manualReset.Wait(_timeout);

            if (!hasTimeout)
            {
                throw new FrameworkTimeoutException("Get the message result has time out.");
            }
            return _result;
        }

        public void SetResult(MessageHandleResult result)
        {
            _result = result;
            _manualReset.Set();

        }
    }
}