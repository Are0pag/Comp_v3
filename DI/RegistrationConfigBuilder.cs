using System.Diagnostics.CodeAnalysis;

namespace DI;

public class RegistrationConfigBuilder
{
    protected readonly AreopagContainer _container;

    internal RegistrationConfigBuilder(AreopagContainer container) {
        _container = container;
    }
    
    public RegistrationResolveConfigBuilder To<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>() {
        _container.To<TImplementation>();
        return new RegistrationResolveConfigBuilder(_container);
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

public class RegistrationResolveConfigBuilder
{
    protected readonly AreopagContainer _container;

    internal RegistrationResolveConfigBuilder(AreopagContainer container) {
        _container = container;
    }
    
    public AdditionalRegistrationResolveConfigBuilder UsingFactoryMethod(Func<object> factoryMethod) {
        _container.UsingFactoryMethod(factoryMethod);
        return new AdditionalRegistrationResolveConfigBuilder(_container);
    }

    public AdditionalRegistrationResolveConfigBuilder SetFactoryMethodFor<TService>(Func<object> factoryMethod) {
        _container.SetFactoryMethodFor<TService>(factoryMethod);
        return new AdditionalRegistrationResolveConfigBuilder(_container);
    }

    public AdditionalRegistrationResolveConfigBuilder FromParentContainer(AreopagContainer parentContainer) {
        _container.FromParentContainer(parentContainer);
        return new AdditionalRegistrationResolveConfigBuilder(_container);
    }

    public void NonLazy() {
        _container.NonLazy();
    }

    public void EnforceInstantiateOnBegin() {
        _container.EnforceInstantiateOnBegin();
    }
}

public class AdditionalRegistrationResolveConfigBuilder
{
    protected readonly AreopagContainer _container;

    internal AdditionalRegistrationResolveConfigBuilder(AreopagContainer container) {
        _container = container;
    }
    
    public void NonLazy() {
        _container.NonLazy();
    }

    public void EnforceInstantiateOnBegin() {
        _container.EnforceInstantiateOnBegin();
    }
}