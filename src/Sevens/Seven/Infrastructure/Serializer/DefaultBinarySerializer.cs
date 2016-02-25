using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Infrastructure.Serializer
{
    /// <summary>
    /// C# 默认的二进制数据序列化与反序列化
    /// </summary>
    public class DefaultBinarySerializer : IBinarySerializer
    {
        private BinaryFormatter _binaryFormatter;

        public DefaultBinarySerializer()
        {
            _binaryFormatter = new BinaryFormatter();
        }

        public byte[] Serialize<T>(T obj)
        {
            if (obj == null)
            {
                return null;
            }

            using (var stream = new MemoryStream())
            {
                _binaryFormatter.Serialize(stream, obj);
                return stream.ToArray();
            }
        }

        public object Deserialize(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                return _binaryFormatter.Deserialize(stream);
            }
        }

        public T Deserialize<T>(byte[] data) where T : class
        {
            using (var stream = new MemoryStream(data))
            {
                return _binaryFormatter.Deserialize(stream) as T;
            }
        }
    }
}
