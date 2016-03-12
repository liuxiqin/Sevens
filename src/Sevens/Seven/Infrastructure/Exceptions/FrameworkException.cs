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
}