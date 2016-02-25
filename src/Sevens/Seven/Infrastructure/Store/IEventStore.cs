using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seven.Infrastructure.Store
{
    public interface IEventStore
    {
        void Add();

        void GetById();
    }
}
