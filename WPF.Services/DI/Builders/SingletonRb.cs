namespace WPF.Services;

public class SingletonRb : TransientRb
{
    protected object? _instance;
    public SingletonRb(RegistrationProxy proxy) : base(proxy) {
    }

    public override object Resolve(ResolveArgs args) {
        if (_instance is not null) 
            return _instance;

        _instance = base.Resolve(args);
        return _instance;
    }
}