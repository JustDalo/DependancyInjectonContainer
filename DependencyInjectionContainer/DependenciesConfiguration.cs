using System;
using System.Collections.Generic;

namespace DependencyInjectionContainer
{
    public class DependenciesConfiguration
    {
        public Dictionary<Type, List<DependencyInfo>> Dependencies;

        public DependenciesConfiguration()
        {
            Dependencies = new Dictionary<Type, List<DependencyInfo>>();
        }

        public void Register<TDependency, TImplementation>(bool isSingleton = true) 
            where TDependency : class 
            where TImplementation : TDependency
        {
            if (!Dependencies.ContainsKey(typeof(TDependency)))
            {
                Dependencies[typeof(TDependency)] = new List<DependencyInfo>();
            }
            DependencyInfo dependencyInfo = new DependencyInfo()
            {
                ImplementationType = typeof(TImplementation),
                IsSingleton = isSingleton,
            };
            if (Dependencies[typeof(TDependency)].IndexOf(dependencyInfo) == -1)
            {
                Dependencies[typeof(TDependency)].Add(dependencyInfo);
            }
        }

        public void Register(Type dependency, Type implementation, bool isSingleton = true)
        {
            
        }
    }
}