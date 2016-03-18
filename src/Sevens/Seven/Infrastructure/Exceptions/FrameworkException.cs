namespace Seven.Infrastructure.Exceptions
{
    /// <summary>
    /// 系统框架错误
    /// </summary>
    public class FrameworkException : SevenException
    {
        public FrameworkException(string message) : base(message)
        {

        }
    }

    /// <summary>
    /// 超时处理
    /// </summary>
    public class FrameworkTimeoutException : FrameworkException
    {
        public FrameworkTimeoutException(string message = "throw the exception of FrameworkTimeoutException.") : base(message)
        {

        }
    }
}