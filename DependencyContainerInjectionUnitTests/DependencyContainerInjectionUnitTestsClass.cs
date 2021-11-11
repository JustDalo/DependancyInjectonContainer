using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DependencyInjectionContainer;
using DependencyInjectionContainer.DependenciesConfiguration.DependenciesConfigurationImpl;
using DependencyInjectionContainer.DependencyProvider.DependencyProviderImpl;
using NUnit.Framework;

namespace DependencyContainerInjectionUnitTests
{
    public class DependencyContainerInjectionUnitTestsClass
    {
        private DependenciesConfiguration _dependenciesConfiguration;
        private DependencyProvider _dependencyProvider;
        
        private DependencyProvider _provider;
        private DependenciesConfiguration _config;
        [SetUp]
        public void Setup()
        {
            _dependenciesConfiguration = new DependenciesConfiguration();
         //   _dependenciesConfiguration.Register<IService<IRepository>, ServiceImpl<IRepository>>(true);
            //_dependenciesConfiguration.Register<IService, ServiceImpl2>(true);
            _dependenciesConfiguration.Register(typeof(IService<>), typeof(ServiceImpl2<>), true);
           // _dependenciesConfiguration.Register<IRepository, RepositoryImpl>();
           // _dependencyProvider = new DependencyProvider(_dependenciesConfiguration); 
            
            // _config = new DependenciesConfiguration();
            // _config.Register<IFooService, FooImplementation<BarImplementation>>(isSingleton: false);
            // _config.Register<IFooService, AnotherFooImplementation>(false);
            // _config.Register<IBarService, BarImplementation>(true);            
            // _config.Register<IBazService<BarImplementation>, AnotherBazImplementation<BarImplementation>>(false);
            // _config.Register(typeof(IBazService<>), typeof(AnotherBazImplementation<>), false);
            //
            // _provider = new DependencyProvider(_config);
        }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       

        [Test]
        public void Test1()
        {
            var service  = _dependencyProvider.Resolve<IService<IRepository>>();
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
            IEnumerable<IService<IRepository>> services = _dependencyProvider.Resolve<IEnumerable<IService<IRepository>>>();
            Assert.AreEqual(2, services.Count());
        }

        [Test]
        public void Test4()
        {
            //   var instanceOne = _dependencyProvider.Resolve<IService>();
            //var instanceTwo = _dependencyProvider.Resolve<IService>();
            
            // Assert.IsTrue(instanceOne.Equals(instanceTwo));
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
            
        }
        /*[Test]
        public void DependencyProviderEnumerableTest()
        {
            IEnumerable<IFooService> fooService = _provider.Resolve<IEnumerable<IFooService>>();
            Assert.IsTrue(fooService != null);
        }

        [Test]
        public void DependencyProviderAnotherBazImplementationTest()
        {
            AnotherBazImplementation<BarImplementation> anotherBaz =
                (AnotherBazImplementation<BarImplementation>)_provider.Resolve<IBazService<BarImplementation>>();

            bool isAnotherBazImplementation = anotherBaz.GetType().Equals(typeof(AnotherBazImplementation<BarImplementation>));

            bool isBarImplementation = anotherBaz.BarService.GetType().Equals(typeof(BarImplementation));

            Assert.IsTrue(isAnotherBazImplementation && isBarImplementation);
        }

       

        [Test]
        public void DependencyProviderSingletonContainerTest()
        {
            var actual = _provider.Resolve<IBarService>();
            Assert.IsNotNull(actual);
        }

        [Test]
        public void DependencyProviderSingletonBarImplementationTest()
        {
            AnotherBazImplementation<BarImplementation> anotherBaz =
                (AnotherBazImplementation<BarImplementation>)_provider.Resolve<IBazService<BarImplementation>>();

            BarImplementation bar = (BarImplementation)_provider.Resolve<IBarService>();

            Assert.AreSame(anotherBaz.BarService, bar);
        }
        */

       
        
        
    }
    
    interface IService<TRepository> where TRepository : IRepository {}
    class ServiceImpl<TRepository> : IService<TRepository> where TRepository : IRepository
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
    
    class ServiceImpl2<TRepository> : IService<TRepository> where TRepository : IRepository
    {
        public ServiceImpl2()
        {
            
        }
    }

    /*public interface IFooService
    {
    }
    public interface IFaker
    {
        // Faker for test
    }
    public interface IBazService<T>
    {
    }
    public interface IBarService
    {
    }
    
    public class FooImplementation<T> : IFooService
    {
        public IBarService BarService { get; set; }

        public IBazService<T> BazService { get; set; }

        public FooImplementation(IBarService barService, IBazService<T> bazService)
        {
            BarService = barService;
            BazService = bazService;
        }
    }
    public class BazImplementation<T> : IBazService<T>
    {
        public IBarService FirstBarService { get; set; }

        public IBarService SecondBarService { get; set; }

        public BazImplementation(IBarService firstBarService, IBarService secondBarService)
        {
            FirstBarService = firstBarService;
            SecondBarService = secondBarService;
        }
    }
    public class BarImplementation : IBarService
    {
        public BarImplementation() { }
    }
    public class AnotherFooImplementation : IFooService
    {
        public IBarService BarService { get; set; }

        public AnotherFooImplementation(IBarService barService)
        {
            BarService = barService;
        }
    }
    public class AnotherBazImplementation<T> : IBazService<T>
    {
        public T GenericParameter { get; set; }

        public T BarService { get; set; }

        public AnotherBazImplementation(T barService)
        {
            BarService = barService;
        }
    }
    public class AnotherBarImplementation : IBarService
    {
        public AnotherBarImplementation(IBarService barService) { }
    }*/
}