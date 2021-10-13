namespace DependencyInjectionContainer
{
    public class DependencyProvider
    {
        private DependenciesConfiguration _dependenciesConfiguration;
        public DependencyProvider(DependenciesConfiguration dependenciesConfiguration)
        {
            this._dependenciesConfiguration = dependenciesConfiguration;
        }

        public void Resolve<TDependency>() where TDependency : class
        {
               
        }
    }
}