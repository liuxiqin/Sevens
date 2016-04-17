using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Infrastructure.Snapshoting
{
    public interface ISnapshotStorage
    {
        void Create(SnapshotRecord snapshot);

        Task<SnapshotRecord> GetLastestSnapshot(string aggregateRootId);
    }
}
