using System.Reflection;
using Infrastructure;

namespace WPF.Services;

public class Container
{
    private readonly List<ContainerRegistration> _registrations = new();
    protected readonly Dictionary<Type, object> _scopeInstances = new();
    private readonly object _lock = new();
    protected Type? _selectedScopeOwnerType;

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

    public RegistrationBuilder AsScoped<TScopeOwner>() where TScopeOwner : class, IDisposable {
        var lastRegistration = _registrations.LastOrDefault();
        lastRegistration.LifeTime = LifeTime.Scoped;
        lastRegistration.ScopeOwnerType = typeof(TScopeOwner);
        return new RegistrationBuilder(lastRegistration, this);
    }

    public T Resolve<T>() {
        return (T)Resolve(typeof(T));
    }

    public Container Select<TScopeOwner>() where TScopeOwner : class, IDisposable {
        _selectedScopeOwnerType = typeof(TScopeOwner);
        return this;
    }

    private object Resolve(Type serviceType) {
        var registration = _registrations.FirstOrDefault(r => r.ServiceType == serviceType); 
        // допустим один тип = scoped сервис, а не один экземпляр типа для одного scope, а другой для другого

        if (registration.LifeTime == LifeTime.Scoped && _selectedScopeOwnerType == null) {
            new InvalidOperationException("Cannot resolve service " + serviceType.Name + " from a scoped container without pointed on lifetime owner.").Log(this);
        }
        else {
            var ownerRegistration = _registrations.FirstOrDefault(r => r.ServiceType == _selectedScopeOwnerType);
            if (registration.LifeTime == LifeTime.Scoped && _selectedScopeOwnerType != null && ownerRegistration.Instance != null) {
                return GetInstance(registration);
            }
        }

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

    protected virtual object GetScopedInstance(ContainerRegistration registration) {
        lock (_lock) {
            if (_scopeInstances.TryGetValue(registration.ServiceType, out var scopedInstance))
                return scopedInstance;

            var constructor = registration.ImplementationType.GetConstructors().FirstOrDefault();

            if (constructor == null)
                throw new InvalidOperationException($"No public constructor found for {registration.ImplementationType.Name}");

            var parameters = constructor.GetParameters();
            var parameterInstances = parameters.Select(p => Resolve(p.ParameterType)).ToArray();

            var instance = constructor.Invoke(parameterInstances);
            _scopeInstances[registration.ServiceType] = instance;

            return instance;
        }
    }

    public virtual void ReleaseScope(Type scopeOwnerType) {
        lock (_lock) {
            var scopedServices = _registrations
                                .Where(r => r.LifeTime == LifeTime.Scoped && r.ScopeOwnerType == scopeOwnerType)
                                .Select(r => r.ServiceType)
                                .ToList();

            foreach (var serviceType in scopedServices) {
                if (_scopeInstances.TryGetValue(serviceType, out var disposable)) {
                    (disposable as IDisposable)?.Dispose();
                    _scopeInstances.Remove(serviceType);
                }
            }
        }
    }

    // Вспомогательные методы для проверки
    public bool IsRegistered<T>() {
        return _registrations.Any(r => r.ServiceType == typeof(T));
    }
}