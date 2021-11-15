using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DependencyInjectionContainer.DependenciesConfiguration;


namespace DependencyInjectionContainer.DependencyProvider.DependencyProviderImpl
{
    public class DependencyProvider : IDependencyProvider
    {
        private readonly IDependencyConfiguration _dependenciesConfiguration;
        private readonly Dictionary<Type, object> _singletons;

        public DependencyProvider(IDependencyConfiguration dependenciesConfiguration)
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
            if (dependencyType.IsGenericType && dependencyType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return ResolveIEnumerable(dependencyType);
            return ResolveNotIEnumerable(dependencyType);
        }

        private object ResolveNotIEnumerable(Type dependencyType)
        {
            if (!_dependenciesConfiguration.Dependencies.ContainsKey(dependencyType))
                throw new ArgumentException("Dependency isn't registered");
            if (_dependenciesConfiguration.Dependencies[dependencyType][0].IsSingleton)
            {
                if (_singletons.ContainsKey(dependencyType))
                {
                    return _singletons[dependencyType];
                }

                var singletonResolution = CreateInstance(_dependenciesConfiguration.Dependencies[dependencyType][0]
                    .ImplementationType);
                _singletons.Add(dependencyType, singletonResolution);
                return singletonResolution;
            }

            var resolution = CreateInstance(_dependenciesConfiguration.Dependencies[dependencyType][0]
                .ImplementationType);
            return resolution;
        }

        private object ResolveIEnumerable(Type dependencyType)
        {
            var implementationList =
                (IList) Activator.CreateInstance(typeof(List<>).MakeGenericType(dependencyType.GetGenericArguments()[0]));

            var implementationsContainers =
                _dependenciesConfiguration.Dependencies[dependencyType.GetGenericArguments()[0]];
            foreach (var implementationContainer in implementationsContainers)
            {
                var instance = ResolveNotIEnumerable(dependencyType.GetGenericArguments()[0]);
                implementationList?.Add(instance);
            }

            return implementationList;

            throw new ArgumentException(" error");
        }

        private object CreateInstance(Type implementationType)
        {
            var constructors = implementationType.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters();
                var generatedParameters = new List<dynamic>();
                foreach (var parameter in parameters)
                {
                    dynamic createdParameter;
                    if (parameter.ParameterType.IsInterface)
                    {
                        createdParameter = Resolve(parameter.ParameterType);
                    }
                    else
                    {
                        break;
                    }

                    generatedParameters.Add(createdParameter);
                }

                return constructor.Invoke(generatedParameters.ToArray());
            }

            throw new AggregateException("Cannot resolve instance of class");
        }
    }
}