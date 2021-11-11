using System;

namespace DependencyInjectionContainer.DependenciesConfiguration
{
    public interface IDependencyConfiguration
    {
        void Register<TDependency, TImplementation>(bool isSingleton = false)
            where TDependency : class
            where TImplementation : TDependency;
        void Register(Type dependency, Type implementation, bool isSingleton = false);
        
    }
}