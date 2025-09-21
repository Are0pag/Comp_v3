namespace WPF.Services;

public class RegistrationBuilder
{
    protected readonly ContainerRegistration _registration;
    protected readonly Container _container;

    public RegistrationBuilder(ContainerRegistration registration, Container container) {
        _registration = registration;
        _container = container;
    }


    public RegistrationBuilder To<TImplementation>() {
        return _container.To<TImplementation>();
    }

    public RegistrationBuilder AsTransient() {
        _registration.LifeTime = LifeTime.Transient;

        return this;
    }

    public RegistrationBuilder AsSingleton() {
        _registration.LifeTime = LifeTime.Singleton;

        return this;
    }

    public RegistrationBuilder AsScoped<TScopeOwner>() where TScopeOwner : class, IDisposable {
        return this;
    }
}