namespace WPF.Services;

public class FactoryRb : IRegistrationBuilder
{
    protected readonly IRegistrationBuilder _builder;
    //protected readonly Func<object> _factory;

    public FactoryRb(IRegistrationBuilder builder, Func<object> factory) {
        _builder = builder;
        _builder.FactoryResolve = factory;
        //_factory = factory;
    }

    public RegistrationProxy Registration => _builder.Registration;
    public Func<object>? FactoryResolve { get; set; }

    public object Resolve(AreopagContainer container) {
        return _builder.Resolve(container);
    }
}