using System.Reflection;
using Infrastructure;

namespace WPF.Services;

public class Container
{
    protected readonly List<IRegistrationBuilder> _registrationBuilders = new();
    protected RegistrationProxy? _creatingRegistration;

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
    
    public Container Add<TService>() {
        if (IsRegistered<TService>())
            new InvalidOperationException("Cannot add service " + typeof(TService).Name + " to container because it is already registered.").Log(this);

        _creatingRegistration = new RegistrationProxy(typeof(TService));
        return this;
    }

    public Container To<TImplementation>() {
        if (!_creatingRegistration.GetRegistration().IsAssignableFrom(typeof(TImplementation)))
            throw new InvalidOperationException($"Type {typeof(TImplementation).Name} is not assignable to {_creatingRegistration.GetRegistration().Name}");

        if (typeof(TImplementation).IsAbstract || typeof(TImplementation).IsInterface)
            throw new InvalidOperationException($"Type {typeof(TImplementation).Name} must be a concrete class (cannot be abstract or interface)");

        var builder = new ImplementedRegistrationProxy(_creatingRegistration.GetRegistration(), typeof(TImplementation));
        _creatingRegistration = builder;
        return this;
    }

    public Container AsTransient() {
        if (_creatingRegistration == null)
            throw new InvalidOperationException();
        _registrationBuilders.Add(new TransientRb(_creatingRegistration));
        return this;
    }

    /*public Container AsSingleton() {
        if (_creatingRegistration == null)
            throw new InvalidOperationException();
        
    }*/

    /*public Container AsScoped<TScopeOwner>() where TScopeOwner : class, IDisposable {
        _registrations[^1] = new 
        lastRegistration.ScopeOwnerType = typeof(TScopeOwner);
        return this;
    }*/

    public T Resolve<T>() {
        return (T) Resolve(typeof(T));
    }

    public object Resolve(Type type) {
        if (_registrationBuilders.FirstOrDefault(r => r.Registration.GetRegistration() == type) is not { } builder)
            throw new InvalidOperationException();
        
        return builder.Resolve(this); // и ему НЕ ВАЖНО который из них!
    }

    public Container SelectScope<TScopeOwner>() where TScopeOwner : class, IDisposable {
        _selectedScopeOwnerType = typeof(TScopeOwner);
        return this;
    }
    
    public Container SelectScope(Type scopeOwnerType) {
        _selectedScopeOwnerType = scopeOwnerType;
        return this;
    }
    
    /*private object Resolve(Type serviceType) {
        var registration = _registrations.FirstOrDefault(r => r.ServiceType == serviceType); 
        // допустим один тип = scoped сервис, а не один экземпляр типа для одного scope, а другой для другого

        if (registration == null)
            throw new InvalidOperationException($"Service {serviceType.Name} is not registered");
        
        if (registration.LifeTime == LifeTime.Scoped && _selectedScopeOwnerType == null) {
            new InvalidOperationException("Cannot resolve service " + serviceType.Name + " from a scoped container without pointed on lifetime owner.").Log(this);
        }
        else if (registration.LifeTime == LifeTime.Scoped && _selectedScopeOwnerType != null) {
            var ownerRegistration = _registrations.FirstOrDefault(r => r.ServiceType == _selectedScopeOwnerType);
            if (ownerRegistration.Instance != null) {
                _selectedScopeOwnerType = null;
                return GetInstance(registration);
            }
            new InvalidOperationException($"Can not resolve scoped service because owner of type {_selectedScopeOwnerType.Name} was not registered.").Log(this);
        }
        return GetInstance(registration);
    }*/
    
    /*private object GetInstance(Registration registration) {
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

        object[] parameterInstances;
        var exsistObjectWhereThisIsScopeParent = _registrations.Any(r => r.ScopeOwnerType == registration.ServiceType);
        bool isScoped = registration.LifeTime == LifeTime.Scoped;
        
        if (!isScoped && !exsistObjectWhereThisIsScopeParent)
            parameterInstances = parameters.Select(p => Resolve(p.ParameterType)).ToArray();
        else {
            parameterInstances = parameters.Select(p =>
                                                       SelectScope(exsistObjectWhereThisIsScopeParent ? 
                                                                       registration.ServiceType : 
                                                                       registration.ScopeOwnerType)
                                                          .Resolve(p.ParameterType)).ToArray();
        }

        var instance = registration.ConstructorInfo.Invoke(parameterInstances);

        // Сохраняем экземпляр для синглтонов
        if (registration.LifeTime == LifeTime.Singleton) {
            lock (_lock) {
                registration.Instance ??= instance;
            }
        }

        return instance;
    }*/

    /*public virtual void ReleaseScope(Type scopeOwnerType) {
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
    }*/

   
    public bool IsRegistered<T>() {
        return _registrationBuilders.Any(r => r.Registration.GetRegistration() == typeof(T));
    }
}