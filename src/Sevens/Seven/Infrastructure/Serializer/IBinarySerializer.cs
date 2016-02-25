using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seven.Infrastructure.Serializer
{
    public interface IBinarySerializer
    {
        byte[] Serialize<T>(T obj);

        object Deserialize(byte[] data);

        T Deserialize<T>(byte[] data) where T : class;
    }
}
