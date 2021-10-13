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
            _dependenciesConfiguration.Register<IBarService, BarImplementation>();

            _dependencyProvider = new DependencyProvider(_dependenciesConfiguration);
        }

        [Test]
        public void Test1()
        {
            _dependencyProvider.Resolve<IBarService>();
        }
    }
    
    public interface IBarService
    {
    }
    
    public class BarImplementation : IBarService
    {
        public BarImplementation() { }
    }
}