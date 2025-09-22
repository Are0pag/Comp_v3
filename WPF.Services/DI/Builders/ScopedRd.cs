namespace WPF.Services;

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

    public void ReleaseInstance() {
        if (_instance is null)
            throw new InvalidOperationException($"Ну куда ты лезешь ёбана");

        _instance = null;
        //((IDisposable)_instance)?.Dispose();
    }
}