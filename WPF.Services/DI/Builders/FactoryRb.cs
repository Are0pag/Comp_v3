namespace WPF.Services;

public class FactoryRb : IRegistrationBuilder
{
    protected readonly IRegistrationBuilder _builder;
    protected readonly Func<object> _factory;

    public FactoryRb(IRegistrationBuilder builder, Func<object> factory) {
        _builder = builder;
        _factory = factory;
    }

    public RegistrationProxy Registration => _builder.Registration;

    public object Resolve(AreopagContainer container) {
        return _factory();
    }
}