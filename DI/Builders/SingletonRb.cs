using DI.Registrations;

namespace DI.Builders;

public class SingletonRb : TransientRb
{
    protected object? _instance;
    public SingletonRb(RegistrationProxy proxy) : base(proxy) {
    }

    public override object Resolve(AreopagContainer container) {
        if (_instance is not null) 
            return _instance;

        _instance = base.Resolve(container);
        return _instance;
    }

    public virtual void ReleaseInstance() {
        switch (_instance) {
            case null:
                return;
            case IDisposable disposable:
                disposable.Dispose();
                break;
        }

        _instance = null;
    }
}