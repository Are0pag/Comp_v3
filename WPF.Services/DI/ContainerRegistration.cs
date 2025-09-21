using System.Reflection;

namespace WPF.Services;

public abstract class AbstractInstaller
{
    public abstract void Install(Container container);
}

public enum LifeTime
{
    Transient,
    Singleton,
    Scoped
}

public class ContainerRegistration
{
    public ContainerRegistration(Type serviceType) {
        ServiceType = serviceType;
    }

    public Type ServiceType { get; set; }                 // base
    public Type? ImplementationType { get; set; }         // base
    public ConstructorInfo? ConstructorInfo { get; set; } // base
    public object? Instance { get; set; }                 // singleton & scoped
    public Type ScopeOwnerType { get; set; }              // scoped
    public LifeTime LifeTime { get; set; }                // to objects 
}

