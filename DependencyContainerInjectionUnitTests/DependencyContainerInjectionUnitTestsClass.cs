using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DependencyInjectionContainer;
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
            _dependenciesConfiguration.Register<IService, ServiceImpl>();
           // _dependenciesConfiguration.Register<IService, ServiceImpl2>();
            _dependenciesConfiguration.Register<IRepository, RepositoryImpl>();
            _dependencyProvider = new DependencyProvider(_dependenciesConfiguration);
        }

        [Test]
        public void Test1()
        {
            var service  = _dependencyProvider.Resolve<IService>();
            Assert.IsTrue(service != null);
        }

        [Test]
        public void Test2()
        {
            
            var repository = _dependencyProvider.Resolve<IRepository>();
            Assert.IsTrue(repository != null);
            
        }

        [Test]
        public void Test3()
        {
            IEnumerable<IService> services = _dependencyProvider.Resolve<IEnumerable<IService>>();
            Assert.AreEqual(2, services.Count());
        }

        [Test]
        public void Test4()
        {
            var instanceOne = _dependencyProvider.Resolve<IService>();
            var instanceTwo = _dependencyProvider.Resolve<IService>();
            
            Assert.IsTrue(instanceOne != instanceTwo);
        }
    }
    
    interface IService {}
    class ServiceImpl : IService
    {
        private IRepository repository;
        public ServiceImpl(IRepository repository) // ServiceImpl зависит от IRepository
        {
            this.repository = repository;
        }
    }

    interface IRepository{}
    class RepositoryImpl : IRepository
    {
        public RepositoryImpl(){} // может иметь свои зависимости, опустим для простоты
    }
    
    class ServiceImpl2 : IService
    {
        public ServiceImpl2()
        {
            
        }
    }
}