using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DependencyInjectionContainer
{
    public class DependencyProvider
    {
        private readonly DependenciesConfiguration _dependenciesConfiguration;
        private readonly Dictionary<Type, object> _singletons;
        public DependencyProvider(DependenciesConfiguration dependenciesConfiguration)
        {
            _dependenciesConfiguration = dependenciesConfiguration;
            _singletons = new Dictionary<Type, object>();
        }

        public TDependency Resolve<TDependency>() where TDependency : class
        {
            return (TDependency) Resolve(typeof(TDependency));
        }

        private object Resolve(Type dependencyType)
        {
            if (_dependenciesConfiguration.Dependencies.ContainsKey(dependencyType))
            {
                if (_dependenciesConfiguration.Dependencies[dependencyType][0].IsSingleton)
                {
                    if (_singletons.ContainsKey(dependencyType))
                    {
                        return _singletons[dependencyType];
                    }
                    else
                    {
                        var singletonResolution = Resolution(_dependenciesConfiguration.Dependencies[dependencyType][0]
                            .ImplementationType);
                        _singletons.Add(dependencyType, singletonResolution);
                        return singletonResolution;
                    }

                }
            }

            if (_dependenciesConfiguration.Dependencies.ContainsKey(dependencyType))
            {
                return Resolution(_dependenciesConfiguration.Dependencies[dependencyType][0].ImplementationType);
            }
            
            //if return type has to be IEnumerable
            if (dependencyType.IsGenericType && dependencyType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                object result = Activator.CreateInstance(typeof(List<>).MakeGenericType(dependencyType.GenericTypeArguments[0]));
                foreach (var item in _dependenciesConfiguration.Dependencies[dependencyType.GenericTypeArguments[0]])
                {
                    ((IList) result)?.Add(Resolution(item.ImplementationType));
                }

                return result;
            }
            return null;
        }

        private object Resolution(Type dependencyType)
        {
            ConstructorInfo[] constructors = dependencyType.GetConstructors();
            ConstructorInfo constructor = constructors[0];

            ParameterInfo[] parameters = constructor.GetParameters();
            object[] tmp = new object[parameters.Length];
            int i = 0;

            foreach (var parameter in parameters)
            {
                if (_dependenciesConfiguration.Dependencies.ContainsKey(parameter.ParameterType))
                {
                    tmp[i++] = Resolve(parameter.ParameterType);
                }
                else
                {
                    if (_dependenciesConfiguration.Dependencies.ContainsKey(
                        _dependenciesConfiguration.Dependencies.First(x =>
                            false).Key))
                    {
                        tmp[i++] = Resolve(_dependenciesConfiguration.Dependencies.First(x =>
                            false).Key);
                    }
                }
            }

            var result = constructor.Invoke(tmp);
            return result;
         
        }
    }
    
    
}