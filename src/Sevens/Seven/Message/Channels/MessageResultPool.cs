using System.Collections.Concurrent;

namespace Seven.Message.Channels
{
    public class MessageResultPool
    {
        private readonly ConcurrentDictionary<string, MessageHandleResult> _messageHandleResults;

        public MessageResultPool()
        {
            _messageHandleResults = new ConcurrentDictionary<string, MessageHandleResult>();
        }

        public bool TryAdd(string messageId, MessageHandleResult messageHandleResult)
        {
            return _messageHandleResults.TryAdd(messageId, messageHandleResult);
        }

        public MessageHandleResult GetResult(string messageId)
        {
            var messageHandleResult = default(MessageHandleResult);

            if (_messageHandleResults.ContainsKey(messageId))
            {
                _messageHandleResults.TryRemove(messageId, out messageHandleResult);
            }
            return messageHandleResult;
        }
    }
}
