using System;
using System.Collections.Generic;

namespace DependencyInjectionContainer.DependenciesConfiguration.DependenciesConfigurationImpl
{
    public class DependenciesConfiguration
    {
        public readonly Dictionary<Type, List<DependencyInfo>> Dependencies;

        public DependenciesConfiguration()
        {
            Dependencies = new Dictionary<Type, List<DependencyInfo>>();
        }

        public void Register<TDependency, TImplementation>(bool isSingleton = false) 
            where TDependency : class 
            where TImplementation : TDependency
        {
            RegisterDependency(typeof(TDependency), typeof(TImplementation), isSingleton);
        }

        public void Register(Type dependency, Type implementation, bool isSingleton = false)
        {
            RegisterDependency(dependency, implementation, isSingleton);
        }

        private void RegisterDependency(Type dependency, Type implementation, bool isSingleton)
        {
            if (!Dependencies.ContainsKey(dependency))
            {
                Dependencies[dependency] = new List<DependencyInfo>();
            }
            DependencyInfo dependencyInfo = new DependencyInfo()
            {
                ImplementationType = implementation,
                IsSingleton = isSingleton,
            };
            
            if (Dependencies[dependency].IndexOf(dependencyInfo) == -1)
            {
                Dependencies[dependency].Add(dependencyInfo);
            }
        }
    }
}