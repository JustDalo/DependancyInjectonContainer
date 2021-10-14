using System.Collections.Generic;
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
            _dependenciesConfiguration.Register<IService, ServiceImpl2>();
            IEnumerable<IService> services = _dependencyProvider.Resolve<IEnumerable<IService>>();
            Assert.AreEqual(2, services.Count());
        }
    }
    
    interface IService {}
    class ServiceImpl : IService
    {
        public ServiceImpl(IRepository repository) // ServiceImpl зависит от IRepository
        {
           
        }
    }

    class ServiceImpl2 : IService
    {
        public ServiceImpl2()
        {
            
        }
    }

    interface IRepository{}
    class RepositoryImpl : IRepository
    {
        public RepositoryImpl(){} // может иметь свои зависимости, опустим для простоты
    }
}