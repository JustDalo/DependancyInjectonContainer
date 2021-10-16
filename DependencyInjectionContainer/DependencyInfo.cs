using System;
using System.Runtime.CompilerServices;

namespace DependencyInjectionContainer
{
    public class DependencyInfo
    {
        public Type ImplementationType;
        public bool IsSingleton;

        public object GetInstance(DependencyProvider provider)
        {
            return provider.Resolve(ImplementationType);
        }
    }
}