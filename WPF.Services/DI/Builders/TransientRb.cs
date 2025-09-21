namespace WPF.Services;

public class TransientRb : IRegistrationBuilder
{
    public TransientRb(RegistrationProxy proxy) {
        Registration = proxy;
    }
    public RegistrationProxy Registration { get; }

    public virtual object Resolve(Container container) {
        if (Registration.GetImplementation().GetConstructors() is not { Length: 1 } constructorInfos)
            throw new InvalidOperationException($"Service {Registration.GetImplementation().Name} must have a public constructor");
        
        var parameterInfos = constructorInfos[0].GetParameters();
        var parameterInstances = parameterInfos.Select(p => {
            var argParameterType = p.ParameterType;
            return container.Resolve(argParameterType);
        }).ToArray();
        return constructorInfos[0].Invoke(parameterInstances);
    }
}

public class ScopedRd : SingletonRb
{
    public ScopedRd(RegistrationProxy proxy, Type scopeRoot) : base(proxy) {
        ScopeRoot = scopeRoot;
    }
    
    public Type ScopeRoot { get; }
    public bool IsRootActive { get; set; }

    public override object Resolve(Container container) {
        if (!IsRootActive)
            throw new InvalidOperationException($"Scope {ScopeRoot.Name} is not active");
        return base.Resolve(container);
    }
}

