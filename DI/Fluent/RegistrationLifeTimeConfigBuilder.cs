namespace DI;

public class RegistrationLifeTimeConfigBuilder
{
    protected readonly AreopagContainer _container;

    internal RegistrationLifeTimeConfigBuilder(AreopagContainer container) {
        _container = container;
    }
    
    public RegistrationResolveConfigBuilder OverrideTo<TImplementation>() {
        _container.OverrideTo<TImplementation>();
        return new RegistrationResolveConfigBuilder(_container);
    }

    public RegistrationResolveConfigBuilder AsTransient() {
        _container.AsTransient();
        return new RegistrationResolveConfigBuilder(_container);
    }

    public RegistrationResolveConfigBuilder AsSingleton() {
        _container.AsSingleton();
        return new RegistrationResolveConfigBuilder(_container);
    }

    public RegistrationResolveConfigBuilder AsScoped<TScopeOwner>() where TScopeOwner : class, IDisposable {
        _container.AsScoped<TScopeOwner>();
        return new RegistrationResolveConfigBuilder(_container);
    }
}