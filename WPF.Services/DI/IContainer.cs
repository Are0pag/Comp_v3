using System.Reflection;
using Infrastructure;

namespace WPF.Services;

public class Container : IDisposable
{
    protected readonly List<IRegistrationBuilder> _registrationBuilders = new();
    protected readonly Dictionary<Type, List<ScopedRd>> _scopes = new();
    protected RegistrationProxy? _creatingRegistration;
    private readonly HashSet<Type> _resolvingTypes = new();

    public Dictionary<Type, List<(Type, object)>> RuntimeParameters { get; protected set; } = new();
    
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

    public Container WithParameters(params Type[] parameterTypes) {
        if (_registrationBuilders.LastOrDefault() is not {} lastRegistrationBuilder) 
            throw new InvalidOperationException();

        var key = lastRegistrationBuilder.Registration.GetRegistration();
        RuntimeParameters[key] = new List<(Type, object)>();
        
        foreach (var parameterType in parameterTypes) {
            RuntimeParameters[key].Add((parameterType, null));
        }
        return this;
    }

    public T Resolve<T>(params object[] parameters) {
        if (parameters.Length != 0) {
            if (RuntimeParameters.TryGetValue(typeof(T), out var registrationParams)) {
                if (parameters.Length != registrationParams.Count) {
                    throw new ArgumentException("Количество параметров не соответствует зарегистрированным типам");
                }
                var updatedParams = new List<(Type, object)>();
                for (var i = 0; i < registrationParams.Count; i++) {
                    // Проверяем совместимость типов
                    if (!registrationParams[i].Item1.IsInstanceOfType(parameters[i])) 
                        throw new ArgumentException($"Тип параметра {i} не соответствует зарегистрированному типу");

                    // Добавляем кортеж с проставленным значением
                    updatedParams.Add((registrationParams[i].Item1, parameters[i]));
                }
                RuntimeParameters[typeof(T)] = updatedParams;
            }
        }
        
        return (T) Resolve(typeof(T));
    }

    public object Resolve(Type type) {
        if (_resolvingTypes.Contains(type))
            throw new InvalidOperationException($"Cyclic dependency detected for type {type.Name}");

        if (_registrationBuilders.FirstOrDefault(r => r.Registration.GetRegistration() == type) is not { } builder)
            throw new InvalidOperationException();
        
        try {
            _resolvingTypes.Add(type);
            return builder.Resolve(this); 
        }
        finally {
            _resolvingTypes.Remove(type);
        }
    }

   
    public bool IsRegistered<T>() {
        return _registrationBuilders.Any(r => r.Registration.GetRegistration() == typeof(T));
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