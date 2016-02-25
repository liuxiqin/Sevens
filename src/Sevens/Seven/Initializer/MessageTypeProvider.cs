using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seven.Infrastructure.Exceptions;
using Seven.Message;

namespace Seven.Initializer
{
    public class MessageTypeProvider : IApplictionInitializer
    {
        private IDictionary<string, Type> _messageTypeDics;

        public MessageTypeProvider()
        {
            _messageTypeDics = new Dictionary<string, Type>();
        }

        public void Initialize(params System.Reflection.Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var types = from type in assembly.GetTypes()
                            where typeof(IMessage).IsAssignableFrom(type)
                            select type;
                foreach (var type in types)
                {
                    _messageTypeDics.Add(type.FullName, type);
                }
            }
        }

        public Type GetType(string typeName)
        {
            if (_messageTypeDics.ContainsKey(typeName))
            {
                return _messageTypeDics[typeName];
            }

            throw new FrameworkException("can not find the type with the name of " + typeName);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
