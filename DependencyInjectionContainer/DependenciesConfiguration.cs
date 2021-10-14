using System;
using System.Collections.Generic;

namespace DependencyInjectionContainer
{
    public class DependenciesConfiguration
    {
        public Dictionary<Type, List<Type>> Dependencies;

        public DependenciesConfiguration()
        {
            Dependencies = new Dictionary<Type, List<Type>>();
        }

        public void Register<TDependency, TImplementation>(bool isSingleton = true) where TDependency : class where TImplementation : class
        {
            if (typeof(TDependency).IsInterface)
            {
                if (!Dependencies.ContainsKey(typeof(TDependency)))
                {
                    Dependencies[typeof(TDependency)] = new List<Type>();
                }

                if (Dependencies[typeof(TDependency)].IndexOf(typeof(TImplementation) ) == -1)
                {
                    Dependencies[typeof(TDependency)].Add(typeof(TImplementation));
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void Register(Type dependency, Type implementation, bool isSingleton = true)
        {
            
        }
    }
}