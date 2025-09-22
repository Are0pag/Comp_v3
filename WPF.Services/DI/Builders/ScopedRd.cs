namespace WPF.Services;

public class ScopedRd : SingletonRb
{
    public ScopedRd(RegistrationProxy proxy, Type scopeRoot) : base(proxy) {
        ScopeRoot = scopeRoot;
    }

    public Type ScopeRoot { get; }
    public bool IsRootActive { get; set; }

    public override object Resolve(ResolveArgs args) {
        if (!IsRootActive)
            throw new InvalidOperationException($"Scope {ScopeRoot.Name} is not active");

        var caller = args.Container.FindRegistrationBuilderByName(GetActualDeclaringType());
        
        switch (caller) {
            case ScopedRd:
                return base.Resolve(args);
                break;

            case SingletonRb:
                throw new InvalidOperationException($"");
                break;
            
            case TransientRb:
                return base.Resolve(args);
                break;

            default:
                throw new InvalidOperationException($"Scope {ScopeRoot.Name} has not been scoped");
                break;
        }
    }

    public void ReleaseInstance() {
        if (_instance is null)
            throw new InvalidOperationException($"Ну куда ты лезешь ёбана");

        _instance = null;
        //((IDisposable)_instance)?.Dispose();
    }
    
    public Type GetActualDeclaringType() {
        var stackTrace = new System.Diagnostics.StackTrace();

        // Проходим по фреймам стека, начиная с самых верхних
        for (var i = 1; i < stackTrace.FrameCount; i++) {
            var frame = stackTrace.GetFrame(i);
            var declaringType = frame.GetMethod().DeclaringType;

            var assemblyFullName = ScopeRoot.Namespace;

            if (!declaringType!.FullName!.Contains(assemblyFullName))
                continue;
            
            return declaringType;
        }
        return null;
    }
}