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
    public class Snapshot
    {
        public Snapshot(IAggregateRoot aggregateRoot)
        {
            AggregateRootId = aggregateRoot.AggregateRootId;
            Version = aggregateRoot.Version;
            AggregateRoot = aggregateRoot;
        }

        public string AggregateRootId { get; set; }

        public int Version { get; set; }

        public IAggregateRoot AggregateRoot { get; set; }
    }
}
