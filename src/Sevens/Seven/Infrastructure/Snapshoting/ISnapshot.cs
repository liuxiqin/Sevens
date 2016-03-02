using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Aggregates;

namespace Seven.Infrastructure.Snapshoting
{
    /// <summary>
    /// 快照
    /// </summary>
    /// <typeparam name="TAggregateRoot"></typeparam>
    public class Snapshot<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
        public Snapshot(TAggregateRoot aggregateRoot)
        {
            AggregateRootId = aggregateRoot.AggregateRootId;
            Version = aggregateRoot.Version;
            AggregateRoot = aggregateRoot;
        }

        public string AggregateRootId { get; set; }

        public int Version { get; set; }

        public TAggregateRoot AggregateRoot { get; set; }
    }
}
