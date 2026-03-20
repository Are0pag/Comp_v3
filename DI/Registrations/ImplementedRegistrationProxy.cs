namespace DI.Registrations;

public class ImplementedRegistrationProxy : RegistrationProxy
{
    public ImplementedRegistrationProxy(Type serviceType, Type implementationType) : base(serviceType) {
        if (implementationType is { IsAbstract: true } or { IsInterface: true })
            throw new InvalidOperationException($"Service type {implementationType.Name} cannot be abstract");
        ImplementationType = implementationType;
    }
    
    public Type ImplementationType { get; init; }

    public override Type GetRegistration() {
        return Registration;
    }

    public override Type GetImplementation() {
        return ImplementationType;
    }
}