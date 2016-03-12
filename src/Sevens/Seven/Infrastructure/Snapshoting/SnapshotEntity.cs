using Seven.Infrastructure.Persistence;

namespace Seven.Infrastructure.Snapshoting
{
    public class SnapshotEntity : EntityBase
    {
        public string AggregateRootId { get;  set; }

        public int Versions { get;  set; }

        public byte[] Datas { get;  set; }

        public SnapshotEntity(string aggregateRootId, int version, byte[] datas)
        {
            AggregateRootId = aggregateRootId;
            Versions = version;
            Datas = datas;
        }

        public SnapshotEntity()
        {
            
        }
    }
}