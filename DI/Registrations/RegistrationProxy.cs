namespace DI.Registrations;

public class RegistrationProxy
{
    public RegistrationProxy(Type registration) {
        Registration = registration;
    }

    protected Type Registration { get; init; }

    public virtual Type GetRegistration() {
        return Registration;
    }

    public virtual Type GetImplementation() {
        if (Registration is { IsAbstract: true } or { IsInterface: true })
            throw new InvalidOperationException($"Service type {Registration.Name} cannot be abstract");
        return GetRegistration();
    }
}