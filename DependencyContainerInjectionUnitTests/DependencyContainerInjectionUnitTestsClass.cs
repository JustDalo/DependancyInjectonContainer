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
    }
    
    interface IService {}
    class ServiceImpl : IService
    {
        public ServiceImpl(IRepository repository) // ServiceImpl зависит от IRepository
        {
           
        }
    }

    interface IRepository{}
    class RepositoryImpl : IRepository
    {
        public RepositoryImpl(){} // может иметь свои зависимости, опустим для простоты
    }
}