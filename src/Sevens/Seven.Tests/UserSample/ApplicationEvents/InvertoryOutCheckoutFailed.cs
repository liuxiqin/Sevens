using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Events;

namespace Seven.Tests.UserSample.ApplicationEvents
{
    [Serializable]
    public class InvertoryOutCheckoutFailed : ApplicationEvent
    {
        public string ProductId { get; private set; }

        public InvertoryOutCheckoutFailed(string productId)
        {
            ProductId = productId;
        }
    }
}
