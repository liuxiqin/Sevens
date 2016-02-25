using System;
using System.Reflection;

namespace Seven.Initializer
{
    public interface IApplictionInitializer : IDisposable
    {
        void Initialize(params Assembly[] assemblies);
    }
}