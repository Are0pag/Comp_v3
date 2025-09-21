using Infrastructure;

namespace WPF.Services;

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
            new InvalidOperationException($"Service type {Registration.Name} cannot be abstract").Log(this);
        return GetRegistration();
    }
}