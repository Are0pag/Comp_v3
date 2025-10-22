using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using DI.Builders;
using DI.Registrations;

namespace DI;

public class AreopagContainer : IDisposable
{
    protected readonly List<IRegistrationBuilder> _registrationBuilders = new();
    protected readonly Dictionary<Type, List<ScopedRd>> _scopes = new();
    private readonly HashSet<Type> _resolvingTypes = new();
    protected RegistrationProxy? _creatingRegistration;

    public string Description;
    
    [Obsolete]
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
    
    public RegistrationConfigBuilder Add<TService>() {
        if (IsRegistered<TService>())
            throw new InvalidOperationException("Cannot add service " + typeof(TService).Name + " to container because it is already registered.");

        _creatingRegistration = new RegistrationProxy(typeof(TService));

        return new RegistrationConfigBuilder(this);
    }
    
    public RegistrationConfigBuilder Select<TService>() { 
        if (!IsRegistered<TService>())
            throw new InvalidOperationException($"Can not select registration of type {typeof(TService).Name} because base reg. is not exists");

        _creatingRegistration = new RegistrationProxy(typeof(TService));
        return new RegistrationConfigBuilder(this);
    }

    internal void To<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>()/* where TImplementation : IDisposable*/ {
        if (!_creatingRegistration.GetRegistration().IsAssignableFrom(typeof(TImplementation)))
            throw new InvalidOperationException($"Type {typeof(TImplementation).Name} is not assignable to {_creatingRegistration.GetRegistration().Name}");

        if (typeof(TImplementation).IsAbstract || typeof(TImplementation).IsInterface)
            throw new InvalidOperationException($"Type {typeof(TImplementation).Name} must be a concrete class (cannot be abstract or interface)");

        var builder = new ImplementedRegistrationProxy(_creatingRegistration.GetRegistration(), typeof(TImplementation));
        _creatingRegistration = builder;
    }

    internal void OverrideTo<TImplementation>() {
        if (_creatingRegistration == null)
            throw new InvalidOperationException();
        
        var overridedRegistration = _creatingRegistration.GetRegistration();
        if (_registrationBuilders.First(rb => rb.Registration.GetRegistration() == overridedRegistration) is not {} builder)
            throw new InvalidOperationException();

        builder.OverrideImplementation(new ImplementedRegistrationProxy(overridedRegistration, typeof(TImplementation)));
        _creatingRegistration = null;
    }

    internal void AsTransient() {
        if (_creatingRegistration == null)
            throw new InvalidOperationException();
        _registrationBuilders.Add(new TransientRb(_creatingRegistration));
    }

    internal void AsSingleton() {
        if (_creatingRegistration == null)
            throw new InvalidOperationException();
        _registrationBuilders.Add(new SingletonRb(_creatingRegistration));
    }

    internal void AsScoped<TScopeOwner>() where TScopeOwner : class, IDisposable {
        if (_creatingRegistration == null)
            throw new InvalidOperationException();
        _registrationBuilders.Add(new ScopedRd(_creatingRegistration, typeof(TScopeOwner)));
    }

    internal void UsingFactoryMethod(Func<object> factoryMethod) {
        if (_registrationBuilders.Count == 0)
            throw new InvalidOperationException();
        
        _registrationBuilders[^1].FactoryResolve = factoryMethod;
    }

    public void SetFactoryMethodFor<TService>(Func<object> factoryMethod) {
        if (_registrationBuilders.First(rb => rb.Registration.GetRegistration() == typeof(TService)) is not { } rb) 
            throw new InvalidOperationException();
        rb.FactoryResolve = factoryMethod;
    }

    public void FromParentContainer<TService>(AreopagContainer parentContainer) {
        if (_registrationBuilders.First(rb => rb.Registration.GetRegistration() == typeof(TService)) is not { } rb) 
            throw new InvalidOperationException();
        
        var type = rb.Registration.GetRegistration();
        rb.FactoryResolve = () => {
            if (parentContainer.Resolve(type) is not {} instance) 
                throw new InvalidOperationException();

            return instance;
        };
    }
    
    internal void FromParentContainer(AreopagContainer parentContainer) {
        if (_registrationBuilders.Count == 0)
            throw new InvalidOperationException();

        var type = _registrationBuilders[^1].Registration.GetRegistration();
        _registrationBuilders[^1].FactoryResolve = () => {
            if (parentContainer.Resolve(type) is not {} instance) 
                throw new InvalidOperationException();

            return instance;
        };
    }

    internal void NonLazy() {
        if (_registrationBuilders.Last() is not {} lastRegistrationBuilder) 
            throw new InvalidOperationException();

        Resolve(lastRegistrationBuilder.Registration.GetRegistration());
    }

    internal void EnforceInstantiateOnBegin() {
        if (_registrationBuilders.Last() is not ScopedRd scopedRd) 
            throw new InvalidOperationException();
        
        scopedRd.IsEnforceInstantiate = true;
    }

    public TScopeOwner BeginScope<TScopeOwner>() where TScopeOwner : class, IDisposable {
        if (_scopes.TryGetValue(typeof(TScopeOwner), out var scopeOwners)) {
            throw new InvalidOperationException($"Scope {typeof(TScopeOwner).Name} has already been scoped.");
        }
        var targetScopeRegistrations = _registrationBuilders
                                      .OfType<ScopedRd>()                             
                                      .Where(r => r.ScopeRoot == typeof(TScopeOwner)) 
                                      .ToList();

        if (targetScopeRegistrations.Count == 0)
            throw new InvalidOperationException($"No scope registrations found for {typeof(TScopeOwner).Name}");

        targetScopeRegistrations.ForEach(r => r.IsRootActive = true);

        targetScopeRegistrations
           .Where(r => r.IsEnforceInstantiate)
           .ToList()
           .ForEach(r => {
                try {
                    Resolve(r.Registration.GetRegistration());
                }
                catch (Exception e) {
                    throw new InvalidOperationException($"Could not resolve scope owner for {typeof(TScopeOwner).Name}: {e.Message}");
                }
            });

        _scopes.Add(typeof(TScopeOwner), targetScopeRegistrations);
        
        // Пусть пользовательский код регистрирует scopeRootType как Transient или как Singleton
        return Resolve<TScopeOwner>();
    }

    public void ReleaseScope<TScopeOwner>() where TScopeOwner : class, IDisposable {
        if (_scopes.TryGetValue(typeof(TScopeOwner), out var scopeRegistrationBuilders)) {
            foreach (var scopeRb in scopeRegistrationBuilders) {
                try {
                    scopeRb.ReleaseInstance();
                }
                catch (Exception ex) {
                    throw new Exception(ex.Message, ex);
                }
                scopeRb.IsRootActive = false;
            }
        }
        _scopes.Remove(typeof(TScopeOwner));
    }

    public T Resolve<T>(params object[] parameters) {
        return (T) Resolve(typeof(T));
    }

    public object Resolve(Type type) {
        if (_resolvingTypes.Contains(type))
            throw new InvalidOperationException($"Cyclic dependency detected for type {type.Name}");

        IRegistrationBuilder builder = null;
        foreach (var registration in _registrationBuilders)
            if (registration.Registration.GetRegistration() == type) {
                builder = registration;
                break;
            }

        if (builder == null)
            throw new InvalidOperationException($"No registration found for type {type.FullName}");
        
        try {
            _resolvingTypes.Add(type);
            return builder.Resolve(this); 
        }
        finally {
            _resolvingTypes.Remove(type);
        }
    }
   
    public bool IsRegistered<T>() {
        foreach (var builder in _registrationBuilders)
            if (builder.Registration.GetRegistration() == typeof(T))
                return true;
        return false;
    }


    public virtual void Dispose() {
        foreach (var s in _registrationBuilders.OfType<SingletonRb>()) {
            s.ReleaseInstance();
        }
        foreach (var scoped in _scopes) {
            scoped.Value.ForEach(r => r.ReleaseInstance());
        }
        _scopes.Clear();
    }
}