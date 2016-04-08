using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Events
{
    public interface IEventProssor
    {
        void Execute(IEvent evnt);
    }

    public class DefaultEventProssor : IEventProssor
    {
        public void Execute(IEvent evnt)
        {

        }
    }
}
