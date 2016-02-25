using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf.Meta;
using ProtoBuf.Serializers;

namespace Seven.Infrastructure.Serializer
{
    /// <summary>
    /// ProtoBuf.Net 第三方工具进行序列化与反序列化
    /// </summary>
    public class ProtoBufBinarySerializer : IBinarySerializer
    {
        public byte[] Serialize<T>(T obj)
        {
            using (var stream = new MemoryStream())
            {
                RuntimeTypeModel.Default.Serialize(stream, obj);

                return stream.ToArray();
            }
        }


        public object Deserialize(byte[] data)
        {
            throw new NotImplementedException();
        }

        public T Deserialize<T>(byte[] data) where T : class
        {
            using (var stream = new MemoryStream(data))
            {
                return ProtoBuf.Serializer.Deserialize<T>(stream);
            }
        }
    }
}
