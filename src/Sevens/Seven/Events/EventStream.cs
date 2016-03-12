using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Events
{
    public class EventStream
    {
        public int Version { get; set; }

        public IList<IEvent> Events { get; set; }

        public EventStream(int version, IList<IEvent> events)
        {
            Version = version;
            Events = events;
        }
    }

}
