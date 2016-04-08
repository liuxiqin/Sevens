
namespace Seven.Messages.Pipelines
{
    /// <summary>
    /// 消息错误处理 
    /// 业务处理，返回结果，
    /// 系统错误，进行重试操作
    /// </summary>
    public class ErrorMessageHandler : IMessageHandler
    {
        public void Handle(MessageContext message)
        {
            message.SetResponse(new MessageHandleResult() { Status = MessageStatus.Fail, Message = message.Exception.Message });
        }
    }
}
