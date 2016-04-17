using System;
using Seven.Infrastructure.Persistence;

namespace Seven.Infrastructure.Snapshoting
{
    public class SnapshotRecord
    {
        public string AggregateRootId { get; set; }

        public int Versions { get; set; }

        public byte[] Datas { get; set; }

        public DateTime TimeStamp { get; set; }

        public SnapshotRecord(string aggregateRootId, int version, byte[] datas)
        {
            AggregateRootId = aggregateRootId;
            Versions = version;
            Datas = datas;
            TimeStamp = DateTime.Now;
        }
    }
}