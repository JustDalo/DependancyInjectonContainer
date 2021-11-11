namespace DependencyInjectionContainer.DependencyProvider
{
    public interface IDependencyProvider
    {
        TDependency Resolve<TDependency>() where TDependency : class;
    }
}