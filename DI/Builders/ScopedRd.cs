using DI.Registrations;

namespace DI.Builders;

public class ScopedRd : SingletonRb
{
    public ScopedRd(RegistrationProxy proxy, Type scopeRoot) : base(proxy) {
        ScopeRoot = scopeRoot;
    }

    public Type ScopeRoot { get; }
    public bool IsRootActive { get; set; }
    
    public bool IsEnforceInstantiate { get; set; } = false;

    public override object Resolve(AreopagContainer container) {
        if (!IsRootActive)
            throw new InvalidOperationException($"Scope {ScopeRoot.Name} is not active");

        var instance = base.Resolve(container);
        return instance;
    }
}