using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace DependencyInjectionContainer
{
    public class DependencyProvider
    {
        private DependenciesConfiguration _dependenciesConfiguration;
        public DependencyProvider(DependenciesConfiguration dependenciesConfiguration)
        {
            _dependenciesConfiguration = dependenciesConfiguration;
        }

        public TDependency Resolve<TDependency>() where TDependency : class
        {
            return (TDependency) Resolve(typeof(TDependency));
        }

        private object Resolve(Type dependencyType)
        {
            //if only one implementation
            if (_dependenciesConfiguration.Dependencies.ContainsKey(dependencyType) && _dependenciesConfiguration.Dependencies[dependencyType].Count == 1)
            {
                return Resolution(_dependenciesConfiguration.Dependencies[dependencyType][0].ImplementationType);
            }
            
            //if return type has to be IEnumerable
            if (dependencyType.IsGenericType && dependencyType.GetGenericTypeDefinition().Equals(typeof(IEnumerable<>)))
            {
                object result = Activator.CreateInstance(typeof(List<>).MakeGenericType(dependencyType.GenericTypeArguments[0]));
                foreach (var item in _dependenciesConfiguration.Dependencies[dependencyType.GenericTypeArguments[0]])
                {
                    ((IList) result).Add(Resolution(item.ImplementationType));
                }

                return result;
            }
        
            return null;
            
        }

        private object Resolution(Type dependencyType)
        {
            ConstructorInfo[] constructors = dependencyType.GetConstructors();
            ConstructorInfo constructor = constructors[0];
            if (constructor != null)
            {
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
                                x.Value[0].Equals((parameter.ParameterType))).Key))
                        {
                            tmp[i++] = Resolve(_dependenciesConfiguration.Dependencies.First(x =>
                                x.Value[0].Equals((parameter.ParameterType))).Key);
                        }
                    }
                }

                var result = constructor.Invoke(tmp);
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}