using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seven.Message
{
    /// <summary>
    /// 消息消费者
    /// </summary>
    public interface IMessageConsumer
    {
        void Start();

        void Stop();

        /// <summary>
        /// Get the message handle result.
        /// </summary>
        /// <param name="responseQueueName"></param>
        /// <param name="correlationId"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        MessageHandleResult SubscribeResult(string responseQueueName, string correlationId, TimeSpan timeout);
    }

    public class MessageConsumerManager
    {

    }
}
