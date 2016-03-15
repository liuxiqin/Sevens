using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Domains;

namespace Seven.Tests.UserSample.Dmains
{
    public class InventoryService : IDomainService
    {
        public bool OutOfStock(IList<string> productId)
        {
            return true;
        }
    }
}
