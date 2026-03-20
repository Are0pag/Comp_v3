namespace DI;

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