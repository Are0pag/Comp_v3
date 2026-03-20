namespace DI;

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