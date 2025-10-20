using System.Diagnostics.CodeAnalysis;

namespace DI;

public class RegistrationConfigBuilder
{
    protected readonly AreopagContainer _container;

    internal RegistrationConfigBuilder(AreopagContainer container) {
        _container = container;
    }
    
    public RegistrationLifeTimeConfigBuilder To<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>() {
        _container.To<TImplementation>();
        return new RegistrationLifeTimeConfigBuilder(_container);
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