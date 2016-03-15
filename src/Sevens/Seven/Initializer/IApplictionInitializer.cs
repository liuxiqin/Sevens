using System;
using System.Reflection;

namespace Seven.Initializer
{
    public interface IApplictionInitializer
    {
        void Initialize(params Assembly[] assemblies);
    }
}