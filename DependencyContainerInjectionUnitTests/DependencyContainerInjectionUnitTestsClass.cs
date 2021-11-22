using System;
using System.Collections.Generic;
using System.Linq;
using DependencyInjectionContainer;
using DependencyInjectionContainer.DependenciesConfiguration;
using DependencyInjectionContainer.DependenciesConfiguration.DependenciesConfigurationImpl;
using DependencyInjectionContainer.DependencyProvider.DependencyProviderImpl;
using Moq;
using NUnit.Framework;

namespace DependencyContainerInjectionUnitTests
{
    public class DependencyContainerInjectionUnitTestsClass
    {
        private Mock<IDependencyConfiguration> _dependencyConfigurationMock;


        [SetUp]
        public void Setup()
        {
            _dependencyConfigurationMock = new Mock<IDependencyConfiguration>();
        }

        [Test]
        public void Register_RegisterTwoInterface_ReturnSameNumber()
        {
            var dependencyConfiguration = new DependenciesConfiguration();
            dependencyConfiguration.Register<IService, ServiceImpl>();
            dependencyConfiguration.Register<IService, ServiceImpl2>();
            dependencyConfiguration.Register<IRepository, RepositoryImpl>();

            var actual = dependencyConfiguration.Dependencies.Count;

            const int expected = 2;
            Assert.That(actual, Is.EqualTo(expected));
        }
        
        [Test]
        public void Register_RegisterGenericTypeOfImplementation_RegisterGenericType()
        {   
            var dependencyConfiguration = new DependenciesConfiguration();
            dependencyConfiguration.Register(typeof(IInterface<IRep>), typeof(Ex<IRep>));
            dependencyConfiguration.Register(typeof(IRep), typeof(Rep));

            var actual = dependencyConfiguration.Dependencies.Count;

            const int expected = 2;
            Assert.That(actual, Is.EqualTo(expected));
        }
        
        [Test]
        public void Resolve_RecursiveDependencyCreation_ResolveImplementations()
        {
            _dependencyConfigurationMock.Setup(expr => expr.Dependencies)
                .Returns(new Dictionary<Type, List<DependencyInfo>>
                {
                    {
                        typeof(IService),
                        new List<DependencyInfo>()
                        {
                            new()
                            {
                                ImplementationType = typeof(ServiceImpl),
                                IsSingleton = false
                            },
                            new()
                            {
                                ImplementationType = typeof(ServiceImpl2),
                                IsSingleton = false,
                            }
                        }
                    },
                    {
                        typeof(IRepository),
                        new List<DependencyInfo>()
                        {
                            new()
                            {
                                ImplementationType = typeof(RepositoryImpl),
                                IsSingleton = false
                            }
                        }
                    }
                });

            DependencyProvider dependencyProvider = new DependencyProvider(_dependencyConfigurationMock.Object);
            var actual = dependencyProvider.Resolve<IService>();

            Assert.NotNull(actual);
        }

        [Test]
        public void Resolve_MultipleImplementationOfOneDependency_ReturnIEnumerableWithDifferentImplementations()
        {   
            _dependencyConfigurationMock.Setup(expr => expr.Dependencies)
                .Returns(new Dictionary<Type, List<DependencyInfo>>
                {
                    {
                        typeof(IService),
                        new List<DependencyInfo>()
                        {
                            new()
                            {
                                ImplementationType = typeof(ServiceImpl),
                                IsSingleton = true
                            },
                            new()
                            {
                                ImplementationType = typeof(ServiceImpl2),
                                IsSingleton = true,
                            }
                        }
                    },
                    {
                        typeof(IRepository),
                        new List<DependencyInfo>()
                        {
                            new()
                            {
                                ImplementationType = typeof(RepositoryImpl),
                                IsSingleton = true
                            }
                        }
                    }
                });
            var dependencyProvider = new DependencyProvider(_dependencyConfigurationMock.Object);
            
            var services = dependencyProvider.Resolve<IEnumerable<IService>>();
            var actual = services.Count();
            
            const int expected = 2;
            Assert.That(actual, Is.EqualTo(expected));
            
        }

        [Test]
        public void Resolve_SingletonLifeCycle_DependenciesAreTheSame()
        {
            _dependencyConfigurationMock.Setup(expr => expr.Dependencies)
                .Returns(new Dictionary<Type, List<DependencyInfo>>
                {
                    {
                        typeof(IService),
                        new List<DependencyInfo>()
                        {
                            new()
                            {
                                ImplementationType = typeof(ServiceImpl),
                                IsSingleton = true
                            },
                            new()
                            {
                                ImplementationType = typeof(ServiceImpl2),
                                IsSingleton = true,
                            }
                        }
                    },
                    {
                        typeof(IRepository),
                        new List<DependencyInfo>()
                        {
                            new()
                            {
                                ImplementationType = typeof(RepositoryImpl),
                                IsSingleton = true
                            }
                        }
                    }
                });
            var dependencyProvider = new DependencyProvider(_dependencyConfigurationMock.Object);
            var services = dependencyProvider.Resolve<IEnumerable<IService>>();
            var impl = dependencyProvider.Resolve<IService>();
            Assert.That(impl, Is.EqualTo(services.ElementAt(0)));
        }

        [Test]
        public void Resolve_UnregisteredDependency_ThrowsArgumentException()
        {
            _dependencyConfigurationMock.Setup(expr => expr.Dependencies)
                .Returns(new Dictionary<Type, List<DependencyInfo>>
                {
                    {
                        typeof(IService),
                        new List<DependencyInfo>()
                        {
                            new()
                            {
                                ImplementationType = typeof(ServiceImpl),
                                IsSingleton = false
                            },
                            new()
                            {
                                ImplementationType = typeof(ServiceImpl2),
                                IsSingleton = false,
                            }
                        }
                    }
                });
            var dependencyProvider = new DependencyProvider(_dependencyConfigurationMock.Object);

            void Actual() => dependencyProvider.Resolve<IService>();
            Assert.Throws<ArgumentException>(Actual);
        }


        [Test]
        public void GenericRegistration()
        {
            var config = new DependenciesConfiguration();
            config.Register(typeof(IInterface<IRep>), typeof(Ex<IRep>));
            config.Register(typeof(IRep), typeof(Rep));
            var provider = new DependencyProvider(config);
            var expected = provider.Resolve<IInterface<IRep>>();
            Assert.NotNull(expected);
        }
    }

    interface IService
    {
    }

    class ServiceImpl : IService
    {
        private IRepository _repository;

        public ServiceImpl(IRepository repository)
        {
            _repository = repository;
        }
    }

    class ServiceImpl2 : IService
    {
    }

    interface IRepository
    {
    }

    class RepositoryImpl : IRepository
    {
    }


    public interface IRep
    {
    }

    public class Rep : IRep
    {
    }

    public interface IInterface<TRep> where TRep : IRep
    {
        TRep F { get; }
    }

    public class Ex<TRep> : IInterface<TRep>
        where TRep : IRep
    {
        public TRep F { get; }
        public Ex(TRep TImpl)
        {
            this.F = TImpl;
        }
    }
}