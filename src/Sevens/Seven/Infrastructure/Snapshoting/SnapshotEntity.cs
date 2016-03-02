using Seven.Infrastructure.Persistence;

namespace Seven.Infrastructure.Snapshoting
{
    public class SnapshotEntity : EntityBase
    {
        public string AggregateRootId { get; private set; }

        public int Version { get; private set; }

        public byte[] Datas { get; private set; }

        public SnapshotEntity(string aggregateRootId, int version, byte[] datas)
        {
            AggregateRootId = aggregateRootId;
            Version = version;
            Datas = datas;
        }
    }
}