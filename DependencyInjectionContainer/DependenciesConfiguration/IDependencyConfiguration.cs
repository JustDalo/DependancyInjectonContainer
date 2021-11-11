using System;
using System.Collections.Generic;

namespace DependencyInjectionContainer.DependenciesConfiguration
{
    public interface IDependencyConfiguration
    {
        Dictionary<Type, List<DependencyInfo>> Dependencies { get;  }

        void Register<TDependency, TImplementation>(bool isSingleton = false)
            where TDependency : class
            where TImplementation : TDependency;
        void Register(Type dependency, Type implementation, bool isSingleton = false);
        
    }
}