using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Seven.Infrastructure.Serializer
{
    public class DefaultJsonSerializer : IJsonSerializer
    {
        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public object DeSerialize(string data, Type type)
        {
            return JsonConvert.DeserializeObject(data, type);
        }


        public T Deserialize<T>(string data) where T : class
        {
            return JsonConvert.DeserializeObject<T>(data);
        }


    }
}
