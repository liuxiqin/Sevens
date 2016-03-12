namespace Seven.Infrastructure.Exceptions
{
    /// <summary>
    /// 系统业务错误
    /// </summary>
    public class BusinessException : SevenException
    {
        public BusinessException(string message) : base(message)
        {

        }
    }
}