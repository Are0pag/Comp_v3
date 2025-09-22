using System.Reflection;
using System.Runtime.CompilerServices;
using Infrastructure;

namespace WPF.Services;

public class Container
{
    protected readonly List<IRegistrationBuilder> _registrationBuilders = new();
    protected readonly Dictionary<Type, List<ScopedRd>> _scopes = new();
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
    
    public Container Add<TService>() where TService : IDisposable{
        if (IsRegistered<TService>())
            new InvalidOperationException("Cannot add service " + typeof(TService).Name + " to container because it is already registered.").Log(this);

        _creatingRegistration = new RegistrationProxy(typeof(TService));
        return this;
    }

    public Container To<TImplementation>() where TImplementation : IDisposable {
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

    public Container AsSingleton() {
        if (_creatingRegistration == null)
            throw new InvalidOperationException();
        _registrationBuilders.Add(new SingletonRb(_creatingRegistration));
        return this;
    }

    public TScopeOwner BeginScope<TScopeOwner>() where TScopeOwner : class, IDisposable {
        if (_scopes.TryGetValue(typeof(TScopeOwner), out var scopeOwners)) {
            throw new InvalidOperationException($"Scope {typeof(TScopeOwner).Name} has already been scoped.");
        }
        
        var targetScopeRegistrations = _registrationBuilders
                                      .OfType<ScopedRd>()                             // Приводим только те, которые можно привести к ScopedRd
                                      .Where(r => r.ScopeRoot == typeof(TScopeOwner)) // Фильтруем по ScopeRoot
                                      .ToList();

        if (!targetScopeRegistrations.Any())
            throw new InvalidOperationException($"No scope registrations found for {typeof(TScopeOwner).Name}");

        foreach (var targetScopeRegistration in targetScopeRegistrations) {
            targetScopeRegistration.IsRootActive = true;
        }
        
        _scopes.Add(typeof(TScopeOwner), targetScopeRegistrations);
        
        // Пусть пользовательский код регистрирует scopeRootType как Transient или как Singleton
        return Resolve<TScopeOwner>();
    }

    public void ReleaseScope<TScopeOwner>() where TScopeOwner : class, IDisposable {
        if (_scopes.TryGetValue(typeof(TScopeOwner), out var scopeRegistrations)) {
            foreach (var scopeRegistration in scopeRegistrations) {
                scopeRegistration.ReleaseInstance();
                scopeRegistration.IsRootActive = false;
            }
        }
        _scopes.Remove(typeof(TScopeOwner));
    }

    public Container AsScoped<TScopeOwner>() where TScopeOwner : class, IDisposable {
        if (_creatingRegistration == null)
            throw new InvalidOperationException();
        _registrationBuilders.Add(new ScopedRd(_creatingRegistration, typeof(TScopeOwner)));
        return this;
    }

    public T Resolve<T>() {
        return (T) Resolve(typeof(T));
    }

    public object Resolve(Type type) {
        if (_registrationBuilders.FirstOrDefault(r => r.Registration.GetRegistration() == type) is not { } builder)
            throw new InvalidOperationException();
        
        return builder.Resolve(new ResolveArgs(this)); // и ему НЕ ВАЖНО который из них!
    }

    public IRegistrationBuilder FindRegistrationBuilderByName(Type type) {
        var caller = _registrationBuilders.FirstOrDefault(r => 
                                                              r.Registration.GetRegistration().FullName == 
                                                              type.FullName);
        return caller;
    }
   
    public bool IsRegistered<T>() {
        return _registrationBuilders.Any(r => r.Registration.GetRegistration() == typeof(T));
    }
    
    public Type GetActualDeclaringType() {
        var stackTrace = new System.Diagnostics.StackTrace();

        // Проходим по фреймам стека, начиная с самых верхних
        for (var i = 1; i < stackTrace.FrameCount; i++) {
            var frame = stackTrace.GetFrame(i);
            var declaringType = frame.GetMethod().DeclaringType;

            if (declaringType?.FullName == typeof(Container).FullName)
                continue;
            
            return declaringType;
        }
        return null;
    }
}