using System;
using System.Linq;
using Seven.Infrastructure.Exceptions;

namespace Seven.Initializer
{
    public class TopicProvider : IApplictionInitializer
    {
        public void Initialize(params System.Reflection.Assembly[] assemblies)
        {

        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}