using System.Reflection;
using Infrastructure;

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


public class RegistrationBuilder
{
    protected readonly ContainerRegistration _registration;
    protected readonly Container _container;

    public RegistrationBuilder(ContainerRegistration registration, Container container) {
        _registration = registration;
        _container = container;
    }


    public RegistrationBuilder To<TImplementation>() {
        return _container.To<TImplementation>();
    }

    public RegistrationBuilder AsTransient() {
        _registration.LifeTime = LifeTime.Transient;

        return this;
    }

    public RegistrationBuilder AsSingleton() {
        _registration.LifeTime = LifeTime.Singleton;

        return this;
    }
}

public class Container
{
    private readonly List<ContainerRegistration> _registrations = new();
    private readonly object _lock = new();

    public void Install() {
        var assembly = Assembly.GetCallingAssembly();
        var installerTypes = assembly.GetTypes()
                                     .Where(t => t.IsSubclassOf(typeof(AbstractInstaller)) && !t.IsAbstract);

        foreach (var installerType in installerTypes) {
            if (Activator.CreateInstance(installerType) is not AbstractInstaller installer)
                throw new InvalidCastException($"Cannot instantiate type {installerType}");
            installer.Install(this);
        }
    }

    
    public RegistrationBuilder Add<TService>() {
        if (IsRegistered<TService>())
            new InvalidOperationException("Cannot add service " + typeof(TService).Name + " to container because it is already registered.").Log(this);

        var registration = new ContainerRegistration(typeof(TService)) {
            LifeTime = LifeTime.Singleton // default
        };
        _registrations.Add(registration);

        return new RegistrationBuilder(registration, this);
    }

    public RegistrationBuilder To<TImplementation>() {
        var lastRegistration = _registrations.LastOrDefault();

        if (lastRegistration == null)
            throw new InvalidOperationException("No service registered to map implementation to");

        if (!lastRegistration.ServiceType.IsAssignableFrom(typeof(TImplementation)))
            throw new InvalidOperationException($"Type {typeof(TImplementation).Name} is not assignable to {lastRegistration.ServiceType.Name}");

        if (typeof(TImplementation).IsAbstract || typeof(TImplementation).IsInterface)
            throw new InvalidOperationException($"Type {typeof(TImplementation).Name} must be a concrete class (cannot be abstract or interface)");

        lastRegistration.ImplementationType = typeof(TImplementation);

        return new RegistrationBuilder(lastRegistration, this);
    }

    public T Resolve<T>() {
        return (T)Resolve(typeof(T));
    }

    private object Resolve(Type serviceType) {
        var registration = _registrations.FirstOrDefault(r => r.ServiceType == serviceType);

        if (registration == null)
            throw new InvalidOperationException($"Service {serviceType.Name} is not registered");

        return GetInstance(registration);
    }

    private object GetInstance(ContainerRegistration registration) {
        // Если уже есть экземпляр (для синглтонов)
        if (registration.Instance != null)
            return registration.Instance;

        if (registration.ImplementationType == null) {
            if (registration.ServiceType is { IsAbstract: true } or { IsInterface: true })
                new InvalidOperationException($"Service type {registration.ServiceType.Name} cannot be abstract").Log(this);

            registration.ImplementationType = registration.ServiceType;
        }

        if (registration.ConstructorInfo == null) {
            var constructors = registration.ImplementationType.GetConstructors();

            if (constructors.Length != 1)
                throw new InvalidOperationException($"Service have invalid count of constructors in {registration.ImplementationType.Name}");
            registration.ConstructorInfo = constructors[0];
        }
        

        var parameters = registration.ConstructorInfo.GetParameters();
        var parameterInstances = parameters.Select(p => Resolve(p.ParameterType)).ToArray();

        var instance = registration.ConstructorInfo.Invoke(parameterInstances);

        // Сохраняем экземпляр для синглтонов
        if (registration.LifeTime == LifeTime.Singleton) {
            lock (_lock) {
                registration.Instance ??= instance;
            }
        }

        return instance;
    }

    // Вспомогательные методы для проверки
    public bool IsRegistered<T>() {
        return _registrations.Any(r => r.ServiceType == typeof(T));
    }
}