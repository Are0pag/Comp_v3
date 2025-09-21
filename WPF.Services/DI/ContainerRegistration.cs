using System.Reflection;

namespace WPF.Services;

public abstract class AbstractInstaller
{
    public abstract void Install(Container container);
}

public enum LifeTime
{
    Transient,
    Singleton
}

public class ContainerRegistration
{
    public ContainerRegistration(Type serviceType) {
        ServiceType = serviceType;
    }

    public Type ServiceType { get; set; }
    public Type? ImplementationType { get; set; }
    public LifeTime LifeTime { get; set; }
    public object? Instance { get; set; }
    public ConstructorInfo? ConstructorInfo { get; set; }
}