using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace Seven.Messages
{
    public class AsyncHandleResult
    {
        public string Message { get; private set; }

        public HandleStatus Status { get; private set; }

        public AsyncHandleResult(HandleStatus status, string message = null)
        {
            Status = status;
            Message = message;
        }
    }

    public enum HandleStatus
    {
        Success,
        Failure,
        Warnning
    }
}
