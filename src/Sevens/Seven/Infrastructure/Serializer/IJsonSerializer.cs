using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Infrastructure.Serializer
{
    public interface IJsonSerializer
    {
        string Serialize<T>(T obj);

        object DeSerialize(string data, Type type);

        T Deserialize<T>(string data) where T : class;

    }
}
