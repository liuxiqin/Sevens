using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Infrastructure.Snapshoting
{
    public interface ISnapshotStorage
    {
        bool Create(SnapshotRecord snapshot);

        Task<SnapshotRecord> GetLastestSnapshot(string aggregateRootId);
    }
}
