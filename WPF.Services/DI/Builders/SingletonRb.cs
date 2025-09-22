namespace WPF.Services;

public class SingletonRb : TransientRb
{
    protected object? _instance;
    public SingletonRb(RegistrationProxy proxy) : base(proxy) {
    }

    public override object Resolve(Container container) {
        if (_instance is not null) 
            return _instance;

        _instance = base.Resolve(container);
        return _instance;
    }

    public virtual void ReleaseInstance() {
        if (_instance is null)
            throw new InvalidOperationException($"Ну куда ты лезешь ёбана");

        _instance = null;
    }
}