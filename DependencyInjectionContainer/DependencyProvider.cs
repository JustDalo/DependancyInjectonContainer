using System;
using System.ComponentModel.Design;
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
            if (_dependenciesConfiguration.Dependencies.ContainsKey(dependencyType))
            {
                return Resolution(_dependenciesConfiguration.Dependencies[dependencyType][0]);
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