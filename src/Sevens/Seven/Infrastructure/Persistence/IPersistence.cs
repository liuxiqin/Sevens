using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Events;

namespace Seven.Infrastructure.Persistence
{
    public interface IPersistence
    {
        EventStream Get(string aggregateRootId);

        EventStream Get(string aggregateRootId, int version);

        void Append(string aggregateRootId, int version, IList<IEvent> events);
    }


    public class EventStoreModel
    {
        public string AggregateRootId { get; set; }

        public int Version { get; set; }

        /// <summary>
        /// 此处采用json格式存储
        /// </summary>
        public string Datas { get; set; }
    }
}
