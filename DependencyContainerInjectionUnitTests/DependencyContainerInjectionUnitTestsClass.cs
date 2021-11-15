using System.Collections.Generic;
using System.Linq;
using DependencyInjectionContainer.DependenciesConfiguration.DependenciesConfigurationImpl;
using DependencyInjectionContainer.DependencyProvider.DependencyProviderImpl;
using NUnit.Framework;

namespace DependencyContainerInjectionUnitTests
{
    public class DependencyContainerInjectionUnitTestsClass
    {
        private DependenciesConfiguration _dependenciesConfiguration;
        private DependencyProvider _dependencyProvider;

        [SetUp]
        public void Setup()
        {
            _dependenciesConfiguration = new DependenciesConfiguration();
            _dependenciesConfiguration.Register(typeof(IService), typeof(ServiceImpl), true);
            _dependenciesConfiguration.Register(typeof(IService), typeof(ServiceImpl3), true);
            _dependenciesConfiguration.Register<IRepository, RepositoryImpl>(true);
            _dependencyProvider = new DependencyProvider(_dependenciesConfiguration);
        }

        [Test]
        public void Test1()
        {
            var service = _dependencyProvider.Resolve<IService>();
            Assert.IsTrue(service != null);
        }

        [Test]
        public void Test2()
        {
            var dependencyConfig = new DependenciesConfiguration();
            dependencyConfig.Register<IService, ServiceImpl>();
            dependencyConfig.Register<IRepository, RepositoryImpl>();
            var dependencyProvider = new DependencyProvider(dependencyConfig);
            var service = dependencyProvider.Resolve<IService>();
            Assert.IsTrue(service != null);
        }

        [Test]
        public void Test3()
        {
            IEnumerable<IService> services = _dependencyProvider.Resolve<IEnumerable<IService>>();
            var impl = _dependencyProvider.Resolve<IService>();
            Assert.That(impl, Is.EqualTo(services.ElementAt(0)));
        }

        [Test]
        public void Test4()
        {
            var dependencyConfiguration = new DependenciesConfiguration();
            dependencyConfiguration.Register<IService, ServiceImpl>(true);
            dependencyConfiguration.Register<IService, ServiceImpl3>(true);
            dependencyConfiguration.Register<IRepository, RepositoryImpl>(true);
            var dependencyProvider = new DependencyProvider(dependencyConfiguration);
            var services = dependencyProvider.Resolve<IEnumerable<IService>>();
            var impl = dependencyProvider.Resolve<IService>();
            Assert.That(impl, Is.EqualTo(services.ElementAt(0)));
        }

        [Test]
        public void Test5()
        {
            //   var enumerableInstanceOne = _dependencyProvider.Resolve<IEnumerable<IService>>();
            //   var enumerableInstanceTwo = _dependencyProvider.Resolve<IEnumerable<IService>>();

            //   Assert.IsTrue(enumerableInstanceOne.Equals(enumerableInstanceTwo));
        }

        [Test]
        public void Test6()
        {
            var config = new DependenciesConfiguration();
            config.Register<IMessageSender, chat>();
            config.Register<IRepository, RepositoryImpl>();
            var provider = new DependencyProvider(config);
            var s = provider.Resolve<IMessageSender>();
            Assert.NotNull(s);
        }
    }

    interface IService
    {
    }

    class ServiceImpl : IService
    {
        private IRepository repository;

        public ServiceImpl(IRepository repository)
        {
            this.repository = repository;
        }
    }

    interface IRepository
    {
    }

    class RepositoryImpl : IRepository
    {
        public RepositoryImpl()
        {
        }
    }

    interface IMessageSender
    {
        
    }

    class chat : IMessageSender
    {
        public chat(IRepository something)
        {
            
        }
    }

    class ServiceImpl3 : IService
    {
        
    }
}